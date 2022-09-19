using Game.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController
{
    public ShopModel Model { get; private set; }
    public Inventory Inventory { get; private set; }

    public ShopController(Inventory inventory)
    {
        Inventory = inventory;
    }

    public void PurchaseItem(ShopItemModel model)
    {
        if (model.IsObtainedWithAd)
        {
            Inventory.Add(model.Reward);
            Inventory.Save();
            return;
        }

        if (Inventory.GetAmount(model.Cost.Type) < model.Cost.Amount)
        {
            Debug.Log("The user does not have enough to pay");
            return;
        }

        Inventory.Remove(model.Cost);
        Inventory.Add(model.Reward);
        Inventory.Save();
    }


    public void Load()
    {
        Model = JsonUtility.FromJson<ShopModel>(Resources.Load<TextAsset>("ShopModel").text);
    }
}
