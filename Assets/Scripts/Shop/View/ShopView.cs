using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.Services;

public class ShopView : MonoBehaviour
{
    [SerializeField] ShopItemView _shopItemPrefab = null;
    [SerializeField] GameObject _itemsParent = null;
    [SerializeField] GameObject _premiumItemsParent = null;
    [SerializeField] CanvasGroup _goldShopCanvasGroup = null;
    [SerializeField] CanvasGroup _diamondShopCanvasGroup = null;
    [SerializeField] TMP_Text _goldText = null;
    [SerializeField] TMP_Text _diamondText = null;
    [SerializeField] TMP_Text _bombBoosterText = null;
    [SerializeField] TMP_Text _colorBombBoosterText = null;

    ShopController _controller;
    Inventory _inventory;

    int _transactionsDone;

    AnalyticsGameService _analyticsService = null;
    AdsGameService _adsService = null;

    public void Initialize(ShopController controller, Inventory inventory)
    {
        _analyticsService = ServiceLocator.GetService<AnalyticsGameService>();
        _adsService = ServiceLocator.GetService<AdsGameService>();

        _controller = controller;
        _inventory = inventory;

        _inventory.OnResourceModified += UpdateResource;

        while (_itemsParent.transform.childCount > 0)
        {
            Transform child = _itemsParent.transform.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        foreach (ShopItemModel shopItemModel in _controller.Model.Items)
        {
            Instantiate(_shopItemPrefab, _itemsParent.transform).SetData(shopItemModel, inventory, OnPurchaseItem);
        }

        foreach (ShopItemModel shopItemModel in _controller.Model.PremiumItems)
        {
            Instantiate(_shopItemPrefab, _premiumItemsParent.transform).SetData(shopItemModel, inventory, OnPurchaseItem);
        }

        UpdateMenuData();
    }

    private void OnDestroy()
    {
        _analyticsService.SendEvent("storeClosed", new Dictionary<string, object>
        {
            { "transactionsPerformed", _transactionsDone }
        });
        _inventory.OnResourceModified -= UpdateResource;
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
            if (await ServiceLocator.GetService<AdsGameService>().ShowAd())
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
        UpdateResource("Gold");
        UpdateResource("Diamond");
        UpdateResource("Bomb Booster");
        UpdateResource("Color Bomb Booster");
    }

    void UpdateResource(string resource)
    {
        switch (resource)
        {
            case "Gold":
                _goldText.text = _inventory.GetAmount("Gold").ToString();
                break;
            case "Diamond":
                _diamondText.text = _inventory.GetAmount("Diamond").ToString();
                break;
            case "Bomb Booster":
                _bombBoosterText.text = _inventory.GetAmount("Bomb Booster").ToString();
                break;
            case "Color Bomb Booster":
                _colorBombBoosterText.text = _inventory.GetAmount("Color Bomb Booster").ToString();
                break;
        }
    }
}
