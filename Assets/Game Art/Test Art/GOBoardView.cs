using System;
using System.Collections.Generic;
using UnityEngine;

public class GOBoardView : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] GOTileView tilePrefab;
    [SerializeField] GameObject[] tileSpawners;

    GOTileView[,] _allTiles;
    BoardController _controller;
    BoardModel _model;

    Action<Tile> _onTileClicked;

    private void Start()
    {
        _model = new BoardModel(width, height);
        _controller = new BoardController(_model);
        _allTiles = new GOTileView[width, height];

        _controller.OnTileChanged += DestroyTile;

        Initialize();
    }

    private void OnDestroy()
    {
        _controller.OnTileChanged -= DestroyTile;
    }

    public void Initialize()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);
                GOTileView tileView = Instantiate(tilePrefab, tempPosition, Quaternion.identity, transform);
                tileView.Initialize(_model.board[i, j], ProcessPlayerInput);
                _allTiles[i, j] = tileView;
            }
        }
    }

    void ProcessPlayerInput(Tile pressedTile)
    {
        _controller.ProcessTouchedTile(pressedTile);
    }

    public async void DestroyTile(Tile tile)
    {
        GOTileView tileToChange = _allTiles[tile.x, tile.y];
        _allTiles[tile.x, tile.y] = null;
        await TileAnimations.DissapearAnimation(tileToChange);
        tileToChange.Destroy();
    }
}
