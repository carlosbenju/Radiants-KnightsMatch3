using System;
using System.Collections.Generic;
using Game.Services;
using UnityEngine;

[Serializable]
public class IconCollectiblesInventoryConfig
{
    public List<IconCollectibleConfig> Icons;

    public void Initialize(RemoteConfigGameService remoteConfig)
    {
        JsonUtility.FromJsonOverwrite(remoteConfig.GetJson("Icons_Config"), this);
    }
}
