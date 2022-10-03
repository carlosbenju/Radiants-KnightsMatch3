using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController
{
    public LevelModel Level;
    public EnemyModel CurrentEnemy;
    public HeroModel Hero;
    public Action OnWaveChanged = delegate { };
    public Action OnTurnPassed = delegate { };

    int _waveIndex;
    bool isFirstCompletion;

    public Action OnGameWon;
    public Action OnGameLost;

    GameProgressionService _gameProgressionService;

    public LevelController(LevelModel level)
    {
        Level = level;
        _gameProgressionService = ServiceLocator.GetService<GameProgressionService>();
    }

    public void InitializeLevel()
    {
        Hero = new HeroModel();
        Hero.InitializeById(_gameProgressionService.Data.CurrentHeroId);

        _waveIndex = 0;
        CurrentEnemy = new EnemyModel(Level.Waves[_waveIndex]);

        if (_gameProgressionService.Data.CompletedLevels.Count > 0)
        {
            foreach (LevelModel level in _gameProgressionService.Data.CompletedLevels)
            {
                isFirstCompletion = Level.LevelNumber == level.LevelNumber ? false : true;
            }
        } else
        {
            isFirstCompletion = true;
        }
    }

    public void AttackToEnemy(float damage, TileType attackType)
    {
        CurrentEnemy.ReceiveDamage(damage + Hero.Strength, attackType);

        if (IsCurrentEnemyDead())
        {
            GoToNextWave();
        }
        else
        {
            EnemyTryToAttack();
        }
    }

    bool IsCurrentEnemyDead()
    {
        if (CurrentEnemy.CurrentHealth != 0)
        {
            return false;
        }

        return true;
    }

    void EnemyTryToAttack()
    {
        if (EnemyCanAttack())
        {
            Hero.ReceiveDamage(CurrentEnemy.Strength, CurrentEnemy.Type);
            CurrentEnemy.TurnsToAttack = CurrentEnemy.AttackInterval;
            if (IsPlayerDead())
            {
                OnGameLost?.Invoke();
            }

            OnTurnPassed?.Invoke();
            return;
        }

        CurrentEnemy.TurnsToAttack--;
        OnTurnPassed?.Invoke();
    }

    private bool IsPlayerDead()
    {
        if (Hero.CurrentHealth != 0)
        {
            return false;
        }

        return true;
    }

    bool EnemyCanAttack()
    {
        if (CurrentEnemy.TurnsToAttack == 0)
        {
            return true;
        }

        return false;
    }

    void GoToNextWave()
    {
        if (_waveIndex < Level.Waves.Length - 1)
        {
            _waveIndex++;
            CurrentEnemy = new EnemyModel(Level.Waves[_waveIndex]);
            OnWaveChanged?.Invoke();
        }
        else
        {
            WinGame();
        }
    }

    void WinGame()
    {
        if (isFirstCompletion)
        {
            foreach (InventoryItem reward in Level.Rewards)
            {
                _gameProgressionService.Data.Inventory.Add(reward);
                _gameProgressionService.Data.Inventory.Save();
            }

            Level.IsCompleted = true;
            _gameProgressionService.Data.CompletedLevels.Add(Level);
            _gameProgressionService.Data.CurrentLevel++;
            _gameProgressionService.Save();
        }

        OnGameWon?.Invoke();
        OnTurnPassed?.Invoke();
    }
}
