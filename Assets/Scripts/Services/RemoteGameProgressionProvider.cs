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
        Application.focusChanged += SaveToCloud;
    }

    public async void SaveToCloud(bool hasFocus)
    {
        Debug.Log("Remote data:  "+ _remoteData);
        
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
        try
        {
            Dictionary<string, string> data = await CloudSaveService.Instance.Data.LoadAsync();

            data.TryGetValue("data", out _remoteData);
            Debug.Log("Loaded: " + _remoteData + " for user: " + AuthenticationService.Instance.PlayerId);
            return true;
        } catch (Exception e)
        {
            Debug.LogError(e);
        }

        return false;
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
