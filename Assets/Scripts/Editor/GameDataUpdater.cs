using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public static class GameDataUpdater
{
    [MenuItem("Game/Update Shop Data")]
    public static void UpdateShopModel()
    {
        string url = 
            "https://script.google.com/macros/s/AKfycbxjfrDr55URJNQry3OoMJUdwol7f1FReft_lN-2QudoMfFX7URf6T7-f3StziDkGQ4l/exec";

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
}
