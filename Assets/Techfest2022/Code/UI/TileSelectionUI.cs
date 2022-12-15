using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Selection UI view class on a tile.
/// </summary>
[RequireComponent(typeof(Tile))]
public class TileSelectionUI : MonoBehaviour
{
    [SerializeField]
    private Renderer _selectorUI = null;

    public void ToggleSelection(bool show)
    {
        if (_selectorUI == null)
            return;

        _selectorUI.enabled = show;
    }
}
