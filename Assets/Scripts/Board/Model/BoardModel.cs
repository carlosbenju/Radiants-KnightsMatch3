using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BoardModel
{
    public Dictionary<Point, Tile> tiles;
    public int width;
    public int height;

    public Tile[,] board;

    public BoardModel(int width, int height)
    {
        this.width = width;
        this.height = height;

        Initialize();
    }

    public void Initialize()
    {
        board = new Tile[width, height];
        tiles = new Dictionary<Point, Tile>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                ItemSO item = ItemsDatabase.Items[Random.Range(0, ItemsDatabase.Items.Length)];
                Tile tile = new Tile(x, y, item);

                tiles[new Point(x, y)] = tile;
                board[x, y] = tile;
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[new Point(x, y)] = SetAdjacents(tiles[new Point(x, y)]);
                board[x, y].SetAdjacents(this);
            }
        }
    }

    public Tile CreateNewItemInPosition(int x, int y, ItemSO item = null)
    {
        board[x, y].item = item;

        if (item == null)
        {
            board[x, y].item = ItemsDatabase.Items[Random.Range(0, ItemsDatabase.Items.Length)];
        }

        return board[x, y];

    }

    public Tile GetCoordsValue(int x, int y)
    {
        Point p = new Point(x, y);
        return tiles[p];
    }

    public void SwapCoordValues(Point point1, Point point2)
    {
        Tile tile1Temp = GetCoordsValue(point1.x, point1.y);

        tiles[point1] = tiles[point2];
        tiles[point2] = tile1Temp;
    }

    public Tile SetAdjacents(Tile tile)
    {
        if (tile.x - 1 >= 0)
        {
            tile.leftTile = GetCoordsValue(tile.x - 1, tile.y);
        }

        if (tile.x + 1 < width)
        {
            tile.rightTile = GetCoordsValue(tile.x + 1, tile.y);
        }

        if (tile.y - 1 >= 0)
        {
            tile.topTile = GetCoordsValue(tile.x, tile.y - 1);
        }

        if (tile.y + 1 < height)
        {
            tile.bottomTile = GetCoordsValue(tile.x, tile.y + 1);
        }

        return tile;
    }
}
