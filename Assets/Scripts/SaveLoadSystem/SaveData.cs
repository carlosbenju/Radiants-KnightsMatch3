using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string ProfileImage;
    public string Name;
    public int CurrentLevel;
    public Inventory Inventory;
    public List<InGameResource> ResourcesInventory;
    public int CurrentHeroId;
    public List<LevelModel> CompletedLevels;
}
