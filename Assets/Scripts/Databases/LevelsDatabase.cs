using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsDatabase : MonoBehaviour
{
    public static LevelSO[] Levels { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Levels = Resources.LoadAll<LevelSO>("Levels/");
    }
}
