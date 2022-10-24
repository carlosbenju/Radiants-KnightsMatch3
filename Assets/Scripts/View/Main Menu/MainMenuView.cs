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
    PlayerProfileView _playerProfilePopupPrefab;

    [SerializeField]
    LevelSelectionView _levelSelectionPopupPrefab;

    [SerializeField]
    ResourceView _resourcePrefab;


    List<ResourceView> _resourceViews = new List<ResourceView>();

    [SerializeField] Image _playerImage = null;

    GameProgressionService _progressionService;
    ResourceInventoryProgression _inventoryProgression;

    public void Initialize()
    {
        _progressionService = ServiceLocator.GetService<GameProgressionService>();
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
            ResourceView view = Instantiate(_resourcePrefab, _resourcesParent);
            view.gameObject.SetActive(false);
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

            view.gameObject.SetActive(true);
        }
    }

    public void OpenPlayerProfilePopup()
    {
        Instantiate(_playerProfilePopupPrefab, _parent).Initialize(_playerImage.sprite, UpdatePlayerData);
    }

    public void OpenLevelSelectionPopup()
    {
        Instantiate(_levelSelectionPopupPrefab, _parent).Initialize(SelectLevel);
    }

    void SelectLevel(int selectedLevel)
    {
        _currentLevelText.text = selectedLevel.ToString();
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
