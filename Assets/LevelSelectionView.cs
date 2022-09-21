using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionView : MonoBehaviour
{
    [SerializeField] LevelButtonView _levelSelectionPrefab;
    [SerializeField] GameObject[] _levelSpawns;

    LevelSO[] _levelSOs;
    PlayerModel _player;
    List<LevelModel> _levels;

    public void Initialize(PlayerModel playerModel)
    {
        _player = playerModel;
        _levelSOs = LevelsDatabase.Levels;
        _levels = new List<LevelModel>();

        foreach (LevelSO levelSO in _levelSOs)
        {
            LevelModel level = new LevelModel(levelSO);

            if (_player.playerData.CompletedLevels != null)
            {
                foreach (LevelModel levelModel in _player.playerData.CompletedLevels)
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
        _player.playerData.CurrentLevel = level;
    }
}
