using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroesDatabase : MonoBehaviour
{
    public static HeroSO[] Heroes { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Heroes = Resources.LoadAll<HeroSO>("Heroes/");
    }
}
