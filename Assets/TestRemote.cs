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
        RemoteConfigGameService remoteConfig = new RemoteConfigGameService();
        GameProgressionService gameProgressionService = new GameProgressionService();
        GameConfigService gameConfigService = new GameConfigService();
        IGameProgressionProvider progressionProvider = new GameProgressionProvider();

        ServiceLocator.RegisterService(gameProgressionService);

        await servicesInitializer.Initialize();
        await loginService.Initialize();
        await remoteConfig.Initialize();
        gameConfigService.Initialize(remoteConfig);
        gameProgressionService.Initialize(gameConfigService, progressionProvider);

        ResourceInventoryConfig inventoryConfig = new ResourceInventoryConfig();
        inventoryConfig.Load(remoteConfig);

        ResourceInventoryProgression inventoryProgression = new ResourceInventoryProgression(inventoryConfig);
        inventoryProgression.Load();
        inventoryProgression.Save();

        ResourceInventoryProgression newProgression = new ResourceInventoryProgression(inventoryConfig);
        newProgression.Load();
        newProgression.AddResource("gold", 700);
        newProgression.Save();
    } 
}
