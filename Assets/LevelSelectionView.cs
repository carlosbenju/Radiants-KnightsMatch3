using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelSelectionView : MonoBehaviour
{
    [SerializeField] LevelButtonView _levelSelectionPrefab;
    [SerializeField] GameObject[] _levelSpawns;

    LevelSO[] _levelSOs;
    PlayerModel _player;
    List<LevelModel> _levels;

    AsyncOperationHandle _levelSelectionPopupHandle;

    Action _onPopupClosed;

    public void Initialize(PlayerModel playerModel, Action onPopupClosed, AsyncOperationHandle handle)
    {
        _player = playerModel;
        _levelSOs = LevelsDatabase.Levels;
        _levels = new List<LevelModel>();
        _onPopupClosed = onPopupClosed;
        _levelSelectionPopupHandle = handle;
        

        foreach (LevelSO levelSO in _levelSOs)
        {
            LevelModel level = new LevelModel(levelSO);

            if (_player.Data.CompletedLevels != null)
            {
                foreach (LevelModel levelModel in _player.Data.CompletedLevels)
                {
                    if (levelModel.LevelNumber == levelSO.Level)
                    {
                        level.IsCompleted = true;
                    }
                }
            }

            _levels.Add(level);
        }

        for (int i = 0; i < _levels.Count; i++)
        {
            Instantiate(_levelSelectionPrefab, _levelSpawns[i].transform.position, Quaternion.identity, _levelSpawns[i].transform)
                .Initialize(_levels[i], SelectLevel);
        }
    }

    public void SelectLevel(int level)
    {
        _player.Data.CurrentLevel = level;
        Debug.Log("Current level: " + _player.Data.CurrentLevel);
        Close();
    }

    public void Close()
    {
        _onPopupClosed?.Invoke();
        if (_levelSelectionPopupHandle.IsValid())
        {
            Addressables.Release(_levelSelectionPopupHandle);
        }

        Destroy(gameObject);
    }
}
