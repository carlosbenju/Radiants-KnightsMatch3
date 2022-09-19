using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;
using Game.Services;

public class SplashScreenLogic : MonoBehaviour
{
    [SerializeField]
    TMP_Text _gameVersionText;

    [SerializeField]
    bool _isDevBuild;

    TaskCompletionSource<bool> _cancellationTaskSource;

    void Start()
    {
        _gameVersionText.text = $"Version: {Application.version}";
        
        _cancellationTaskSource = new TaskCompletionSource<bool>();

        LoadServicesCancellable().ContinueWith(task => Debug.Log(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
    }

    void OnDestroy()
    {
        _cancellationTaskSource.SetResult(true);
    }

    async Task LoadServicesCancellable()
    {
        await Task.WhenAny(LoadServices(), _cancellationTaskSource.Task);
    }

    async Task LoadServices()
    {
        string environmentId = _isDevBuild ? "development" : " production";

        ServicesInitializer servicesInitializer = new ServicesInitializer(environmentId);

        LoginGameService login = new LoginGameService();
        GameConfigService gameConfig = new GameConfigService();
        RemoteConfigGameService remoteConfig = new RemoteConfigGameService();
        AnalyticsGameService analyticsService = new AnalyticsGameService();
        AdsGameService adsService = new AdsGameService("4931437", "Rewarded_Android");

        ServiceLocator.RegisterService(gameConfig);
        ServiceLocator.RegisterService(remoteConfig);
        ServiceLocator.RegisterService(login);
        ServiceLocator.RegisterService(analyticsService);
        ServiceLocator.RegisterService(adsService);

        await servicesInitializer.Initialize();
        await login.Initialize();
        await remoteConfig.Initialize();
        await analyticsService.Initialize();
        bool adsInitialized = await adsService.Initialize(Application.isEditor);
        gameConfig.Initialize(remoteConfig);

        SceneManager.LoadScene("Master Scene");
    }
}
