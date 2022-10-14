using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Services;

[Serializable]
public class BoostersInventoryConfig
{
    public List<BoosterConfig> Boosters;

    public void Initialize(RemoteConfigGameService remoteConfig)
    {
        JsonUtility.FromJsonOverwrite(remoteConfig.GetJson("Boosters_Config"), this);
    }
}
