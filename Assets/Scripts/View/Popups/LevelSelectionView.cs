using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionView : MonoBehaviour
{
    [SerializeField] LevelButtonView _levelSelectionPrefab;
    [SerializeField] GameObject[] _levelSpawns;

    LevelSO[] _levelSOs;
    List<LevelModel> _levels;

    Action<int> _onPopupClosed;

    GameProgressionService _progressionService;

    public void Initialize(Action<int> onPopupClosed)
    {
        _progressionService = ServiceLocator.GetService<GameProgressionService>();
        _levelSOs = LevelsDatabase.Levels;
        _levels = new List<LevelModel>();
        _onPopupClosed = onPopupClosed;

        foreach (LevelSO levelSO in _levelSOs)
        {
            LevelModel level = new LevelModel(levelSO);

            if (_progressionService.Data.CompletedLevels != null)
            {
                foreach (LevelModel levelModel in _progressionService.Data.CompletedLevels)
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
                .Initialize(_levels[i], _progressionService, SelectLevel);
        }
    }

    public void SelectLevel(int level)
    {
        _onPopupClosed?.Invoke(level);
        Close();
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
