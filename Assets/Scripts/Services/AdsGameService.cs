using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Game.Services
{
    public class AdsGameService : IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener, IService
    {
        string _adsGameId;
        string _adUnitId;

        public bool IsAdReady => IsInitialized && Advertisement.IsReady(_adUnitId);
        public bool IsInitialized => _initializationTaskStatus == TaskStatus.RanToCompletion;

        TaskStatus _initializationTaskStatus = TaskStatus.Created;
        TaskStatus _showTaskStatus = TaskStatus.Created;

        public AdsGameService(string adsGameId, string adUnityId)
        {
            _adsGameId = adsGameId;
            _adUnitId = adUnityId;
        }

        public async Task<bool> Initialize(bool testMode = false)
        {
            _initializationTaskStatus = TaskStatus.Running;
            Advertisement.Initialize(_adsGameId, testMode, true, this);
            while (_initializationTaskStatus == TaskStatus.Running)
            {
                await Task.Delay(500);
            }

            return IsInitialized;
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
            LoadAd();
            _initializationTaskStatus = TaskStatus.RanToCompletion;
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads initialization Failed: {error.ToString()} - {message}");
        }

        public void LoadAd()
        {
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }

        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            Debug.Log("Ad Loaded: " + adUnitId);
        }

        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unity {adUnitId}: {error.ToString()} - {message}");
            Advertisement.Load(_adUnitId, this);
        }

        public async Task<bool> ShowAd()
        {
            if (_showTaskStatus == TaskStatus.Running)
                return false;

            if (!IsInitialized)
                return false;

            if (!IsAdReady)
                return false;

            _showTaskStatus = TaskStatus.Running;

            Advertisement.Show(_adUnitId, this);
#if UNITY_EDITOR
            await Task.Delay(2000);
            OnUnityAdsShowComplete(_adUnitId, UnityAdsShowCompletionState.COMPLETED);
#endif
            while (_showTaskStatus == TaskStatus.Running)
            {
                await Task.Delay(500);
            }

            return _showTaskStatus == TaskStatus.RanToCompletion;
        }

        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            Debug.Log("Unity Ads Rewarded Ad: " + showCompletionState);
            Advertisement.Load(_adUnitId, this);
            _showTaskStatus = showCompletionState == UnityAdsShowCompletionState.COMPLETED ? TaskStatus.RanToCompletion : TaskStatus.Faulted;
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unity {adUnitId}: {error.ToString()} - {message}");
            Advertisement.Load(_adUnitId, this);
            _showTaskStatus = TaskStatus.Faulted;
        }

        public void OnUnityAdsShowStart(string adUnitId)
        {
            Debug.Log("Started watching an ad");
        }

        public void OnUnityAdsShowClick(string adUnitId)
        {
            Debug.Log("User clicked in the ad");
        }

        public void Clear()
        {
        }
    }
}
