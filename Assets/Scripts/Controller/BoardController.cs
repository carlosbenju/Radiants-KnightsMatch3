using System;
using System.Collections.Generic;

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
        _width = model.Width;
        _height = model.Height;
    }

    public void ProcessTouchedTile(Tile tile)
    {
        List<Tile> matchesOnPressedTile = FindMatchesOnPressedTile(tile);

        if (tile.item.BoosterType == BoosterType.Bomb)
        {
            BombAction(tile);
        }

        if (tile.item.BoosterType == BoosterType.ColorBomb)
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
        OnTileDestroyed(matchesOnPressedTile.Count, tile.item.Type);
        CreateNewTiles(tile, matchesOnPressedTile);
    }

    private void BombAction(Tile tile)
    {
        List<Tile> tileAdjacents = tile.allAdjacents;
        tileAdjacents.Add(tile);

        OnTileDestroyed(tileAdjacents.Count, tile.item.Type);
        foreach(Tile t in tileAdjacents)
        {
            CreateNormalTile(t);
        }
    }

    private void ColorBombAction(Tile tile)
    {
        List<Tile> allTilesOfType = GetAllTilesOfType(tile.item.Type);
        allTilesOfType.Add(tile);

        OnTileDestroyed(allTilesOfType.Count, tile.item.Type);
        foreach (Tile t in allTilesOfType)
        {
            CreateNormalTile(t);
        }
    }

    public Tile GetTileInCoords(int x, int y)
    {
        return Model.GetTileInPosition(x, y);
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
            if (pressedTile.item.BoosterType == BoosterType.Bomb 
                || pressedTile.item.BoosterType == BoosterType.ColorBomb || matchedTiles.Count == 3)
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
        tile = Model.CreateTileInPosition(tile.x, tile.y, tile.item);

        OnTileChanged?.Invoke(tile);
    }

    public void CreateBomb(Tile tile = null)
    {
        ItemSO bomb = null;
        foreach (ItemSO item in BoostersDatabase.Boosters)
        {
            if (item.BoosterType == BoosterType.Bomb)
            {
                bomb = item;
            }
        }

        if (tile == null)
        {
            int randomX = UnityEngine.Random.Range(0, _width - 1);
            int randomY = UnityEngine.Random.Range(0, _height - 1);

            tile = Model.CreateTileInPosition(randomX, randomY, bomb);
            OnTileChanged?.Invoke(tile);
            return;
        }

        tile = Model.CreateTileInPosition(tile.x, tile.y, bomb);
        OnTileChanged?.Invoke(tile);
    }

    public void CreateColorBomb(Tile pressedTile = null)
    {
        ItemSO colorBomb = null;
        foreach (ItemSO item in BoostersDatabase.Boosters)
        {
            if (item.BoosterType == BoosterType.ColorBomb)
            {
                colorBomb = item;
            }
        }

        if (pressedTile == null)
        {
            colorBomb.Type = TileType.Red;

            int randomX = UnityEngine.Random.Range(0, _width - 1);
            int randomY = UnityEngine.Random.Range(0, _height - 1);

            pressedTile = Model.CreateTileInPosition(randomX, randomY, colorBomb);
            OnTileChanged?.Invoke(pressedTile);
            return;
        }

        colorBomb.Type = pressedTile.item.Type;
        pressedTile = Model.CreateTileInPosition(pressedTile.x, pressedTile.y, colorBomb);
        OnTileChanged?.Invoke(pressedTile);
    }

    private List<Tile> GetAllTilesOfType(TileType type)
    {
        List<Tile> tilesOfType = new List<Tile>();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (Model.Board[x, y].item.Type == type)
                {
                    tilesOfType.Add(Model.Board[x, y]);
                }
            }
        }

        return tilesOfType;
    }
}
