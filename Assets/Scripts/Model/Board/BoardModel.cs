using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BoardModel
{
    public int Width;
    public int Height;

    public Tile[,] Board;

    public BoardModel(int width, int height)
    {
        this.Width = width;
        this.Height = height;

        Initialize();
    }

    public void Initialize()
    {
        Board = new Tile[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                ItemSO item = ItemsDatabase.Items[Random.Range(0, ItemsDatabase.Items.Length)];
                Tile tile = new Tile(x, y, item);

                Board[x, y] = tile;
            }
        }

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Board[x, y].SetAdjacents(this);
            }
        }
    }

    public Tile CreateTileInPosition(int x, int y, ItemSO item = null)
    {
        Board[x, y].item = item;

        if (item == null)
        {
            Board[x, y].item = ItemsDatabase.Items[Random.Range(0, ItemsDatabase.Items.Length)];
        }

        return Board[x, y];

    }

    public Tile GetTileInPosition(int x, int y)
    {
        return Board[x, y];
    }

    public Tile SetAdjacents(Tile tile)
    {
        if (tile.x - 1 >= 0)
        {
            tile.leftTile = GetTileInPosition(tile.x - 1, tile.y);
        }

        if (tile.x + 1 < Width)
        {
            tile.rightTile = GetTileInPosition(tile.x + 1, tile.y);
        }

        if (tile.y - 1 >= 0)
        {
            tile.topTile = GetTileInPosition(tile.x, tile.y - 1);
        }

        if (tile.y + 1 < Height)
        {
            tile.bottomTile = GetTileInPosition(tile.x, tile.y + 1);
        }

        return tile;
    }
}
