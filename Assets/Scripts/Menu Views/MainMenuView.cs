using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] Transform _parent;

    [SerializeField] TextMeshProUGUI _goldText;
    [SerializeField] TextMeshProUGUI _diamondText;
    [SerializeField] TextMeshProUGUI _bombBoosterText;
    [SerializeField] TextMeshProUGUI _colorBombBoosterText;


    [SerializeField] TextMeshProUGUI _currentLevelText;

    [SerializeField] Image _playerImage = null;
    [SerializeField] TextMeshProUGUI _playerName = null;

    [SerializeField] List<Sprite> _imageSprites = null;

    Inventory _inventory;
    PlayerModel _player;

    public void Initialize(Inventory inventory, PlayerModel player)
    {
        _inventory = inventory;
        _player = player;

        _inventory.OnResourceModified += UpdateResource;

        UpdateMenuData();
    }

    void OnDestroy()
    {
        if (_inventory != null)
            _inventory.OnResourceModified -= UpdateResource;
    }

    public void OpenPlayerProfilePopup()
    {
        Addressables.LoadAssetAsync<GameObject>("player_info_popup_v2").Completed += handle =>
        {
            if (handle.Result != null)
            {
                PlayerProfileView popup = handle.Result.GetComponent<PlayerProfileView>();
                Instantiate(popup, _parent).Initialize(_player, _inventory, _imageSprites, UpdatePlayerData, handle);
            }
        };
    }

    void UpdateMenuData()
    {
        UpdateResource("Gold");
        UpdateResource("Diamond");
        UpdateResource("Bomb Booster");
        UpdateResource("Color Bomb Booster");

        UpdatePlayerData();
    }

    void UpdatePlayerData()
    {
        _playerName.text = _player.playerData.Name;
        _playerImage.sprite = _imageSprites.Find(sprite => sprite.name == _player.playerData.ProfileImage);
        _currentLevelText.text = _player.playerData.CurrentLevel.ToString();
    }

    void UpdateResource(string resource)
    {
        switch (resource)
        {
            case "Gold":
                _goldText.text = _inventory.GetAmount("Gold").ToString();
                break;
            case "Diamond":
                _diamondText.text = _inventory.GetAmount("Diamond").ToString();
                break;
            case "Bomb Booster":
                _bombBoosterText.text = _inventory.GetAmount("Bomb Booster").ToString();
                break;
            case "Color Bomb Booster":
                _colorBombBoosterText.text = _inventory.GetAmount("Color Bomb Booster").ToString();
                break;
        }
    }
}
