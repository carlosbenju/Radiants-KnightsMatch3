using Game.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController
{
    public ShopConfig Config { get; private set; }

    GameProgressionTestService _gameProgression;

    ResourceInventoryProgression _resourceProgression;
    BoostersInventoryProgression _boosterProgression;
    IconCollectibleProgression _iconProgression;

    public ShopController(ShopConfig config, GameProgressionTestService gameProgression)
    {
        Config = config;
        _gameProgression = gameProgression;

        _resourceProgression = _gameProgression.ResourceProgression;
        _boosterProgression = _gameProgression.BoostersProgression;
        _iconProgression = _gameProgression.IconProgression;
    }

    public void PurchaseItem(ShopItemModel model)
    {
        if (_resourceProgression.GetResourceAmount(model.CostType) < model.CostAmount)
        {
            Debug.Log("The user does not have enough to pay");
        }

        if (model.RewardType == "Booster")
        {
            _resourceProgression.RemoveResource(model.CostType, model.CostAmount);
            _boosterProgression.AddBooster(model.RewardName, model.RewardAmount);
            return;
        }

        if (model.RewardType == "Resource")
        {
            _resourceProgression.RemoveResource(model.CostType, model.CostAmount);
            _resourceProgression.AddResource(model.RewardName, model.RewardAmount);
            return;
        }

        if (model.RewardType == "Icon")
        {
            _resourceProgression.RemoveResource(model.CostType, model.CostAmount);
            _iconProgression.AddIcon(model.RewardName);
            return;
        }
    }
}
