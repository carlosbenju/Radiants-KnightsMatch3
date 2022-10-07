using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Services;

[Serializable]
public class ResourceInventoryConfig
{
    public List<InGameResourceConfig> Resources;

    public void Load(RemoteConfigGameService remoteConfig)
    {
        JsonUtility.FromJsonOverwrite(remoteConfig.GetJson("ResourceInventory_Config"), this);
    }
}
