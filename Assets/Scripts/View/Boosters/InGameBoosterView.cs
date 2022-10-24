using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameBoosterView : MonoBehaviour
{
    public string BoosterType;
    public Image Icon;
    public TMP_Text Amount;

    Action<string> OnBoosterClicked;

    public void Initialize(string type, int amount, Action<string> onBoosterClicked)
    {
        BoosterType = type;
        Amount.text = amount.ToString();
        OnBoosterClicked = onBoosterClicked;
    }

    public void OnClick()
    {
        OnBoosterClicked?.Invoke(BoosterType);
    }
}
