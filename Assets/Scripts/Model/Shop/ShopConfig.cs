using Game.Services;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopConfig
{
    public List<ShopItemModel> GoldShopItems = new List<ShopItemModel>();

    public void Initialize(RemoteConfigGameService remoteConfig)
    {
        ShopConfig data = JsonUtility.FromJson<ShopConfig>(remoteConfig.GetJson("GoldShop_Config"));
        GoldShopItems = data.GoldShopItems;
    }
}
