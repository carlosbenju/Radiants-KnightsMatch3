using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class PlayerProfileView : MonoBehaviour
{
    [SerializeField] IconSelectionView _iconSelectionPrefab;
    [SerializeField] Transform _parent;
    [SerializeField] Transform _iconSelectionPopupPosition;

    [SerializeField] List<Sprite> _heroImages;

    [SerializeField] Image _playerImage;
    [SerializeField] TextMeshProUGUI _playerNameText;

    [SerializeField] TMP_Text _goldAmount;
    [SerializeField] TMP_Text _diamondAmount;
    [SerializeField] TMP_Text _bombsAmount;
    [SerializeField] TMP_Text _colorBombsAmount;

    [SerializeField] Image _currentHeroImage;
    [SerializeField] TextMeshProUGUI _currentHeroName;
    [SerializeField] TextMeshProUGUI _currentHeroType;
    [SerializeField] TextMeshProUGUI _currentHeroHealth;
    [SerializeField] TextMeshProUGUI _currentHeroStrength;

    Action _onPopupClosed;

    AsyncOperationHandle _currentProfileViewHandle;

    GameProgressionService _progressionService;
    ResourceInventoryProgression _inventoryProgression;
    BoostersInventoryProgression _boosterProgression;

    Sprite _playerIcon;

    public void Initialize(Sprite playerIcon, Action onPopupClosed, AsyncOperationHandle handler)
    {
        _progressionService = ServiceLocator.GetService<GameProgressionService>();
        _inventoryProgression = _progressionService.ResourceProgression;
        _boosterProgression = _progressionService.BoostersProgression;

        _onPopupClosed = onPopupClosed;
        _currentProfileViewHandle = handler;
        _playerIcon = playerIcon;

        HeroModel currentHero = new HeroModel();
        currentHero.InitializeById(_progressionService.Data.CurrentHeroId);

        SetPlayerData();
        SetHeroData(currentHero);
    }

    private void SetPlayerData()
    {
        _playerImage.sprite = _playerIcon;
        _playerNameText.text = _progressionService.Data.Name;
        _goldAmount.text = _inventoryProgression.GetResourceAmount("Gold").ToString();
        _diamondAmount.text = _inventoryProgression.GetResourceAmount("Diamond").ToString();
        _bombsAmount.text = _boosterProgression.GetBoosterAmount("BombBooster").ToString();
        _colorBombsAmount.text = _boosterProgression.GetBoosterAmount("ColorBombBooster").ToString();
    }

    void SetHeroData(HeroModel hero)
    {
        _currentHeroName.text = hero.Name;
        switch (hero.Type)
        {
            case TileType.Blue:
                _currentHeroType.color = new Color32(69, 163, 220, 255);
                break;
            case TileType.Green:
                _currentHeroType.color = new Color32(57, 125, 71, 255);
                break;
            case TileType.Red:
                _currentHeroType.color = new Color32(196, 54, 65, 255);
                break;
        }

        _currentHeroType.text = hero.Type.ToString();
        _currentHeroHealth.text = hero.Health.ToString();
        _currentHeroStrength.text = hero.Strength.ToString();
        _currentHeroImage.sprite = _heroImages.Find(sprite => sprite.name == hero.HeroSprite);
    }

    public void OpenIconSelectionPopup()
    {
        Addressables.LoadAssetAsync<GameObject>("icon_selection_popup").Completed += handle =>
        {
            if (handle.Result != null)
            {
                IconSelectionView popup = handle.Result.GetComponent<IconSelectionView>();
                Instantiate(popup, _iconSelectionPopupPosition.position, Quaternion.identity, _parent).Initialize(ChangePlayerIcon, handle);
            }
        };
    }

    void ChangePlayerIcon(string newIconName)
    {
        _progressionService.Data.ProfileImage = newIconName;
        Addressables.LoadAssetAsync<Sprite>(newIconName).Completed += handle =>
        {
            if (handle.Result != null)
            {
                _playerImage.sprite = handle.Result;
            }
        };
    }

    public void Close()
    {
        if (_currentProfileViewHandle.IsValid())
        {
            Addressables.Release(_currentProfileViewHandle);
        }

        _onPopupClosed?.Invoke();
        Destroy(gameObject);
    }
}
