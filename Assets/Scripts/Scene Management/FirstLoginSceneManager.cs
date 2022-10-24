using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FirstLoginSceneManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _playerNameInput;
    [SerializeField] string _sceneToLoad;
    MasterSceneManager _masterSceneManager;

    GameProgressionService _progressionService;

    private void Awake()
    {
        _masterSceneManager = FindObjectOfType<MasterSceneManager>();

        _progressionService = ServiceLocator.GetService<GameProgressionService>();
    }

    public void SelectName()
    {
        _progressionService.CreateUser(_playerNameInput.text);

        _masterSceneManager.LoadScene(_sceneToLoad);
    }
}
