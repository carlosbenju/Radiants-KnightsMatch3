using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileView : MonoBehaviour
{
    public int x, y;

    public Image icon;
    public ItemSO item;
    public Button button;
    public bool isEmpty;
    public List<TileView> neightbours;
    public List<TileView> allAdjacents;

    public TileView leftTile;
    public TileView rightTile;
    public TileView topTile;
    public TileView bottomTile;

    public void SetAdjacents(TileView[,] tiles, int width, int height)
    {
        if (x - 1 >= 0)
        {
            leftTile = tiles[x - 1, y];
            neightbours.Add(leftTile);
            allAdjacents.Add(leftTile);

            if (y - 1 >= 0)
            {
                allAdjacents.Add(tiles[x - 1, y - 1]);
            }

            if (y + 1 < height)
            {
                allAdjacents.Add(tiles[x - 1, y + 1]);
            }
        }

        if (x + 1 < width)
        {
            rightTile = tiles[x + 1, y];
            neightbours.Add(rightTile);
            allAdjacents.Add(rightTile);

            if (y - 1 >= 0)
            {
                allAdjacents.Add(tiles[x + 1, y - 1]);
            }

            if (y + 1 < height)
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

            if (x + 1 < height)
            {
                allAdjacents.Add(tiles[x + 1, y - 1]);
            }
        }

        if (y + 1 < height)
        {
            bottomTile = tiles[x, y + 1];
            neightbours.Add(bottomTile);
            allAdjacents.Add(bottomTile);

            if (x - 1 >= 0)
            {
                allAdjacents.Add(tiles[x - 1, y + 1]);
            }

            if (x + 1 < width)
            {
                allAdjacents.Add(tiles[x + 1, y + 1]);
            }
        }
    }

    public List<TileView> GetConnectedTiles(List<TileView> exclude = null)
    {
        List<TileView> result = new List<TileView> { this, };

        if (exclude == null)
        {
            exclude = new List<TileView> { this, };
        }
        else
        {
            exclude.Add(this);
        }

        foreach (TileView neighbour in neightbours)
        {
            if (neighbour == null || exclude.Contains(neighbour) || neighbour.item != item) continue;

            result.AddRange(neighbour.GetConnectedTiles(exclude));
        }

        return result;
    }
}
