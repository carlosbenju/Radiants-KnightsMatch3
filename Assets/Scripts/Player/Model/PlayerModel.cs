using Game.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    public SaveData Data;

    GameConfigService _gameConfigService;
    GameProgressionService _gameProgressionService;

    public void Initialize()
    {
        _gameProgressionService = ServiceLocator.GetService<GameProgressionService>();
        Load();
    }

    public void CreateNewUser(string name)
    {
        _gameConfigService = ServiceLocator.GetService<GameConfigService>();
        _gameProgressionService = ServiceLocator.GetService<GameProgressionService>();

        Inventory inventory = new Inventory();
        List<InventoryItem> InitialInventory = new List<InventoryItem>();
        InitialInventory.Add(new InventoryItem { Type = "Gold", Amount =  _gameConfigService.InitialGold });
        InitialInventory.Add(new InventoryItem { Type = "Diamond", Amount = _gameConfigService.InitialDiamonds });
        InitialInventory.Add(new InventoryItem { Type = "Bomb Booster", Amount = _gameConfigService.InitialBombBoosters });
        InitialInventory.Add(new InventoryItem { Type = "Color Bomb Booster", Amount = _gameConfigService.InitialColorBombBoosters });
        InitialInventory.Add(new InventoryItem { Type = _gameConfigService.InitialProfileImage, Amount = 1 });

        inventory.CreateInventory(InitialInventory);

        Data = new SaveData { ProfileImage = _gameConfigService.InitialProfileImage, Name = name, CurrentLevel = 1, Inventory = inventory, CurrentHeroId = _gameConfigService.InitialHeroId };
    }

    public void Load()
    {
        Data = _gameProgressionService.Data;
    }

    public void Save()
    {
        _gameProgressionService.Data = Data;
    }
}
