using System;
using UnityEngine;

public class EnemyModel
{
    public EnemySO Enemy;
    public string EnemySprite;
    public TileType Type;
    public int Health;
    public int Strength;
    public int CurrentHealth;
    public int AttackInterval;
    public int TurnsToAttack;

    public Action<int> OnCurrentHealthModified = delegate(int currentHealth) { };
    public Action OnEnemyDead = delegate { };

    public EnemyModel(EnemySO enemy)
    {
        Enemy = enemy;

        Initialize();
    }

    public void Initialize()
    {
        EnemySprite = Enemy.Image;
        Type = Enemy.Type;
        Health = Enemy.Health;
        CurrentHealth = Enemy.Health;
        AttackInterval = Enemy.AttackInterval;
        TurnsToAttack = Enemy.AttackInterval;
        Strength = Enemy.Strength;
    }

    public void ReceiveDamage(float damage, TileType attackType)
    {
        switch (Type)
        {
            case TileType.Red:
                if (attackType == TileType.Blue)
                {
                    damage *= 1.5f;
                }
                else if (attackType == TileType.Green)
                {
                    damage *= .5f;
                }
                break;
            case TileType.Blue:
                if (attackType == TileType.Green)
                {
                    damage *= 1.5f;
                }
                else if (attackType == TileType.Red)
                {
                    damage *= .5f;
                }
                break;
            case TileType.Green:
                if (attackType == TileType.Red)
                {
                    damage *= 1.5f;
                }
                else if (attackType == TileType.Blue)
                {
                    damage *= .5f;
                }
                break;
        }

        int finalDamage = (int)Mathf.Round(damage);
        CurrentHealth = Mathf.Max(0, CurrentHealth - finalDamage);
    }
}
