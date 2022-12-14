using System;
using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Game.Services
{
    public class RemoteConfigGameService : IService
    {
        struct appData
        {
        }

        struct userData
        {
        }

        [Serializable]
        class Wrapper<T>
        {
            public T data;
        }

        RuntimeConfig _config;

        public async Task Initialize()
        {
            _config = await RemoteConfigService.Instance.FetchConfigsAsync(new userData(), new appData());

            switch (_config.origin)
            {
                case ConfigOrigin.Default:
                    Debug.Log("No settings loaded this session; using default values.");
                    break;
                case ConfigOrigin.Cached:
                    Debug.Log("No settings loaded this session; using cached values from a previous session.");
                    break;
                case ConfigOrigin.Remote:
                    Debug.Log("New settings loaded this session; update values accordingly.");
                    break;
            }
        }

        public string GetString(string key, string defaultValue = "")
        {
            return _config?.GetString(key, defaultValue) ?? defaultValue;
        }

        public string GetJson(string key, string defaultValue = "")
        {
            return _config?.GetJson(key, defaultValue) ?? defaultValue;
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return _config?.GetInt(key, defaultValue) ?? defaultValue;
        }

        public float GetFloat(string key, float defaultValue = 0)
        {
            return _config?.GetFloat(key, defaultValue) ?? defaultValue;
        }

        public T Get<T>(string key, T defaultValue = default)
        {
            string data = _config?.GetString(key, "{}");
            if (string.IsNullOrEmpty(data))
            {
                return defaultValue;
            }

            try
            {
                return JsonUtility.FromJson<Wrapper<T>>(data).data;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return defaultValue;
            }
        }

        public void Clear()
        {
        }
    }
}

