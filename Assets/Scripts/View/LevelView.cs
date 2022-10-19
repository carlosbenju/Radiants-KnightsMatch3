using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;

public class LevelView : MonoBehaviour
{
    [SerializeField] BoardView _boardPrefab;
    [SerializeField] HeroView _heroPrefab;
    [SerializeField] EnemyView _enemyPrefab;

    [SerializeField] GameObject _heroSpawn;
    [SerializeField] GameObject _enemySpawn;

    [SerializeField] Slider _heroHealthSlider;
    [SerializeField] Slider _enemyHealthSlider;
    [SerializeField] TMP_Text _heroHealthText;
    [SerializeField] TMP_Text _enemyHealthText;
    [SerializeField] TMP_Text _enemyTurnsToAttackText;

    [SerializeField] InGameBoosterView _boosterPrefab;
    [SerializeField] Transform _boostersParent;
    List<InGameBoosterView> _boostersViews = new List<InGameBoosterView>();

    BoostersInventoryProgression _boostersInventoryProgression;

    EnemyView _currentEnemyView;

    LevelController _levelController;
    BoardController _boardController;

    public void Initialize(GameProgressionService progressionService, LevelController levelController, BoardController boardController)
    {
        _boostersInventoryProgression = progressionService.BoostersProgression;

        _levelController = levelController;
        _boardController = boardController;

        _levelController.InitializeLevel();

        _boostersInventoryProgression.OnBoosterModified += UpdateBooster;
        _boardController.OnTileDestroyed += OnTilesDestroyed;
        _levelController.OnWaveChanged += SpawnEnemy;
        _levelController.OnWaveChanged += UpdateHealthBarAndText;
        _levelController.OnTurnPassed += UpdateHealthBarAndText;
        _levelController.OnGameWon += OpenWinPopup;
        _levelController.OnGameLost += OpenLosePopup;

        Instantiate(_boardPrefab, transform).Initialize(_boardController);
        Instantiate(_heroPrefab, _heroSpawn.transform.position, Quaternion.identity, transform)
            .SetData(_levelController.Hero);

        SpawnEnemy();

        UpdateHealthBarAndText();
        InstantiateBoosterViews();
    }

    private void OnDestroy()
    {
        if (_boardController != null)
        {
            _boardController.OnTileDestroyed -= OnTilesDestroyed;

        }

        if (_levelController != null)
        {
            _levelController.OnWaveChanged -= SpawnEnemy;
            _levelController.OnWaveChanged -= UpdateHealthBarAndText;
            _levelController.OnTurnPassed -= UpdateHealthBarAndText;
            _levelController.OnGameWon -= OpenWinPopup;
            _levelController.OnGameLost -= OpenLosePopup;
        }

        _boostersInventoryProgression.OnBoosterModified -= UpdateBooster;
    }

    void InstantiateBoosterViews()
    {
        foreach (BoosterConfig booster in _boostersInventoryProgression.Config.Boosters)
        {
            InGameBoosterView view = Instantiate(_boosterPrefab, _boostersParent);
            int boosterAmount = _boostersInventoryProgression.GetBoosterAmount(booster.Id);
            view.Initialize(booster.Id, boosterAmount, OnBoosterClick);
            _boostersViews.Add(view);

            Addressables.LoadAssetAsync<Sprite>(booster.AssetName).Completed += handle =>
            {
                if (handle.Result != null)
                {
                    view.Icon.sprite = handle.Result;
                }
            };
        }
    }

    void SpawnEnemy()
    {
        if (_currentEnemyView != null) 
            Destroy(_currentEnemyView.gameObject);

        _currentEnemyView = Instantiate(_enemyPrefab, _enemySpawn.transform.position, Quaternion.identity, transform);
        _currentEnemyView.SetData(_levelController.CurrentEnemy);
        _currentEnemyView.GetComponent<Image>().SetNativeSize();
    }

    void OnTilesDestroyed(float quantity, TileType type)
    {
        _levelController.AttackToEnemy(quantity, type);
    }

    void UpdateBooster(string boosterId)
    {
        InGameBoosterView boosterView = _boostersViews.Find(b => b.BoosterType == boosterId);
        boosterView.Amount.text = _boostersInventoryProgression.GetBoosterAmount(boosterId).ToString();
    }

    void UpdateHealthBarAndText()
    {
        HeroModel hero = _levelController.Hero;
        EnemyModel enemy = _levelController.CurrentEnemy;

        float normalizedHeroHealth = Mathf.InverseLerp(0, hero.Health, hero.CurrentHealth);
        float normalizedEnemyHealth = Mathf.InverseLerp(0, enemy.Health, enemy.CurrentHealth);

        _heroHealthSlider.value = normalizedHeroHealth;
        _heroHealthText.text = $"{hero.CurrentHealth}/{hero.Health}";

        _enemyHealthSlider.value = normalizedEnemyHealth;
        _enemyHealthText.text = $"{enemy.CurrentHealth}/{enemy.Health}";
        _enemyTurnsToAttackText.text = enemy.TurnsToAttack.ToString();
    }

    void OpenWinPopup()
    {
        Addressables.LoadAssetAsync<GameObject>("game_won_popup").Completed += handle =>
        {
            if (handle.Result != null)
            {
                GameWonPopupView popup = handle.Result.GetComponent<GameWonPopupView>();
                Instantiate(popup).Initialize(_levelController.Level);
            }
        };
    }

    void OpenLosePopup()
    {
        Addressables.LoadAssetAsync<GameObject>("game_lost_popup").Completed += handle =>
        {
            if (handle.Result != null)
            {
                Instantiate(handle.Result);
            }
        };
    }

    public void OnBoosterClick(string clickedBoosterType)
    {
        if (clickedBoosterType == "BombBooster")
        {
            BombBoosterPressed();
        } else if (clickedBoosterType == "ColorBombBooster")
        {
            ColorBombBoosterPressed();
        }
    }

    public void BombBoosterPressed()
    {
        string booster = "BombBooster";

        if (_boostersInventoryProgression.GetBoosterAmount(booster) > 0)
        {
            _boardController.CreateBomb();
            _boostersInventoryProgression.RemoveBooster(booster, 1);
        }
    }

    public void ColorBombBoosterPressed()
    {
        string booster = "ColorBombBooster";

        if (_boostersInventoryProgression.GetBoosterAmount(booster) > 0)
        {
            _boardController.CreateColorBomb();
            _boostersInventoryProgression.RemoveBooster(booster, 1);
        }
    }
} 
