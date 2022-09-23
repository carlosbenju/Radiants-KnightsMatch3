using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile
{
    public int x, y;
    public ItemSO item;

    public List<Tile> neightbours;
    public List<Tile> allAdjacents;

    public Tile leftTile;
    public Tile rightTile;
    public Tile topTile;
    public Tile bottomTile;

    public bool IsEmpty;

    public Tile(int x, int y, ItemSO item)
    {
        this.x = x;
        this.y = y;
        this.item = item;
    }

    public void SetAdjacents(BoardModel gridModel)
    {
        Tile[,] tiles = gridModel.board;
        neightbours = new List<Tile>();
        allAdjacents = new List<Tile>();

        if (x - 1 >= 0)
        {
            leftTile = tiles[x - 1, y];
            neightbours.Add(leftTile);
            allAdjacents.Add(leftTile);

            if (y - 1 >= 0)
            {
                allAdjacents.Add(tiles[x - 1, y - 1]);
            }

            if (y + 1 < gridModel.height)
            {
                allAdjacents.Add(tiles[x - 1, y + 1]);
            }
        }

        if (x + 1 < gridModel.width)
        {
            rightTile = tiles[x + 1, y];
            neightbours.Add(rightTile);
            allAdjacents.Add(rightTile);

            if (y - 1 >= 0)
            {
                allAdjacents.Add(tiles[x + 1, y - 1]);
            }

            if (y + 1 < gridModel.height)
            {
                allAdjacents.Add(tiles[x + 1, y + 1]);
            }
        }

        if (y - 1 >= 0)
        {
            topTile = tiles[x, y - 1];
            neightbours.Add(topTile);
            allAdjacents.Add(topTile);

            if (x - 1 >= 0)
            {
                allAdjacents.Add(tiles[x - 1, y - 1]);
            }

            if (x + 1 < gridModel.height)
            {
                allAdjacents.Add(tiles[x + 1, y - 1]);
            }
        }

        if (y + 1 < gridModel.height)
        {
            bottomTile = tiles[x, y + 1];
            neightbours.Add(bottomTile);
            allAdjacents.Add(bottomTile);

            if (x - 1 >= 0)
            {
                allAdjacents.Add(tiles[x - 1, y + 1]);
            }

            if (x + 1 < gridModel.width)
            {
                allAdjacents.Add(tiles[x + 1, y + 1]);
            }
        }
    }

    public List<Tile> GetConnectedTiles(List<Tile> exclude = null)
    {
        List<Tile> result = new List<Tile> { this, };

        if (exclude == null)
        {
            exclude = new List<Tile> { this, };
        }
        else
        {
            exclude.Add(this);
        }

        foreach (Tile neighbour in neightbours)
        {
            if (neighbour == null || exclude.Contains(neighbour) || neighbour.item != item) continue;

            result.AddRange(neighbour.GetConnectedTiles(exclude));
        }

        return result;
    }
}
