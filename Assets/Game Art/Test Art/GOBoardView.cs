using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOBoardView : MonoBehaviour
{
    public int width, height;
    public GameObject tilePrefab;
    private Tile[,] allTiles;

    private void Start()
    {
        allTiles = new Tile[width, height];
        SetUp();
    }

    public void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);
                Instantiate(tilePrefab, tempPosition, Quaternion.identity);
            }
        }
    }
}
