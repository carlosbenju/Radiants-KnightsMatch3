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

        await loginService.Initialize();
        await remoteConfig.Initialize();

        InventoryConfig inventoryConfig = new InventoryConfig();
        inventoryConfig.Load(remoteConfig);
    } 
}
