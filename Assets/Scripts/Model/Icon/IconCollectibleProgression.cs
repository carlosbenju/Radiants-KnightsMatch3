using Game.Services;
using System.Collections.Generic;
using UnityEngine;

public class IconCollectibleProgression
{
    public IconCollectiblesInventoryConfig Config;

    public List<IconCollectible> Icons = new List<IconCollectible>();

    public string DefaultIconId = "galaxy-icon";

    IGameProgressionProvider _progressionProvider;

    public IconCollectibleProgression(IconCollectiblesInventoryConfig config, IGameProgressionProvider provider)
    {
        _progressionProvider = provider;
        Config = config;
    }

    public void AddIcon(string iconId)
    {
        IconCollectible icon = Icons.Find(i => i.Id == iconId);
        if (icon == null)
        {
            icon = new IconCollectible { Id = iconId };
            Icons.Add(icon);
            return;
        }

        Debug.Log("The user already owns this icon");
    }

    public IconCollectible GetIcon(string iconId)
    {
        return Icons.Find(i => i.Id == iconId);
    }

    public void SetInitialValues()
    {
        IconCollectibleConfig defaultIconConfig = Config.Icons.Find(icon => icon.Id == DefaultIconId);
        AddIcon(defaultIconConfig.Id);
    }

    public void Load()
    {
        SaveDataModel savedData = JsonUtility.FromJson<SaveDataModel>(_progressionProvider.Load());
        Icons = savedData.IconInventory;
    }
}
