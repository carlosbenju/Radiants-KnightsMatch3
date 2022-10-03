using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Hero")]
[System.Serializable]
public class HeroSO : ScriptableObject
{
    public int Id;
    public string Name;
    public TileType Type;
    public int Strength;
    public int Health;
    public string Image;
}
