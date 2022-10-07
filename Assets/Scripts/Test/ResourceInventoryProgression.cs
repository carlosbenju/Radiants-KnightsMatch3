using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ResourceInventoryProgression
{
    public SaveData SaveData;

    [NonSerialized]
    public ResourceInventoryConfig Config;
    [NonSerialized]
    bool _isDirty = false;

    public List<InGameResource> Resources = new List<InGameResource>();

    GameProgressionService _gameProgression;

    public ResourceInventoryProgression(ResourceInventoryConfig config)
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
        Config = config;
    }

    public void AddResource(string resourceId, int amount)
    {
        InGameResource resource = Resources.Find(r => r.Id == resourceId);
        if (resource == null)
        {
            resource = new InGameResource { Id = resourceId, Amount = 0 };
            Resources.Add(resource);
        }

        resource.Amount = resource.Amount + amount;
        _isDirty = true;
    }

    public void RemoveResource(string resourceId, int amount)
    {
        InGameResource resource = Resources.Find(resource => resource.Id == resourceId);
        if (resource == null)
        {
            resource = new InGameResource { Id = resourceId, Amount = 0 };
            Resources.Add(resource);
        }

        resource.Amount = Mathf.Max(0, resource.Amount - amount);
        _isDirty = true;
    }

    public int GetResourceAmount(string resourceId)
    {
        return Resources.Find(resource => resource.Id == resourceId)?.Amount ?? 0;
    }

    public void Save()
    {
        var dir = Application.persistentDataPath + "/Saves/";

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        File.WriteAllText(dir + "inventory.sav", JsonUtility.ToJson(this, true));
        GUIUtility.systemCopyBuffer = dir;
    }

    public void Load()
    {
        Resources = _gameProgression.Data.ResourcesInventory;
        _gameProgression.Save();
    }
}
