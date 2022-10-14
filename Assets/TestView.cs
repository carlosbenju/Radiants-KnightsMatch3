using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Services;
using System.Threading.Tasks;

public class TestView : MonoBehaviour
{
    [SerializeField]
    ResourceView _blueprint;

    [SerializeField]
    Transform _parent;

    private async void Start()
    {
        await Initialize();
    }

    public async Task Initialize()
    {
        //ServicesInitializer servicesInitializer = new ServicesInitializer("development");

        //LoginGameService loginService = new LoginGameService();
        //GameProgressionTestService progressionService = new GameProgressionTestService();
        //RemoteConfigGameService remoteConfig = new RemoteConfigGameService();
        //ResourceInventoryConfig resourcesProgression = new ResourceInventoryConfig();
        //IconCollectiblesInventoryConfig iconsConfig = new IconCollectiblesInventoryConfig();
        //IGameProgressionProvider progressionProvider = new GameProgressionProvider();

        //await servicesInitializer.Initialize();
        //await loginService.Initialize();
        //await remoteConfig.Initialize();
        //resourcesProgression.Initialize(remoteConfig);
        //iconsConfig.Initialize(remoteConfig);
        ////progressionService.Initialize(resourcesProgression, iconsConfig, progressionProvider);

        //progressionService.CreateUser("Enygma");
        //progressionService.Save();
        //progressionService.Load();

        //foreach (InGameResourceConfig resource in progressionService.ResourceConfig.Resources)
        //{
        //    ResourceBlueprint view = Instantiate(_blueprint, _parent);
        //    view.Amount.text = progressionService.GetResourceAmount(resource.Id).ToString();
        //}
    }

}
