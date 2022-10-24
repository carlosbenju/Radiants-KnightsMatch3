using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;
using Game.Services;

public class SplashScreenSceneManager : MonoBehaviour
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

        LoginGameService loginService = new LoginGameService();
        GameProgressionService progressionService = new GameProgressionService();
        GameConfigService gameConfig = new GameConfigService();
        RemoteConfigGameService remoteConfig = new RemoteConfigGameService();
        AnalyticsGameService analyticsService = new AnalyticsGameService();
        AdsGameService adsService = new AdsGameService("4931437", "Rewarded_Android");
        IAPGameService iapService = new IAPGameService();
        IGameProgressionProvider gameProgressionProvider = new GameProgressionProvider();


        ServiceLocator.RegisterService(gameConfig);
        ServiceLocator.RegisterService(remoteConfig);
        ServiceLocator.RegisterService(progressionService);
        ServiceLocator.RegisterService(loginService);
        ServiceLocator.RegisterService(analyticsService);
        ServiceLocator.RegisterService(adsService);
        ServiceLocator.RegisterService(iapService);

        await servicesInitializer.Initialize();
        await loginService.Initialize();
        await remoteConfig.Initialize();
        await analyticsService.Initialize();
        await adsService.Initialize(Application.isEditor);
        Dictionary<string, string> productsDicc = new Dictionary<string, string>
        {
            ["50 Diamonds Pack"] = "com.enygmagames.radiants.50diamondspack",
            ["200 Diamonds Pack"] = "com.enygmagames.radiants.200diamondspack"
        };
        await iapService.Initialize(productsDicc);
        await gameProgressionProvider.Initialize();
        progressionService.Initialize(remoteConfig, gameProgressionProvider);

        SceneManager.LoadScene("Master Scene");
    }
}
