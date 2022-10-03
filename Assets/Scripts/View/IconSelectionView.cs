using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class IconSelectionView : MonoBehaviour
{
    [SerializeField] IconSelectionItem _iconItemPrefab;
    [SerializeField] Transform _parent;

    List<Sprite> _possibleIcons;

    Action<string> _onImageSelected = delegate (string imageName) { };

    AsyncOperationHandle _currentIconSelectionHandle;

    GameProgressionService _gameProgressionService;

    public void Initialize(List<Sprite> possibleIcons, Action<string> onImageIconSelected, AsyncOperationHandle handle)
    {
        _gameProgressionService = ServiceLocator.GetService<GameProgressionService>();
        _possibleIcons = possibleIcons;
        _onImageSelected = onImageIconSelected;
        _currentIconSelectionHandle = handle;
        List<string> userIcons = new List<string>();

        List<InventoryItem> inventoryItems = _gameProgressionService.Data.Inventory.GetInventoryItems();
        foreach (InventoryItem i in inventoryItems)
        {
            if (i.Type.Contains("icon"))
            {
                userIcons.Add(i.Type);
            }
        }

        foreach (string i in userIcons)
        {
            Instantiate(_iconItemPrefab, _parent).Initialize(i, possibleIcons, SelectImage);
        }
    }

    public void SelectImage(Image image)
    {
        string iconName = image.sprite.name;

        if (_currentIconSelectionHandle.IsValid())
        {
            Addressables.Release(_currentIconSelectionHandle);
        }

        _onImageSelected?.Invoke(iconName);
        Destroy(gameObject);
    }
}
