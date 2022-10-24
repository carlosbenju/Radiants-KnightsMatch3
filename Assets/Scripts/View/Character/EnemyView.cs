using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyView : MonoBehaviour
{
    [SerializeField] EnemyModel _model = null;

    [SerializeField] Image _characterImage = null;
    [SerializeField] List<Sprite> _enemySprites = null;

    public void SetData(EnemyModel model)
    {
        _model = model;

        _characterImage.sprite = _enemySprites.Find(sprite => sprite.name == _model.EnemySprite);
    }
}
