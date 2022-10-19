using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FirstLoginManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerNameInput;
    [SerializeField] string sceneToLoad;
    MasterSceneManager masterSceneManager;

    GameProgressionService _progressionService;

    private void Awake()
    {
        masterSceneManager = FindObjectOfType<MasterSceneManager>();

        _progressionService = ServiceLocator.GetService<GameProgressionService>();
    }

    public void SelectName()
    {
        _progressionService.CreateUser(playerNameInput.text);

        masterSceneManager.LoadScene(sceneToLoad);
    }
}
