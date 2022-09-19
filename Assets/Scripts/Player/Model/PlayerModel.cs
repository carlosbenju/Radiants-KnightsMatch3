using Game.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    public PlayerData playerData;

    GameConfigService _gameConfigService;

    public void Initialize()
    {
        Load();
    }

    public void CreateNewUser(string name)
    {
        _gameConfigService = ServiceLocator.GetService<GameConfigService>();

        Inventory inventory = new Inventory();
        List<InventoryItem> InitialInventory = new List<InventoryItem>();
        InitialInventory.Add(new InventoryItem { Type = "Gold", Amount =  _gameConfigService.InitialGold });
        InitialInventory.Add(new InventoryItem { Type = "Diamond", Amount = _gameConfigService.InitialDiamonds });
        InitialInventory.Add(new InventoryItem { Type = "Bomb Booster", Amount = _gameConfigService.InitialBombBoosters });
        InitialInventory.Add(new InventoryItem { Type = "Color Bomb Booster", Amount = _gameConfigService.InitialColorBombBoosters });
        InitialInventory.Add(new InventoryItem { Type = _gameConfigService.InitialProfileImage, Amount = 1 });

        inventory.CreateInventory(InitialInventory);

        playerData = new PlayerData { ProfileImage = _gameConfigService.InitialProfileImage, Name = name, CurrentLevel = 1, Inventory = inventory, CurrentHeroId = _gameConfigService.InitialHeroId };
    }

    void Load() => playerData = SaveGameManager.CurrentSaveData.PlayerData;

    public void Save() => SaveGameManager.CurrentSaveData.PlayerData = playerData;
}
