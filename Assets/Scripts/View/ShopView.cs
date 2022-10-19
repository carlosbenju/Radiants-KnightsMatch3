using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.Services;
using UnityEngine.AddressableAssets;

public class ShopView : MonoBehaviour
{
    [SerializeField] ShopItemView _shopItemPrefab = null;
    [SerializeField] GameObject _itemsParent = null;
    [SerializeField] GameObject _premiumItemsParent = null;
    [SerializeField] CanvasGroup _goldShopCanvasGroup = null;
    [SerializeField] CanvasGroup _diamondShopCanvasGroup = null;

    [SerializeField] Transform _resourcesParent;
    [SerializeField] ResourceView _resourceView;
    List<ResourceView> _resourceViews = new List<ResourceView>();

    [SerializeField] Transform _boostersParent;
    [SerializeField] BoosterView _boosterView;
    List<BoosterView> _boostersViews = new List<BoosterView>();

    ShopController _controller;

    int _transactionsDone;

    GameProgressionService _progressionService;
    ResourceInventoryProgression _resourceProgression;
    IconCollectibleProgression _iconCollectibleProgression;
    BoostersInventoryProgression _boosterProgression;
    AnalyticsGameService _analyticsService = null;
    AdsGameService _adsService = null;
    IAPGameService _iapService = null;

    public void Initialize(GameProgressionService gameProgression, ShopController controller)
    {
        _progressionService = gameProgression;
        _resourceProgression = _progressionService.ResourceProgression;
        _iconCollectibleProgression = _progressionService.IconProgression;
        _boosterProgression = _progressionService.BoostersProgression;

        _analyticsService = ServiceLocator.GetService<AnalyticsGameService>();
        _adsService = ServiceLocator.GetService<AdsGameService>();
        _iapService = ServiceLocator.GetService<IAPGameService>();

        _resourceProgression.OnResourceModified += UpdateResource;
        _boosterProgression.OnBoosterModified += UpdateResource;
        _controller = controller;

        InstantiateResourceViews();
        InstantiateBoosterViews();

        while (_itemsParent.transform.childCount > 0)
        {
            Transform child = _itemsParent.transform.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        foreach (ShopItemModel shopItemModel in _controller.Config.RegularShopItems)
        {
            Instantiate(_shopItemPrefab, _itemsParent.transform).SetData(shopItemModel, _resourceProgression, _iconCollectibleProgression, OnPurchaseItem);
        }

        foreach (ShopItemModel iapItemModel in _controller.Config.IAPShopItems)
        {
            Instantiate(_shopItemPrefab, _premiumItemsParent.transform).SetData(iapItemModel, _resourceProgression, _iconCollectibleProgression, OnPurchaseItem);
        }

        UpdateMenuData();
    }

    private void OnDestroy()
    {
        _analyticsService.SendEvent("storeClosed", new Dictionary<string, object>
        {
            { "transactionsPerformed", _transactionsDone }
        });
        _resourceProgression.OnResourceModified -= UpdateResource;
        _boosterProgression.OnBoosterModified -= UpdateResource;
    }

    private void InstantiateResourceViews()
    {
        foreach (InGameResourceConfig resource in _resourceProgression.Config.Resources)
        {
            ResourceView view = Instantiate(_resourceView, _resourcesParent);
            view.ResourceType = resource.Id;
            view.Amount.text = _resourceProgression.GetResourceAmount(resource.Id).ToString();
            _resourceViews.Add(view);

            Addressables.LoadAssetAsync<Sprite>(resource.AssetName).Completed += handle =>
            {
                view.Icon.sprite = handle.Result;
            };

            Addressables.LoadAssetAsync<Sprite>(resource.AssetName + "Bar").Completed += handle =>
            {
                view.Background.sprite = handle.Result;
            };
        }
    }

    private void InstantiateBoosterViews()
    {
        foreach (BoosterConfig booster in _boosterProgression.Config.Boosters)
        {
            BoosterView view = Instantiate(_boosterView, _boostersParent);
            view.BoosterType = booster.Id;
            view.Amount.text = _resourceProgression.GetResourceAmount(booster.Id).ToString();
            _boostersViews.Add(view);

            Addressables.LoadAssetAsync<Sprite>(booster.AssetName).Completed += handle =>
            {
                view.Icon.sprite = handle.Result;
            };
        }
    }

    public void MoveToPremiumShopSection()
    {
        _goldShopCanvasGroup.alpha = 0;
        _goldShopCanvasGroup.blocksRaycasts = false;
        _goldShopCanvasGroup.interactable = false;
        _diamondShopCanvasGroup.alpha = 1;
        _diamondShopCanvasGroup.blocksRaycasts = true;
        _diamondShopCanvasGroup.interactable = true;
    }

    public void MoveToNormalShopSection()
    {
        _diamondShopCanvasGroup.alpha = 0;
        _diamondShopCanvasGroup.blocksRaycasts = false;
        _diamondShopCanvasGroup.interactable = false;
        _goldShopCanvasGroup.alpha = 1;
        _goldShopCanvasGroup.blocksRaycasts = true;
        _goldShopCanvasGroup.interactable = true;
    }

    async void OnPurchaseItem(ShopItemModel model)
    {
        if (model.IsObtainedWithAd)
        {
            if (await _adsService.ShowAd())
            {
                _controller.PurchaseItem(model);
                _transactionsDone++;
            }

            return;
        }

        if (model.IsObtainedWithIAP)
        {
            if (await _iapService.StartPurchase(model.Id))
            {
                _controller.PurchaseItem(model);
                _transactionsDone++;
            }

            return;
        }

        _controller.PurchaseItem(model);
        _transactionsDone++;
    }

    void UpdateMenuData()
    {
        _resourceViews.ForEach(r => r.Amount.text = _resourceProgression.GetResourceAmount(r.ResourceType).ToString());
        _boostersViews.ForEach(r => r.Amount.text = _boosterProgression.GetBoosterAmount(r.BoosterType).ToString());
    }

    void UpdateResource(string resourceId)
    {
        ResourceView resourceView = _resourceViews.Find(r => r.ResourceType == resourceId);
        if (resourceView != null)
        {
            resourceView.Amount.text = _resourceProgression.GetResourceAmount(resourceId).ToString();
            return;
        }

        BoosterView boosterView = _boostersViews.Find(b => b.BoosterType == resourceId);
        if (boosterView != null)
        {
            boosterView.Amount.text = _boosterProgression.GetBoosterAmount(resourceId).ToString();
            return;
        }
    }
}
