using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public PlayerData PlayerData;
}

[System.Serializable]
public struct PlayerData
{
    public string ProfileImage;
    public string Name;
    public int CurrentLevel;
    public Inventory Inventory;
    public int CurrentHeroId;
    public List<LevelModel> CompletedLevels;
}
