using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveDataModel
{
    public string Name;
    public string ProfileImage;
    public int CurrentLevel;
    public List<InGameResource> ResourcesInventory;
    public List<IconCollectible> IconInventory;
    public List<BoosterModel> BoostersInventory;
    public int CurrentHeroId;
    public List<LevelModel> CompletedLevels;
}
