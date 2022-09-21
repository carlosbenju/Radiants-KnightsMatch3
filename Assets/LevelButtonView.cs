using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButtonView : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] TMP_Text _levelNumberText;
    Action<int> _onIconSelected;

    LevelModel _levelModel;

    public void Initialize(LevelModel level, Action<int> onIconSelected)
    {
        _levelModel = level;
        _button.interactable = _levelModel.IsCompleted ? true : false;
        _levelNumberText.text = _levelModel.LevelNumber.ToString();
        _onIconSelected = onIconSelected;
    }

    public void OnClick()
    {
        _onIconSelected?.Invoke(_levelModel.LevelNumber);
    }
}
