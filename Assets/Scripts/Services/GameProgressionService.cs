using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Services;

public class GameProgressionService : IService
{
    IGameProgressionProvider _progressionProvider;

    public SaveData Data;

    GameConfigService _gameConfigService;

    public void Initialize(GameConfigService gameConfig, IGameProgressionProvider provider)
    {
        _progressionProvider = provider;
        _gameConfigService = gameConfig;
    }

    public void CreateNewUser(string name)
    {
        Inventory inventory = new Inventory();
        List<InventoryItem> InitialInventory = new List<InventoryItem>();
        InitialInventory.Add(new InventoryItem { Type = "Gold", Amount = _gameConfigService.InitialGold });
        InitialInventory.Add(new InventoryItem { Type = "Diamond", Amount = _gameConfigService.InitialDiamonds });
        InitialInventory.Add(new InventoryItem { Type = "Bomb Booster", Amount = _gameConfigService.InitialBombBoosters });
        InitialInventory.Add(new InventoryItem { Type = "Color Bomb Booster", Amount = _gameConfigService.InitialColorBombBoosters });
        InitialInventory.Add(new InventoryItem { Type = _gameConfigService.InitialProfileImage, Amount = 1 });

        inventory.CreateInventory(InitialInventory);

        Data = new SaveData { ProfileImage = _gameConfigService.InitialProfileImage, Name = name, CurrentLevel = 1, Inventory = inventory, CurrentHeroId = _gameConfigService.InitialHeroId };
    }

    public SaveData Load()
    {
        string savedData = _progressionProvider.Load();
        if (string.IsNullOrEmpty(savedData))
        {
            return null;
        }

        Data = JsonUtility.FromJson<SaveData>(savedData);
        return Data;
    }

    public void Save()
    {
        string savedData = JsonUtility.ToJson(Data, true);
        _progressionProvider.Save(savedData);
    }

    public void Clear()
    {
    }
}
