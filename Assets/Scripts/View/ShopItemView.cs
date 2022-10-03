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
    Inventory _inventory;

    Action<ShopItemModel> _onClickedEvent;

    AdsGameService _adsService = null;
    IIAPService _iapService = null;

    public void SetData(ShopItemModel model, Inventory inventory, Action<ShopItemModel> onClickedEvent)
    {
        _adsService = ServiceLocator.GetService<AdsGameService>();
        _iapService = ServiceLocator.GetService<IIAPService>();

        _model = model;
        _inventory = inventory;
        _onClickedEvent = onClickedEvent;

        _inventory.OnResourceModified += InventoryUpdate;

        UpdateVisuals();
    }

    private void OnDestroy()
    {
        if (_inventory != null)
        {
            _inventory.OnResourceModified -= InventoryUpdate;
        }
    }

    void InventoryUpdate(string resource)
    {
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        if (_model == null) return;

        _cost.color = UserCanPay() ? Color.white : Color.red;

        _image.sprite = _imageSprites.Find(sprite => sprite.name == _model.Image);
        _title.text = _model.Name;
        _costImage.sprite = _model.IsObtainedWithAd 
            ? _costSprites.Find(sprite => sprite.name == "AdCost") 
            : _costSprites.Find(sprite => sprite.name == _model.Cost.Type);

        if (_model.IsObtainedWithAd)
        {
            _itemButton.interactable =  false;
            StartCoroutine(WaitForAdToLoad());
        }
        else if (_model.IsObtainedWithIAP)
        {
            _itemButton.interactable = false;
            StartCoroutine(WaitForIAPReady());
        } 
        else
        {
            _itemButton.interactable = UserCanPay() ? true : false;
            _cost.text = _model.Cost.Amount.ToString();
        }
    }

    bool UserCanPay()
    {
        if (_model.Reward.Type.Contains("icon"))
        {
            foreach (InventoryItem i in _inventory.GetInventoryItems())
            {
                if (i.Type == _model.Reward.Type || _model.Cost.Amount > _inventory.GetAmount(_model.Cost.Type))
                    return false;
            }
        } 

        if (_model.Cost.Amount > _inventory.GetAmount(_model.Cost.Type) && !_model.IsObtainedWithAd) {
            return false;
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
        _cost.text = _iapService.GetLocalizedPrice("test1");
    }

    public void OnClicked()
    {
        if (_model != null)
        {
            _onClickedEvent?.Invoke(_model);
        }
    }
}
