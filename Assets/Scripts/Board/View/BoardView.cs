using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    public TileView[,] tiles;
    public Row[] rows;

    BoardController _controller;
    BoardModel _board;

    int _width;
    int _height;

    Action<TileView> _onTileClickedEvent;
    Action<float, TileType> _onTilesDestroyedEvent;

    public void Initialize(BoardController controller)
    {
        _controller = controller;
        _board = _controller.Model;

        _width = _board.width;
        _height = _board.height;

        tiles = new TileView[_width, _height];

        _controller.OnTileChanged += ChangeTileVisual;

        InitializeBoard();
    }

    private void OnDestroy()
    {
        _controller.OnTileChanged -= ChangeTileVisual;
    }

    public void InitializeBoard()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                TileView tileView = rows[y].tileViews[x];
                tileView.x = x;
                tileView.y = y;
                tileView.item = _controller.GetTileInCoords(x, y).item;
                tileView.icon.sprite = tileView.item.sprite;
                tiles[x, y] = tileView;
            }
        }
    }

    public void Select(TileView pressedTile)
    {
        Tile tile = _controller.GetTileInCoords(pressedTile.x, pressedTile.y);

        _controller.ProcessTouchedTile(tile);
    }

    public async void ChangeTileVisual(Tile tile)
    {
        TileView tileToChange = tiles[tile.x, tile.y];

        await TileAnimations.DissapearAnimation(tileToChange);
        tileToChange.item = tile.item;
        tileToChange.icon.sprite = tileToChange.item.sprite;
        await TileAnimations.AppearAnimation(tileToChange);
    }
}
