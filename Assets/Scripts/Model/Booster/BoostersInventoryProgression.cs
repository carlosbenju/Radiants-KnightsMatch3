using Game.Services;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BoostersInventoryProgression
{
    public BoostersInventoryConfig Config;

    public List<BoosterModel> Boosters = new List<BoosterModel>();

    IGameProgressionProvider _progressionProvider;

    public Action<string> OnBoosterModified = resource => { };

    public BoostersInventoryProgression(BoostersInventoryConfig config, IGameProgressionProvider provider)
    {
        _progressionProvider = provider;
        Config = config;
    }

    public void AddBooster(string boosterId, int amount)
    {
        BoosterModel booster = Boosters.Find(b => b.Id == boosterId);
        if (booster == null)
        {
            booster = new BoosterModel { Id = boosterId, Amount = 0 };
            Boosters.Add(booster);
        }

        booster.Amount = booster.Amount + amount;
        OnBoosterModified(boosterId);
    }

    public void RemoveBooster(string boosterId, int amount)
    {
        BoosterModel booster = Boosters.Find(b => b.Id == boosterId);
        if (booster == null)
        {
            booster = new BoosterModel { Id = boosterId, Amount = 0 };
            Boosters.Add(booster);
        }

        booster.Amount = Mathf.Max(0, booster.Amount - amount);
        OnBoosterModified(boosterId);
    }

    void DeleteBoosterType(string boosterId)
    {
        Boosters.Remove(Boosters.Find(booster => booster.Id == boosterId));
    }

    public int GetBoosterAmount(string boosterId)
    {
        return Boosters.Find(b => b.Id == boosterId).Amount;
    }

    public void SetInitialValues()
    {
        foreach (BoosterConfig booster in Config.Boosters)
        {
            AddBooster(booster.Id, booster.StartAmount);
        }
    }

    public void Load()
    {
        SaveData savedData = JsonUtility.FromJson<SaveData>(_progressionProvider.Load());
        Boosters = savedData.BoostersInventory;
        AddNewBoostersFromConfig();
        RemoveUnusedBoosters();
    }

    public void AddNewBoostersFromConfig()
    {
        foreach (BoosterConfig resourceConfig in Config.Boosters)
        {
            BoosterModel booster = Boosters.Find(resource => resource.Id == resourceConfig.Id);
            if (booster == null)
            {
                booster = new BoosterModel { Id = resourceConfig.Id, Amount = 0 };
                booster.Amount += resourceConfig.StartAmount;
                Boosters.Add(booster);
            }
        }
    }

    public void RemoveUnusedBoosters()
    {
        List<BoosterModel> BoostersToDelete = new List<BoosterModel>();
        foreach (BoosterModel booster in Boosters)
        {
            BoosterConfig boosterConfig = Config.Boosters.Find(b => b.Id == booster.Id);
            if (boosterConfig == null)
            {
                BoostersToDelete.Add(booster);
            }
        }

        foreach (BoosterModel booster in BoostersToDelete)
        {
            DeleteBoosterType(booster.Id);
        }
    }
}
