using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item")]
public class ItemSO : ScriptableObject
{
    public TileType Type;
    public BoosterType BoosterType;

    public string ItemName;
    public Sprite Sprite;
}
