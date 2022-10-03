using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoosterInventoryView : MonoBehaviour
{
    // TODO: Create Booster View and Model and refactor this to be scalable
    [SerializeField] Image _bombBoosterIcon;
    [SerializeField] TextMeshProUGUI _bombBoosterAmount;

    [SerializeField] Image _colorBombBoosterIcon;
    [SerializeField] TextMeshProUGUI _colorBombBoosterAmount;

    [SerializeField] List<Sprite> _boosterSprites;

    Inventory _inventory;

    public void Initialize(Inventory inventory)
    {
        _inventory = inventory;
        _inventory.OnResourceModified += UpdateResource;
        UpdateBoosterInventoryView();
    }

    private void OnDestroy()
    {
        if (_inventory != null)
        {
            _inventory.OnResourceModified -= UpdateResource;
        }
    }

    void UpdateBoosterInventoryView()
    {
        UpdateResource("Bomb Booster");
        UpdateResource("Color Bomb Booster");
    }

    void UpdateResource(string resource)
    {
        switch (resource)
        {
            case "Bomb Booster":
                _bombBoosterAmount.text = _inventory.GetAmount("Bomb Booster").ToString();
                _bombBoosterIcon.sprite = _boosterSprites.Find(sprite => sprite.name == "Bomb");
                break;
            case "Color Bomb Booster":
                _colorBombBoosterAmount.text = _inventory.GetAmount("Color Bomb Booster").ToString();
                _colorBombBoosterIcon.sprite = _boosterSprites.Find(sprite => sprite.name == "Color Bomb");
                break;
        }
    }

}
