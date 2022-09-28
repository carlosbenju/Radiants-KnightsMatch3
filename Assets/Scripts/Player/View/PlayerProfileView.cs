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

    List<Sprite> _profileImages;
    [SerializeField] List<Sprite> _heroImages;

    [SerializeField] Image _playerProfileImage;
    [SerializeField] TextMeshProUGUI _playerNameText;

    [SerializeField] TextMeshProUGUI _goldAmount;
    [SerializeField] TextMeshProUGUI _diamondAmount;

    [SerializeField] Image _currentHeroImage;
    [SerializeField] TextMeshProUGUI _currentHeroName;
    [SerializeField] TextMeshProUGUI _currentHeroType;
    [SerializeField] TextMeshProUGUI _currentHeroHealth;
    [SerializeField] TextMeshProUGUI _currentHeroStrength;

    PlayerModel _playerModel;
    Inventory _inventory;

    Action _onPopupClosed;

    AsyncOperationHandle _currentProfileViewHandle;

    public void Initialize(PlayerModel playerModel, Inventory inventory, List<Sprite> profileImages, Action onPopupClosed, AsyncOperationHandle handler)
    {
        _playerModel = playerModel;
        _inventory = inventory;
        _profileImages = profileImages;
        _onPopupClosed = onPopupClosed;
        _currentProfileViewHandle = handler;

        HeroModel currentHero = new HeroModel();
        currentHero.InitializeById(_playerModel.Data.CurrentHeroId);

        SetPlayerData();
        SetHeroData(currentHero);
    }

    private void SetPlayerData()
    {
        _playerProfileImage.sprite = _profileImages.Find(sprite => sprite.name == _playerModel.Data.ProfileImage);
        _playerNameText.text = _playerModel.Data.Name;
        _goldAmount.text = _inventory.GetAmount("Gold").ToString();
        _diamondAmount.text = _inventory.GetAmount("Diamond").ToString();
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
                Instantiate(popup, _parent).Initialize(_playerModel, _profileImages, ChangePlayerIcon, handle);
            }
        };
    }

    void ChangePlayerIcon(string newIconName)
    {
        _playerModel.Data.ProfileImage = newIconName;
        _playerModel.Save();

        SetPlayerData();
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
