using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Services;
using System.Threading.Tasks;
using System.IO;

public class FileGameProgressionProvider : IGameProgressionProvider
{
    public const string SaveDirectory = "/Saves/";
    public const string FileName = "SaveGame.sav";

    public async Task<bool> Initialize()
    {
        await Task.Yield();
        return true;
    }

    public string Load()
    {
        string fullPath = Application.persistentDataPath + SaveDirectory + FileName;

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            return JsonUtility.FromJson<string>(json);
        }

        return string.Empty;
    }

    public void Save(string data)
    {
        var dir = Application.persistentDataPath + SaveDirectory;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        File.WriteAllText(dir + FileName, data);

        GUIUtility.systemCopyBuffer = dir;
    }
}
