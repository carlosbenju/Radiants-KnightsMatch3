using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Services;

[Serializable]
public class InventoryConfig
{
    public List<InGameResourceConfig> Resources;

    public InGameResourceConfig GetResourceConfig(string resourceId)
    {
        return Resources.Find(resource => resource.Id == resourceId);
    }

    public void Load(RemoteConfigGameService remoteConfig)
    {
        JsonUtility.FromJsonOverwrite(remoteConfig.GetString("Inventory_Config"), this);
        foreach (InGameResourceConfig i in Resources)
        {
            Debug.Log(i.Id);
        }
    }
}
