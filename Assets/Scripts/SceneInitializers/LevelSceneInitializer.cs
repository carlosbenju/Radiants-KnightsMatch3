using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSceneInitializer : MonoBehaviour
{
    [SerializeField] LevelView _levelPrefab = null;
    [SerializeField] LevelSO _level = null;

    LevelController _levelController = null;
    BoardController _boardController;
    Inventory _inventory;
    PlayerModel _player;

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
        LevelModel level = new LevelModel(_level);
        _player = new PlayerModel();
        _player.Initialize();
        _levelController = new LevelController(level, _player);

        _boardController = new BoardController(new BoardModel(7, 5));

        _inventory = new Inventory();
        _inventory.Load();

        Instantiate(_levelPrefab).Initialize(_levelController, _boardController, _inventory);
    }
}
