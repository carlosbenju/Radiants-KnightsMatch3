using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconSelectionItem : MonoBehaviour
{
    [SerializeField] Button _iconButton;
    [SerializeField] Image _iconImage;
    Action<Image> _onIconSelected;

    IconCollectibleProgression _iconProgression;

    void Awake()
    {
        _iconButton.interactable = false;
    }

    public void Initialize(IconCollectibleProgression iconProgression, Sprite icon, Action<Image> onIconSelected)
    {
        _iconProgression = iconProgression;
        _iconImage.sprite = icon;
        _onIconSelected = onIconSelected;

        IconCollectible i = _iconProgression.GetIcon(icon.name);
        if (i == null)
        {
            _iconButton.interactable = false;
        } else
        {
            _iconButton.interactable = true;
        }
    }

    public void OnClick()
    {
        _onIconSelected?.Invoke(_iconImage);
    }
}
