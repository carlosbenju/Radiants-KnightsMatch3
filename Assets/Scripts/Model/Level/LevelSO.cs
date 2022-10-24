using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level")]
[System.Serializable]
public class LevelSO : ScriptableObject
{
    public int Level;
    public EnemySO[] Waves;
    public List<Reward> Rewards;
}
