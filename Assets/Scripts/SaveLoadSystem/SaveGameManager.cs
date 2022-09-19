using UnityEngine;
using System.IO;

public static class SaveGameManager
{
    public static SaveData CurrentSaveData = new SaveData();

    public const string SaveDirectory = "/Saves/";
    public const string FileName = "SaveGame.sav";

    public static bool SaveGame(string fileName = FileName)
    {
        var dir = Application.persistentDataPath + SaveDirectory;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string json = JsonUtility.ToJson(CurrentSaveData, true);
        File.WriteAllText(dir + FileName, json);

        GUIUtility.systemCopyBuffer = dir;

        return true;
    }

    public static bool LoadGame()
    {
        string fullPath = Application.persistentDataPath + SaveDirectory + FileName;

        SaveData tempData = new SaveData();

        if (File.Exists(fullPath))
        {
            string json =  File.ReadAllText(fullPath);
            tempData = JsonUtility.FromJson<SaveData>(json);
        } else
        {
            return false;
        }

        CurrentSaveData = tempData;
        return true;
    }
}
