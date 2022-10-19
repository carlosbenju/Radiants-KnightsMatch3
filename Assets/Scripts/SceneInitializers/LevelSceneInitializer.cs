using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSceneInitializer : MonoBehaviour
{
    [SerializeField] LevelView _mainMenuView = null;
    [SerializeField] LevelSO _level = null;

    LevelController _levelController = null;
    BoardController _boardController;

    MasterSceneManager _masterSceneManager = null;

    private void Awake()
    {
        _masterSceneManager = FindObjectOfType<MasterSceneManager>();
        _masterSceneManager.OnSceneCompleteLoading += Initialize;
    }

    private void OnDestroy()
    {
        _masterSceneManager.OnSceneCompleteLoading -= Initialize;
    }

    void Initialize()
    {
        GameProgressionService progressionService = ServiceLocator.GetService<GameProgressionService>();

        LevelModel level = new LevelModel(_level);
        _levelController = new LevelController(level);

        _boardController = new BoardController(new BoardModel(7, 5));

        _mainMenuView.Initialize(progressionService, _levelController, _boardController);
    }
}
