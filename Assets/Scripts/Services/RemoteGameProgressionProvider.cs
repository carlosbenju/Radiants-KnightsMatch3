using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Services;
using System.Threading.Tasks;
using System;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;

public class RemoteGameProgressionProvider : IGameProgressionProvider
{
    string _remoteData;

    public RemoteGameProgressionProvider()
    {
        Application.focusChanged += OnApplicationFocusChanged;
    }

    async void OnApplicationFocusChanged(bool hasFocus)
    {
        if (!hasFocus)
        {
            try
            {
                await CloudSaveService.Instance.Data
                    .ForceSaveAsync(new Dictionary<string, object> { { "data", _remoteData } });
            } 
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            Debug.Log("Loaded " + _remoteData + " for user " + AuthenticationService.Instance.PlayerId);
        }
    }

    public async Task<bool> Initialize()
    {
        Dictionary<string, string> data = await CloudSaveService.Instance.Data.LoadAsync();
        foreach (var keyValuePair in data)
        {
            Debug.Log("Key: " + keyValuePair.Key + " Value: " + keyValuePair.Value);
        }

        data.TryGetValue("data", out _remoteData);
        Debug.Log("Loaded: " + _remoteData + " for user: " + AuthenticationService.Instance.PlayerId);
        return true;
    }

    public string Load()
    {
        return _remoteData;
    }

    public void Save(string data)
    {
        _remoteData = data;
    }
}
