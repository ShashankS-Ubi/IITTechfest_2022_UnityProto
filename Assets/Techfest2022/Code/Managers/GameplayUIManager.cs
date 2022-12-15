using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager class for all the UI interactions in the game.
/// </summary>
public class GameplayUIManager : MonoBehaviour
{
    [SerializeField]
    private BuildingInfoManager _buildingInfoManager;
    [SerializeField]
    private TopBarComponent _topBar;

    public Action<BuildingMenuData, Tile> BuildingCreationRequested;

    public void Init()
    {
        BuildingButtonItem.BuildingPlaced += OnBuildingPlaced;

        Tile.BuildingClick += OnBuildingClicked;
    }

    private void OnBuildingClicked(Tile obj)
    {
        _buildingInfoManager.SetTile(obj);
    }

    private void OnDestroy()
    {
        BuildingButtonItem.BuildingPlaced -= OnBuildingPlaced;
        Tile.BuildingClick -= OnBuildingClicked;
    }

    private void OnBuildingPlaced(BuildingMenuData buildingMenuData, Tile tile)
    {
        BuildingCreationRequested?.Invoke(buildingMenuData, tile);
    }

    public void RefreshTopBarUI()
    {
        _topBar.RefreshUI();
    }
}
