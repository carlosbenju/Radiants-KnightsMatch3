using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostersDatabase : MonoBehaviour
{
    public static ItemSO[] Boosters { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Boosters = Resources.LoadAll<ItemSO>("Boosters/");
    }
}
