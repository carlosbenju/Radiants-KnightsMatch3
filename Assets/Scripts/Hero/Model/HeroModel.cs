using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroModel
{
    public int Id;
    public string Name;
    public string HeroSprite;
    public TileType Type;
    public int Health;
    public int CurrentHealth;
    public int Strength;

    public void Initialize(HeroSO hero)
    {
        Id = hero.Id;
        Name = hero.Name;
        HeroSprite = hero.Image;
        Type = hero.Type;
        Health = hero.Health;
        CurrentHealth = hero.Health;
        Strength = hero.Strength;
    }

    public void InitializeById(int id)
    {
        HeroSO[] heroes = HeroesDatabase.Heroes;
        HeroSO hero = null;

        foreach (HeroSO h in heroes)
        {
            if (h.Id == id) hero = h;
        }

        Id = hero.Id;
        Name = hero.Name;
        HeroSprite = hero.Image;
        Type = hero.Type;
        Health = hero.Health;
        CurrentHealth = hero.Health;
        Strength = hero.Strength;
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
