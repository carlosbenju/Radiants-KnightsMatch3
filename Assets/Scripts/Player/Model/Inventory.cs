using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    [SerializeField] List<InventoryItem> Items = new List<InventoryItem>();

    public event Action<string> OnResourceModified = resource => { };

    public void CreateInventory(List<InventoryItem> items)
    {
        Items = items;
    }

    public void Add(InventoryItem item)
    {
        foreach (InventoryItem resourceItem in Items)
        {
            if (resourceItem.Type == item.Type)
            {
                resourceItem.Amount += item.Amount;
                OnResourceModified(item.Type);
                return;
            }
        }

        Items.Add(new InventoryItem { Type = item.Type, Amount = item.Amount });
    }

    public void Remove(InventoryItem item)
    {
        foreach (InventoryItem resourceItem in Items)
        {
            if (resourceItem.Type == item.Type)
            {
                resourceItem.Amount = Mathf.Max(0, resourceItem.Amount - item.Amount);
                OnResourceModified(resourceItem.Type);
                return;
            }
        }
    }

    public int GetAmount(string resourceType)
    {
        foreach (InventoryItem resourceItem in Items)
        {
            if (resourceItem.Type == resourceType)
            {
                return resourceItem.Amount;
            }
        }

        return 0;
    }

    public List<InventoryItem> GetInventoryItems()
    {
        return Items;
    }

    public void Load() => Items = SaveGameManager.CurrentSaveData.PlayerData.Inventory.GetInventoryItems();

    public void Save()
    {
        foreach (InventoryItem item in Items)
        {
            Debug.Log(item.Type + ": " + item.Amount);
        }
        SaveGameManager.CurrentSaveData.PlayerData.Inventory = this;
    } 
}
