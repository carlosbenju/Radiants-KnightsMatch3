using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public static class GameDataUpdater
{
    const string scriptUrl = "https://script.google.com/macros/s/AKfycbxOQCWSwVOilG1REVdiMd9uzv-BE9mVe1MqU2xjPqyWYDmKx8zI0veW1idDLlfDtQE/exec";

    [MenuItem("Game/Update Shop Data")]
    public static void UpdateShopModel()
    {
        string url = scriptUrl + "?action=shop"; 

        UnityWebRequest request = new UnityWebRequest(url, "GET", new DownloadHandlerBuffer(), null);
        request.SendWebRequest().completed += asyncOp =>
        {
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.Log(request.error);
                return;
            }

            System.IO.File.WriteAllText(Application.dataPath + "/Resources/ShopModel.json",
                request.downloadHandler.text);

            Debug.Log("Shop model updated with text: " + request.downloadHandler.text);
        };
    }

    [MenuItem("Game/Update Resources Data")]
    public static void UpdateGameData()
    {
        string url = scriptUrl + "?action=resources";

        UnityWebRequest request = new UnityWebRequest(url, "GET", new DownloadHandlerBuffer(), null);
        request.SendWebRequest().completed += asyncOp =>
        {
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.Log(request.error);
                return;
            }

            System.IO.File.WriteAllText(Application.dataPath + "/Resources/GameConfig.json",
                request.downloadHandler.text);

            Debug.Log("Shop model updated with text: " + request.downloadHandler.text);
        };
    }
}
