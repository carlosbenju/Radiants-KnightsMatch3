using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _gold;
    [SerializeField] TextMeshProUGUI _diamonds;
    [SerializeField] TextMeshProUGUI _bombBoosters;
    [SerializeField] TextMeshProUGUI _colorBombBoosters;

    Inventory _inventory;

    public void Initialize(Inventory inventory)
    {
        _inventory = inventory;
        _inventory.OnResourceModified += UpdateResource;
        UpdateInventoryView();
    }

    private void OnDestroy()
    {
        if (_inventory != null)
        {
            _inventory.OnResourceModified -= UpdateResource;
        }
    }

    private void UpdateInventoryView()
    {
        UpdateResource("Gold");
        UpdateResource("Diamond");
        UpdateResource("Bomb Booster");
        UpdateResource("Color Bomb Booster");
    }

    void UpdateResource(string resource)
    {
        switch (resource)
        {
            case "Gold":
                _gold.text = _inventory.GetAmount("Gold").ToString();
                break;
            case "Diamond":
                _diamonds.text = _inventory.GetAmount("Diamond").ToString();
                break;
            case "Bomb Booster":
                _bombBoosters.text = _inventory.GetAmount("Bomb Booster").ToString();
                break;
            case "Color Bomb Booster":
                _colorBombBoosters.text = _inventory.GetAmount("Color Bomb Booster").ToString();
                break;
        }        
    }
}
