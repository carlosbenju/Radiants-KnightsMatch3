using Game.Services;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopConfig
{
    public List<ShopItemModel> RegularShopItems = new List<ShopItemModel>();
    public List<ShopItemModel> IAPShopItems = new List<ShopItemModel>();

    public void Initialize(RemoteConfigGameService remoteConfig)
    {
        ShopConfig regularShopData = JsonUtility.FromJson<ShopConfig>(remoteConfig.GetJson("RegularShop_Config"));
        ShopConfig iapShopData = JsonUtility.FromJson<ShopConfig>(remoteConfig.GetJson("IAPProducts_Config"));
        RegularShopItems = regularShopData.RegularShopItems;
        IAPShopItems = iapShopData.IAPShopItems;
    }
}
