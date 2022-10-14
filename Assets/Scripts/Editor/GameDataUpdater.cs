using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public static class GameDataUpdater
{
    const string scriptUrl = "https://script.google.com/macros/s/AKfycbwQ2y__Mb_YAdEohr9qsenZneZmTHigx5TM9kuXAQ_hURRaHrRAO_bVjfXYnbvHO8TT/exec";

    [MenuItem("Game/Update Gold Shop Data")]
    public static void UpdateGoldShopData()
    {
        string url = scriptUrl + "?action=gold-shop";

        UnityWebRequest request = new UnityWebRequest(url, "GET", new DownloadHandlerBuffer(), null);
        request.SendWebRequest().completed += asyncOp =>
        {
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.Log(request.error);
                return;
            }

            System.IO.File.WriteAllText(Application.dataPath + "/Resources/GoldShopConfig.json",
                request.downloadHandler.text);

            Debug.Log("Gold shop config updated with text: " + request.downloadHandler.text);
        };
    }

    [MenuItem("Game/Update Resources Data")]
    public static void UpdateResourcesData()
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

            System.IO.File.WriteAllText(Application.dataPath + "/Resources/ResourcesConfig.json",
                request.downloadHandler.text);

            Debug.Log("Resources config updated with text: " + request.downloadHandler.text);
        };
    }

    [MenuItem("Game/Update Icons Data")]
    public static void UpdateIconsData()
    {
        string url = scriptUrl + "?action=icons";

        UnityWebRequest request = new UnityWebRequest(url, "GET", new DownloadHandlerBuffer(), null);
        request.SendWebRequest().completed += asyncOp =>
        {
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.Log(request.error);
                return;
            }

            System.IO.File.WriteAllText(Application.dataPath + "/Resources/IconsConfig.json",
                request.downloadHandler.text);

            Debug.Log("Icons config updated with text: " + request.downloadHandler.text);
        };
    }

    [MenuItem("Game/Update Boosters Data")]
    public static void UpdateBoostersData()
    {
        string url = scriptUrl + "?action=boosters";

        UnityWebRequest request = new UnityWebRequest(url, "GET", new DownloadHandlerBuffer(), null);
        request.SendWebRequest().completed += asyncOp =>
        {
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.Log(request.error);
                return;
            }

            System.IO.File.WriteAllText(Application.dataPath + "/Resources/BoostersConfig.json",
                request.downloadHandler.text);

            Debug.Log("Boosters config updated with text: " + request.downloadHandler.text);
        };
    }
}
