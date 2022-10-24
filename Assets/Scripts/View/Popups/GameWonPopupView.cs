using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameWonPopupView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _goldAmountOnWinText;

    LevelModel _level = null;

    public void Initialize(LevelModel level)
    {
        _level = level;

        List<Reward> Rewards = _level.Rewards;

        foreach (Reward reward in Rewards)
        {
            if (reward.Type == "Gold")
            {
                StartCoroutine(SetGoldAmountSlowly(reward.Amount));
            }
        }
    }

    IEnumerator SetGoldAmountSlowly(int amount)
    {
        int goldAmount = 0;
        while (goldAmount < amount)
        {
            goldAmount++;
            _goldAmountOnWinText.text = goldAmount.ToString();
            yield return null;
        }
    }
}
