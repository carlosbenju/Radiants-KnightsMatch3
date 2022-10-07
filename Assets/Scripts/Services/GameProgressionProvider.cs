using Game.Services;
using System.Threading.Tasks;
using UnityEngine;

public class GameProgressionProvider : IGameProgressionProvider
{
    FileGameProgressionProvider _localData = new FileGameProgressionProvider();
    RemoteGameProgressionProvider _remoteData = new RemoteGameProgressionProvider();

    public async Task<bool> Initialize()
    {
        await Task.WhenAll(_localData.Initialize(), _remoteData.Initialize());
        return true;
    }

    public string Load()
    {
        string localData = _localData.Load();
        string remoteData = _remoteData.Load();

        if (string.IsNullOrEmpty(localData) && !string.IsNullOrEmpty(remoteData))
        {
            return remoteData;
        }

        if (!string.IsNullOrEmpty(localData) && string.IsNullOrEmpty(remoteData))
        {
            return localData;
        }

        return remoteData;
    }

    public void Save(string data)
    {
        _localData.Save(data);
        // _remoteData.Save(data);
    }
}
