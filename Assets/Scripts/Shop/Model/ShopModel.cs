using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopModel
{
    public List<ShopItemModel> Items = new List<ShopItemModel>();
    public List<ShopItemModel> PremiumItems = new List<ShopItemModel>();
}
