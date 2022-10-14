using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Services;
using System.Threading.Tasks;

public class TestRemote : MonoBehaviour
{
    private async void Start()
    {
        await InitializeServices();
    }

    async Task InitializeServices()
    {
        ServicesInitializer servicesInitializer = new ServicesInitializer("development");

        LoginGameService loginService = new LoginGameService();
        GameProgressionTestService progressionService = new GameProgressionTestService();
        RemoteConfigGameService remoteConfig = new RemoteConfigGameService();
        ResourceInventoryConfig resourcesProgression = new ResourceInventoryConfig();
        IconCollectiblesInventoryConfig iconsConfig = new IconCollectiblesInventoryConfig();
        IGameProgressionProvider progressionProvider = new FileGameProgressionProvider();

        await servicesInitializer.Initialize();
        await loginService.Initialize();
        await remoteConfig.Initialize();
        progressionService.Initialize(remoteConfig, progressionProvider);
        progressionService.Load();
        progressionService.Save();

        //progressionService.CreateUser("Enygma");
        //progressionService.Save();
    } 
}
