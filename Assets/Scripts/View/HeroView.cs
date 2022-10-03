using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroView : MonoBehaviour
{
    [SerializeField] HeroModel _model = null;

    [SerializeField] Image _characterImage = null;
    [SerializeField] List<Sprite> _heroSprites = null;

    public void SetData(HeroModel model)
    {
        _model = model;

        _characterImage.sprite = _heroSprites.Find(sprite => sprite.name == _model.HeroSprite);
    }
}
