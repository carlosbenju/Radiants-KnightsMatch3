using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Services;
using System.Collections;

public class ShopItemView : MonoBehaviour
{
    [SerializeField] Button _itemButton = null;
    [SerializeField] List<Sprite> _imageSprites = new List<Sprite>();
    [SerializeField] List<Sprite> _costSprites = new List<Sprite>();

    [SerializeField] Image _image = null;
    [SerializeField] Image _costImage = null;

    [SerializeField] TextMeshProUGUI _title = null;
    [SerializeField] TextMeshProUGUI _cost = null;

    ShopItemModel _model;

    Action<ShopItemModel> _onClickedEvent;

    ResourceInventoryProgression _resourceProgression;
    IconCollectibleProgression _iconProgression;

    AdsGameService _adsService = null;
    IAPGameService _iapService = null;

    public void SetData(ShopItemModel model, ResourceInventoryProgression resourceProgression,IconCollectibleProgression iconProgression, Action<ShopItemModel> onClickedEvent)
    {
        _adsService = ServiceLocator.GetService<AdsGameService>();
        _iapService = ServiceLocator.GetService<IAPGameService>();

        _resourceProgression = resourceProgression;
        _resourceProgression.OnResourceModified += InventoryUpdate;

        _iconProgression = iconProgression;

        _model = model;
        _onClickedEvent = onClickedEvent;

        UpdateVisuals();
    }

    private void OnDestroy()
    {
        _resourceProgression.OnResourceModified -= InventoryUpdate;
    }

    void InventoryUpdate(string resource)
    {
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        if (_model == null) return;

        bool canPay = UserCanPay();

        _cost.color = canPay ? Color.white : Color.red;

        _image.sprite = _imageSprites.Find(sprite => sprite.name == _model.Image);
        _title.text = _model.Id;
        _costImage.sprite = _model.IsObtainedWithAd 
            ? _costSprites.Find(sprite => sprite.name == "AdCost") 
            : _costSprites.Find(sprite => sprite.name == _model.CostType);

        if (_model.IsObtainedWithAd)
        {
            _itemButton.interactable =  false;
            StartCoroutine(WaitForAdToLoad());
            _costImage.sprite = _costSprites.Find(sprite => sprite.name == "AdCost");
        }
        else if (_model.IsObtainedWithIAP)
        {
            _itemButton.interactable = false;
            StartCoroutine(WaitForIAPReady());
            _costImage.gameObject.SetActive(false);
            _cost.text = _iapService.GetLocalizedPrice(_model.Id);
        } 
        else
        {
            _itemButton.interactable = canPay ? true : false;
            _cost.text = _model.CostAmount == 0 ? "Free!" : _model.CostAmount.ToString();
            _costImage.sprite = _costSprites.Find(sprite => sprite.name == _model.CostType);
        }
    }

    bool UserCanPay()
    {
        if (_model.CostAmount > _resourceProgression.GetResourceAmount(_model.CostType) && !_model.IsObtainedWithAd)
        {
            return false;
        }

        if (_model.RewardType == "Icon")
        {
            IconCollectible iconToFind = _iconProgression.GetIcon(_model.RewardName);
            if (iconToFind != null)
            {
                return false;
            }

            return true;
        }

        return true;
    }

    IEnumerator WaitForAdToLoad()
    {
        while(!_adsService.IsAdReady)
        {
            yield return new WaitForSeconds(0.5f);
        }

        _itemButton.interactable = true;
        _cost.text = string.Empty;
    }

    IEnumerator WaitForIAPReady()
    {
        _cost.text = "Loading";

        while (!_iapService.IsReady())
        {
            yield return new WaitForSeconds(0.5f);
        }

        _itemButton.interactable = true;
    }

    public void OnClicked()
    {
        if (_model != null)
        {
            _onClickedEvent?.Invoke(_model);
        }
    }
}
