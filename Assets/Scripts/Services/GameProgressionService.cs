using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Services;

public class GameProgressionService : IService
{
    IGameProgressionProvider _progressionProvider;

    public SaveData Data;

    public void Initialize(GameConfigService gameConfig, IGameProgressionProvider provider)
    {
        _progressionProvider = provider;
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
