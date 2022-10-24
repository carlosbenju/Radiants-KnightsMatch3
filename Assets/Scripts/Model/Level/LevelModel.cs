using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelModel
{
    [SerializeField]
    public int LevelNumber;

    [System.NonSerialized]
    public EnemySO[] Waves;

    [System.NonSerialized]
    public List<Reward> Rewards;

    [SerializeField]
    public bool IsCompleted = false;

    public LevelModel(LevelSO level)
    {
        LevelNumber = level.Level;
        Waves = level.Waves;
        Rewards = level.Rewards;
    }
}
