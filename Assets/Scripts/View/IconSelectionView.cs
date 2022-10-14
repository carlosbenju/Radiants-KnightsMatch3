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

    Action<string> _onImageSelected = delegate (string imageName) { };

    AsyncOperationHandle _currentIconSelectionHandle;

    GameProgressionTestService _progressionService;
    IconCollectibleProgression _iconProgression;

    public void Initialize(Action<string> onImageIconSelected, AsyncOperationHandle handle)
    {
        _progressionService = ServiceLocator.GetService<GameProgressionTestService>();
        _iconProgression = _progressionService.IconProgression;

        _onImageSelected = onImageIconSelected;
        _currentIconSelectionHandle = handle;

        foreach (IconCollectibleConfig iconConfig in _iconProgression.Config.Icons)
        {
            Addressables.LoadAssetAsync<Sprite>(iconConfig.AssetName).Completed += handle =>
            {
                Instantiate(_iconItemPrefab, _parent).Initialize(_iconProgression, handle.Result, SelectImage);
            };
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
