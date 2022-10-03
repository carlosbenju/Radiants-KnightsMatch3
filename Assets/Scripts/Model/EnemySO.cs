using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy")]
public class EnemySO : ScriptableObject
{
    public int Id;
    public TileType Type;
    public int Strength;
    public int Health;
    public int AttackInterval;
    public string Image;
}
