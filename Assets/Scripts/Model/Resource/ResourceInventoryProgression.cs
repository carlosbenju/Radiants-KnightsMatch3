using System;
using Game.Services;
using System.Collections.Generic;
using UnityEngine;

public class ResourceInventoryProgression
{
    public ResourceInventoryConfig Config;

    public List<InGameResource> Resources = new List<InGameResource>();

    public Action<string> OnResourceModified = resource => { };

    IGameProgressionProvider _progressionProvider;

    public ResourceInventoryProgression(ResourceInventoryConfig config, IGameProgressionProvider provider)
    {
        _progressionProvider = provider;
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
        OnResourceModified(resourceId);
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
        OnResourceModified(resourceId);
    }

    void DeleteResource(string resourceId)
    {
        Resources.Remove(Resources.Find(r => r.Id == resourceId));
    }

    public int GetResourceAmount(string resourceId)
    {
        return Resources.Find(resource => resource.Id == resourceId)?.Amount ?? 0;
    }

    public void SetInitialValues()
    {
        foreach (InGameResourceConfig resource in Config.Resources)
        {
            AddResource(resource.Id, resource.StartAmount);
        }
    }

    public void Load()
    {
        SaveDataModel savedData = JsonUtility.FromJson<SaveDataModel>(_progressionProvider.Load());
        Resources = savedData.ResourcesInventory;
        AddNewResourcesFromConfig();
        RemoveUnusedResources();
    }

    public void AddNewResourcesFromConfig()
    {
        foreach (InGameResourceConfig resourceConfig in Config.Resources)
        {
            InGameResource resource = Resources.Find(resource => resource.Id == resourceConfig.Id);
            if (resource == null)
            {
                resource = new InGameResource { Id = resourceConfig.Id, Amount = 0 };
                resource.Amount += resourceConfig.StartAmount;
                Resources.Add(resource);
            }
        }
    }

    public void RemoveUnusedResources()
    {
        List<InGameResource> ResourcesToDelete = new List<InGameResource>();
        foreach (InGameResource resource in Resources)
        {
            InGameResourceConfig resourceConfig = Config.Resources.Find(r => r.Id == resource.Id);
            if (resourceConfig == null)
            {
                ResourcesToDelete.Add(resource);
            }
        }

        foreach (InGameResource resource in ResourcesToDelete)
        {
            DeleteResource(resource.Id);
        }
    }
}
