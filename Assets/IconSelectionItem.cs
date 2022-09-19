using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconSelectionItem : MonoBehaviour
{
    [SerializeField] Image _iconImage;
    List<Sprite> _icons;
    Action<Image> _onIconSelected;

    public void Initialize(string image, List<Sprite> icons, Action<Image> onIconSelected)
    {
        _onIconSelected = onIconSelected;
        _icons = icons;
        _iconImage.sprite = _icons.Find(sprite => sprite.name == image);
    }

    public void OnClick()
    {
        _onIconSelected?.Invoke(_iconImage);
    }
}
