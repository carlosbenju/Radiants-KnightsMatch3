using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButtonView : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] TMP_Text _levelNumberText;
    Action<int> _onIconSelected;

    LevelModel _levelModel;

    GameProgressionService _progressionService;

    void Awake()
    {
        _button.interactable = false;
    }

    public void Initialize(LevelModel level,GameProgressionService progression, Action<int> onIconSelected)
    {
        _levelModel = level;
        _progressionService = progression;

        _levelNumberText.text = _levelModel.LevelNumber.ToString();
        _onIconSelected = onIconSelected;
        _button.interactable = _progressionService.Data.CurrentLevel >= level.LevelNumber ? true : false;
    }

    public void OnClick()
    {
        _onIconSelected?.Invoke(_levelModel.LevelNumber);
    }
}
