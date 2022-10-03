using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSceneInitializer : MonoBehaviour
{
    [SerializeField] 
    MainMenuView _mainMenuView = null;

    Inventory _inventory = null;

    MasterSceneManager _masterSceneManager = null;

    private void Awake()
    {
        _masterSceneManager = FindObjectOfType<MasterSceneManager>();
        _masterSceneManager.OnSceneCompleteLoading += Initialize;
    }

    private void OnDestroy()
    {
        _masterSceneManager.OnSceneCompleteLoading -= Initialize;
    }

    void Initialize()
    {
        _inventory = new Inventory();
        _inventory.Load();

        _mainMenuView.Initialize(_inventory);
    }
}
