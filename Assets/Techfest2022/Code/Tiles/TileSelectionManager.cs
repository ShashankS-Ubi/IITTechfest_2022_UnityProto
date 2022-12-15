using UnityEngine;

public class TileSelectionManager : MonoBehaviour
{
    //private List<Tile> _currentSelectedTiles = null; //For future.
    private Tile _currentSelectedTile = null;
    public void SelectTile(Tile tile)
    {
        if (_currentSelectedTile != null)
        {
            GameDirector.Instance.GameTileManager.SelectTile(_currentSelectedTile, false);
        }

        _currentSelectedTile = tile;
        GameDirector.Instance.GameTileManager.SelectTile(tile, true);
    }
}
