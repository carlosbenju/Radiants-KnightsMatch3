using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item")]
public class ItemSO : ScriptableObject
{
    public TileType type;
    public BoosterType boosterType;

    public string itemName;
    public Sprite sprite;
}
