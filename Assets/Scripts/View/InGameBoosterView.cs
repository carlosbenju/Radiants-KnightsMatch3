using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameBoosterView : MonoBehaviour
{
    [SerializeField] Button _bombButton;
    [SerializeField] Button _colorBombButton;

    [SerializeField] Image _bombBoosterImage = null;
    [SerializeField] TextMeshProUGUI _bombBoosterAmount = null;

    [SerializeField] Image _colorBombBoosterImage = null;
    [SerializeField] TextMeshProUGUI _colorBombBoosterAmount = null;

    [SerializeField] List<Sprite> _boosterSprites = null;

    Inventory _inventory = null;

    public Action OnBombButtonSelected = delegate { };
    public Action OnColorBombButtonSelected = delegate { };

    public void Initialize(Inventory inventory)
    {
        _inventory = inventory;

        SetData();
    }

    void SetData()
    {
        _bombBoosterImage.sprite = _boosterSprites.Find(sprite => sprite.name == "Bomb");
        _bombBoosterAmount.text = _inventory.GetAmount("Bomb Booster").ToString();

        _colorBombBoosterImage.sprite = _boosterSprites.Find(sprite => sprite.name == "Color Bomb");
        _colorBombBoosterAmount.text = _inventory.GetAmount("Color Bomb Booster").ToString();
    }
}
