using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItemModel
{
    public int Id;
    public string Name;
    public string Image;
    public InventoryItem Cost;
    public InventoryItem Reward;
    public bool IsObtainedWithAd;
}
