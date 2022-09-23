using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDatabase : MonoBehaviour
{
    public static ItemSO[] Items { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Items = Resources.LoadAll<ItemSO>("Items/");
    }
}
