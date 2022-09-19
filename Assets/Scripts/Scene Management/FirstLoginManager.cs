using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FirstLoginManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerNameInput;
    [SerializeField] string sceneToLoad;
    MasterSceneManager masterSceneManager;

    private void Awake()
    {
        masterSceneManager = FindObjectOfType<MasterSceneManager>();
    }

    public void SelectName()
    {
        PlayerModel playerModel = new PlayerModel();

        playerModel.CreateNewUser(playerNameInput.text);
        playerModel.Save();

        masterSceneManager.LoadScene(sceneToLoad);
    }
}
