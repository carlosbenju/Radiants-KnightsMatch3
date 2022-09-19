using Game.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSceneInitializer : MonoBehaviour
{
    [SerializeField] ShopView _shopView = null;

    Inventory _inventory = null;
    ShopController _shopController = null;

    MasterSceneManager _masterSceneManager = null;

    AnalyticsGameService _analyticsService = null;

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
        _analyticsService = ServiceLocator.GetService<AnalyticsGameService>();
        _inventory = new Inventory();
        _inventory.Load();

        _shopController = new ShopController(_inventory);
        _shopController.Load();

        _shopView.Initialize(_shopController, _inventory);
        _analyticsService.SendEvent("enterShop");
    }
}
