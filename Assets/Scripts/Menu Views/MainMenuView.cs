using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] Transform _parent;
    [SerializeField] Transform _resourcesParent;

    [SerializeField] TMP_Text _currentLevelText;
    [SerializeField] TMP_Text _playerName = null;

    [SerializeField]
    ResourceView _blueprint;

    List<ResourceView> _resourceViews = new List<ResourceView>();

    [SerializeField] Image _playerImage = null;

    GameProgressionTestService _progressionService;
    ResourceInventoryProgression _inventoryProgression;

    public void Initialize()
    {
        _progressionService = ServiceLocator.GetService<GameProgressionTestService>();
        _inventoryProgression = _progressionService.ResourceProgression;
        _inventoryProgression.OnResourceModified += UpdateResource;

        InstantiateResourceViews();

        UpdatePlayerData();
        UpdateResourcesViewData();
    }

    void OnDestroy()
    {
        _inventoryProgression.OnResourceModified -= UpdateResource;
    }

    private void InstantiateResourceViews()
    {
        foreach (InGameResourceConfig resource in _inventoryProgression.Config.Resources)
        {
            ResourceView view = Instantiate(_blueprint, _resourcesParent);
            view.ResourceType = resource.Id;
            view.Amount.text = _inventoryProgression.GetResourceAmount(resource.Id).ToString();
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

    public void OpenPlayerProfilePopup()
    {
        Addressables.LoadAssetAsync<GameObject>("player_info_popup_v2").Completed += handle =>
        {
            if (handle.Result != null)
            {
                PlayerProfileView popup = handle.Result.GetComponent<PlayerProfileView>();
                Instantiate(popup, _parent).Initialize(_playerImage.sprite, UpdatePlayerData, handle);
            }
        };
    }

    public void OpenLevelSelectionPopup()
    {
        Addressables.LoadAssetAsync<GameObject>("level_selection_popup").Completed += handle =>
        {
            if (handle.Result != null)
            {
                LevelSelectionView popup = handle.Result.GetComponent<LevelSelectionView>();
                Instantiate(popup, _parent).Initialize(UpdatePlayerData,handle);
            }
        };
    }

    void UpdateResourcesViewData()
    {
        _resourceViews.ForEach(r => r.Amount.text = _inventoryProgression.GetResourceAmount(r.ResourceType).ToString());
    }

    void UpdatePlayerData()
    {
        Addressables.LoadAssetAsync<Sprite>(_progressionService.Data.ProfileImage).Completed += handle =>
        {
            _playerImage.sprite = handle.Result;
        };

        _playerName.text = _progressionService.Data.Name;
        _currentLevelText.text = _progressionService.Data.CurrentLevel.ToString();
    }

    void UpdateResource(string resourceId)
    {
        ResourceView resourceView = _resourceViews.Find(r => r.ResourceType == resourceId);
        resourceView.Amount.text = _inventoryProgression.GetResourceAmount(resourceId).ToString();
    }
}
