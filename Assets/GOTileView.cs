using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GOTileView : MonoBehaviour
{
    public SpriteRenderer TileSpriteRenderer;

    Tile _model;

    Action<Tile> _onTileClicked;

    bool _mouseOver = false;

    public void Initialize(Tile model, Action<Tile> onTileClicked)
    {
        _model = model;
        _onTileClicked = onTileClicked;

        TileSpriteRenderer.sprite = _model.item.sprite;
    }

    void OnMouseOver()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        OnClick();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void OnClick()
    {
        Debug.Log(_model.x + ", " + _model.y);
        _onTileClicked?.Invoke(_model);
    }
}
