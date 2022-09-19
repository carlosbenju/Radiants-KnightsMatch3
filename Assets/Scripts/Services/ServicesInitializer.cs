using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public class ServicesInitializer
{
    string _environmentName;

    public ServicesInitializer(string environmentName)
    {
        _environmentName = environmentName;
    }

    public async Task Initialize()
    {
        Debug.Log("Services initializing...");
        InitializationOptions options = new InitializationOptions();
        if (!string.IsNullOrEmpty(_environmentName))
        {
            options.SetEnvironmentName(_environmentName);
        }

        await UnityServices.InitializeAsync(options);
        Debug.Log("Services initialized in: " + _environmentName);
    }
}
