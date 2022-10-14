using Game.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSceneInitializer : MonoBehaviour
{
    [SerializeField] ShopView _shopView = null;

    ShopController _shopController = null;

    MasterSceneManager _masterSceneManager = null;

    GameProgressionTestService _gameProgression;
    AnalyticsGameService _analyticsService;

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
        _gameProgression = ServiceLocator.GetService<GameProgressionTestService>();
        _analyticsService = ServiceLocator.GetService<AnalyticsGameService>();

        _shopController = new ShopController(_gameProgression.ShopConfig, _gameProgression);

        _shopView.Initialize(_gameProgression, _shopController);
        _analyticsService.SendEvent("enterShop");
    }
}
