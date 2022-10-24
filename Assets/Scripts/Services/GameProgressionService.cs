using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Services;
using System.IO;

public class GameProgressionService : IService
{
    IGameProgressionProvider _progressionProvider;
    RemoteConfigGameService _remoteConfig;
    ResourceInventoryConfig _resourceConfig = new ResourceInventoryConfig();
    IconCollectiblesInventoryConfig _iconCollectibleConfig = new IconCollectiblesInventoryConfig();
    BoostersInventoryConfig _boostersInventoryConfig = new BoostersInventoryConfig();

    public ShopConfig ShopConfig = new ShopConfig();
    public ResourceInventoryProgression ResourceProgression;
    public IconCollectibleProgression IconProgression;
    public BoostersInventoryProgression BoostersProgression;
    public SaveDataModel Data;

    public void Initialize(RemoteConfigGameService remoteConfig, IGameProgressionProvider provider)
    {
        _remoteConfig = remoteConfig;
        _progressionProvider = provider;

        _resourceConfig.Initialize(remoteConfig);
        _iconCollectibleConfig.Initialize(remoteConfig);
        _boostersInventoryConfig.Initialize(remoteConfig);
        ShopConfig.Initialize(remoteConfig);

        ResourceProgression = new ResourceInventoryProgression(_resourceConfig, provider);
        IconProgression = new IconCollectibleProgression(_iconCollectibleConfig, provider);
        BoostersProgression = new BoostersInventoryProgression(_boostersInventoryConfig, provider);
    }

    public void CreateUser(string name)
    {
        Data = new SaveDataModel();
        Data.ResourcesInventory = new List<InGameResource>();
        Data.IconInventory = new List<IconCollectible>();
        Data.BoostersInventory = new List<BoosterModel>();

        SetInitialUserValues(name);
        ResourceProgression.SetInitialValues();
        IconProgression.SetInitialValues();
        BoostersProgression.SetInitialValues();
    }

    public void SetInitialUserValues(string name)
    {
        Data.Name = name;
        Data.CurrentLevel = 1;
        Data.CurrentHeroId = _remoteConfig.GetInt("InitialHeroId", 1);
        Data.ProfileImage = IconProgression.DefaultIconId;
    }

    public void Save()
    {
        SaveProgressions();
        string savedData = JsonUtility.ToJson(Data, true);
        _progressionProvider.Save(savedData);
    }

    public void SaveProgressions()
    {
        Data.ResourcesInventory = ResourceProgression.Resources;
        Data.IconInventory = IconProgression.Icons;
        Data.BoostersInventory = BoostersProgression.Boosters;
    }


    public void SaveToCloud()
    {
        _progressionProvider.SaveToCloud();
    }

    public SaveDataModel Load()
    {
        string savedData = _progressionProvider.Load();
        if (string.IsNullOrEmpty(savedData))
        {
            return null;
        }

        Data = JsonUtility.FromJson<SaveDataModel>(savedData);
        LoadProgressions();
        return Data;
    }

    public void LoadProgressions()
    {
        ResourceProgression.Load();
        IconProgression.Load();
        BoostersProgression.Load();
    }

    public void Clear()
    {

    }
}
