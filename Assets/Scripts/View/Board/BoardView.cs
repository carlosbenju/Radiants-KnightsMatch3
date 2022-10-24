using UnityEngine;

public class BoardView : MonoBehaviour
{
    public TileView[,] Tiles;
    public Row[] Rows;

    BoardController _controller;
    BoardModel _board;

    int _width;
    int _height;

    public void Initialize(BoardController controller)
    {
        _controller = controller;
        _board = _controller.Model;

        _width = _board.Width;
        _height = _board.Height;

        Tiles = new TileView[_width, _height];

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
                TileView tileView = Rows[y].TileViews[x];
                tileView.x = x;
                tileView.y = y;
                tileView.item = _controller.GetTileInCoords(x, y).item;
                tileView.icon.sprite = tileView.item.Sprite;
                Tiles[x, y] = tileView;
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
        TileView tileToChange = Tiles[tile.x, tile.y];

        await TileAnimations.DissapearAnimation(tileToChange);
        tileToChange.item = tile.item;
        tileToChange.icon.sprite = tileToChange.item.Sprite;
        await TileAnimations.AppearAnimation(tileToChange);
    }
}
