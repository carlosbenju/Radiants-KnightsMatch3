using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class BoardController
{
    public BoardModel Model;

    int _width;
    int _height;

    public event Action<float, TileType> OnTileDestroyed = delegate(float quantity, TileType type) { };
    public event Action<Tile> OnTileChanged = delegate(Tile tile) { };

    public BoardController(BoardModel model)
    {
        Model = model;
        _width = model.width;
        _height = model.height;
    }

    public void ProcessTouchedTile(Tile tile)
    {
        List<Tile> matchesOnPressedTile = FindMatchesOnPressedTile(tile);

        if (tile.item.boosterType == BoosterType.Bomb)
        {
            BombAction(tile);
        }

        if (tile.item.boosterType == BoosterType.ColorBomb)
        {
            ColorBombAction(tile);
        }

        if (matchesOnPressedTile.Count > 2)
        {
            Match3Action(tile, matchesOnPressedTile);
        }
    }

    private void Match3Action(Tile tile, List<Tile> matchesOnPressedTile)
    {
        OnTileDestroyed(matchesOnPressedTile.Count, tile.item.type);
        CreateNewTiles(tile, matchesOnPressedTile);
    }

    private void BombAction(Tile tile)
    {
        List<Tile> tileAdjacents = tile.allAdjacents;
        tileAdjacents.Add(tile);

        OnTileDestroyed(tileAdjacents.Count, tile.item.type);
        foreach(Tile t in tileAdjacents)
        {
            CreateNormalTile(t);
        }
    }
    private void ColorBombAction(Tile tile)
    {
        List<Tile> allTilesOfType = GetAllTilesOfType(tile.item.type);
        allTilesOfType.Add(tile);

        OnTileDestroyed(allTilesOfType.Count, tile.item.type);
        foreach (Tile t in allTilesOfType)
        {
            CreateNormalTile(t);
        }
    }

    public Tile GetTileInCoords(int x, int y)
    {
        return Model.GetCoordsValue(x, y);
    }

    public List<Tile> FindMatchesOnPressedTile(Tile tile)
    {
        List<Tile> tilesConnected = GetTileInCoords(tile.x, tile.y).GetConnectedTiles();

        return tilesConnected;
    }

    public void CreateNewTiles(Tile pressedTile, List<Tile> matchedTiles)
    {
        foreach (Tile tile in matchedTiles)
        {
            if (pressedTile.item.boosterType == BoosterType.Bomb 
                || pressedTile.item.boosterType == BoosterType.ColorBomb || matchedTiles.Count == 3)
            {
                CreateNormalTile(tile);
                continue;
            }

            if (matchedTiles.Count > 3 && matchedTiles.Count < 6 && tile == pressedTile)
            {
                CreateBomb(tile);
                continue;
            }

            if (matchedTiles.Count >= 6 && tile == pressedTile)
            {
                CreateColorBomb(tile);
                continue;
            }
        }
    }

    private void CreateNormalTile(Tile tile)
    {
        tile.item = ItemsDatabase.Items[UnityEngine.Random.Range(0, ItemsDatabase.Items.Length)];
        tile = Model.CreateNewItemInPosition(tile.x, tile.y, tile.item);

        OnTileChanged?.Invoke(tile);
    }

    public void CreateBomb(Tile tile = null)
    {
        ItemSO bomb = null;
        foreach (ItemSO item in BoostersDatabase.Boosters)
        {
            if (item.boosterType == BoosterType.Bomb)
            {
                bomb = item;
            }
        }

        if (tile == null)
        {
            int randomX = UnityEngine.Random.Range(0, _width - 1);
            int randomY = UnityEngine.Random.Range(0, _height - 1);

            tile = Model.CreateNewItemInPosition(randomX, randomY, bomb);
            OnTileChanged?.Invoke(tile);
            return;
        }

        tile = Model.CreateNewItemInPosition(tile.x, tile.y, bomb);
        OnTileChanged?.Invoke(tile);
    }

    public void CreateColorBomb(Tile pressedTile = null)
    {
        ItemSO colorBomb = null;
        foreach (ItemSO item in BoostersDatabase.Boosters)
        {
            if (item.boosterType == BoosterType.ColorBomb)
            {
                colorBomb = item;
            }
        }

        if (pressedTile == null)
        {
            colorBomb.type = TileType.Red;

            int randomX = UnityEngine.Random.Range(0, _width - 1);
            int randomY = UnityEngine.Random.Range(0, _height - 1);

            pressedTile = Model.CreateNewItemInPosition(randomX, randomY, colorBomb);
            OnTileChanged?.Invoke(pressedTile);
            return;
        }

        colorBomb.type = pressedTile.item.type;
        pressedTile = Model.CreateNewItemInPosition(pressedTile.x, pressedTile.y, colorBomb);
        OnTileChanged?.Invoke(pressedTile);
    }

    public TileView AssignTileItem(TileView tile)
    {
        Tile tileModel = GetTileInCoords(tile.x, tile.y);
        tile.item = tileModel.item;
        tile.icon.sprite = tile.item.sprite;

        return tile;
    }

    private List<Tile> GetAllTilesOfType(TileType type)
    {
        List<Tile> tilesOfType = new List<Tile>();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (Model.board[x, y].item.type == type)
                {
                    tilesOfType.Add(Model.board[x, y]);
                }
            }
        }

        return tilesOfType;
    }
}
