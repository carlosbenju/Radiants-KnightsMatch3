using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController
{
    public EnemyModel Model = null;

    public EnemyController(EnemyModel model)
    {
        Model = model;
    }

    public void ReceiveDamage(float damage, TileType attackType)
    {

    }
}
