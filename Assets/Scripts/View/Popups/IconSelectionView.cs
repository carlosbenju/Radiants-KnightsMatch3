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

    Action _onImageSelected = delegate { };

    GameProgressionService _progressionService;
    IconCollectibleProgression _iconProgression;

    public void Initialize(Action onImageIconSelected)
    {
        _progressionService = ServiceLocator.GetService<GameProgressionService>();
        _iconProgression = _progressionService.IconProgression;

        _onImageSelected = onImageIconSelected;

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
        _progressionService.Data.ProfileImage = iconName;
        _onImageSelected?.Invoke();
        Destroy(gameObject);
    }
}
