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
    [SerializeField] GameObject _parent;

    [SerializeField] Slider _heroHealthSlider;
    [SerializeField] Slider _enemyHealthSlider;
    [SerializeField] TextMeshProUGUI _heroHealthText;
    [SerializeField] TextMeshProUGUI _enemyHealthText;
    [SerializeField] TextMeshProUGUI _enemyTurnsToAttackText;
    [SerializeField] TextMeshProUGUI _bombBoosterAmount;
    [SerializeField] TextMeshProUGUI _colorBombBoosterAmount;
    [SerializeField] Image _bombBoosterImage;
    [SerializeField] Image _colorBombBoosterImage;
    [SerializeField] List<Sprite> _boosterSprites;

    EnemyView _currentEnemyView;

    LevelController _levelController;
    BoardController _boardController;
    Inventory _inventory;


    public void Initialize(LevelController levelController, BoardController boardController, Inventory inventory)
    {
        _levelController = levelController;
        _boardController = boardController;
        _inventory = inventory;

        _levelController.InitializeLevel();

        _boardController.OnTileDestroyed += OnTilesDestroyed;
        _levelController.OnWaveChanged += SpawnEnemy;
        _levelController.OnWaveChanged += UpdateHealthBarAndText;
        _levelController.OnTurnPassed += UpdateHealthBarAndText;
        _levelController.OnGameWon += OpenWinPopup;
        _levelController.OnGameLost += OpenLosePopup;

        Instantiate(_boardPrefab).Initialize(_boardController);
        Instantiate(_heroPrefab, _heroSpawn.transform.position, Quaternion.identity, _parent.transform)
            .SetData(_levelController.Hero);

        SpawnEnemy();

        UpdateInventoryView();
        UpdateHealthBarAndText();
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
    }

    void SpawnEnemy()
    {
        if (_currentEnemyView != null) 
            Destroy(_currentEnemyView.gameObject);

        _currentEnemyView = Instantiate(_enemyPrefab, _enemySpawn.transform.position, Quaternion.identity, _parent.transform);
        _currentEnemyView.SetData(_levelController.CurrentEnemy);
        _currentEnemyView.GetComponent<Image>().SetNativeSize();
    }

    void OnTilesDestroyed(float quantity, TileType type)
    {
        _levelController.AttackToEnemy(quantity, type);
    }

    void UpdateInventoryView()
    {
        _bombBoosterAmount.text = _inventory.GetAmount("Bomb Booster").ToString();
        _bombBoosterImage.sprite = _boosterSprites.Find(sprite => sprite.name == "Bomb");
        _colorBombBoosterAmount.text = _inventory.GetAmount("Color Bomb Booster").ToString();
        _colorBombBoosterImage.sprite = _boosterSprites.Find(sprite => sprite.name == "Color Bomb");
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

    public void ClickOnBombBoosterButton()
    {
        if (_inventory.GetAmount("Bomb Booster") > 0)
        {
            _boardController.CreateBomb();
            _inventory.Remove(new InventoryItem { Type = "Bomb Booster", Amount = 1 });

            UpdateInventoryView();
        }
    }

    public void ClickOnColorBombBoosterButton()
    {
        if (_inventory.GetAmount("Color Bomb Booster") > 0)
        {
            _boardController.CreateColorBomb();
            _inventory.Remove(new InventoryItem { Type = "Color Bomb Booster", Amount = 1 });

            UpdateInventoryView();
        }
    }
} 
