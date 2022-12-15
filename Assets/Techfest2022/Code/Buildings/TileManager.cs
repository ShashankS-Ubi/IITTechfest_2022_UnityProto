using System;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    Floor,
    Production,
    Storage
}

[Serializable]
public struct BuildingData
{
    public ResourceType Resource;
    public Building BuildingObject;
}

/// <summary>
/// Manager class for tiles. This class will simply connect with the tiles and call their functions. 
/// The Tile class is supposed to handle function calls in depth.
/// </summary>
public class TileManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _floorTile = null;
    [SerializeField]
    private BuildingsMenuDataScriptableObject _buildingPrefabs = null;

    private Dictionary<ResourceType, Building> _storageBuildingArchetypes = new Dictionary<ResourceType, Building>();
    private Dictionary<ResourceType, Building> _productionBuildingArchetypes = new Dictionary<ResourceType, Building>();

    private Tile[] _tiles = null;

    public void Init()
    {
        if (_buildingPrefabs == null || _buildingPrefabs.BuildingDataList.Count == 0 || _floorTile == null)
        {
            gameObject.SetActive(false);
            enabled = false;
            return;
        }

        for (int i = 0; i < _buildingPrefabs.BuildingDataList.Count; i++)
        {
            if (_buildingPrefabs.BuildingDataList[i].BuildingCategory == BuildingType.Storage)
            {
                if (!_storageBuildingArchetypes.ContainsKey(_buildingPrefabs.BuildingDataList[i].Resource))
                {
                    _storageBuildingArchetypes.Add(_buildingPrefabs.BuildingDataList[i].Resource, _buildingPrefabs.BuildingDataList[i].Building.gameObject.GetComponent<Building>());
                }
            }
            else if (_buildingPrefabs.BuildingDataList[i].BuildingCategory == BuildingType.Production)
            {
                if (!_productionBuildingArchetypes.ContainsKey(_buildingPrefabs.BuildingDataList[i].Resource))
                {
                    _productionBuildingArchetypes.Add(_buildingPrefabs.BuildingDataList[i].Resource, _buildingPrefabs.BuildingDataList[i].Building.gameObject.GetComponent<Building>());
                }
            }
        }

        ///Fetches all the tiles available in the scene at the start of the game.
        _tiles = FindObjectsOfType<Tile>();
    }

    public void CreateBuilding(BuildingMenuData buildingMenuData, Tile tile)
    {
        if (buildingMenuData.BuildingCategory == BuildingType.Floor && tile.IsWater)
        {
            CreateFloor(tile);
            return;
        }

        ///Don't do anything if tile is not a floor. Every building is supposed to be on a floor only.
        if (tile.IsWater || !tile.IsFloor)
            return;

        IBuildingUpgradeData currData = GameDirector.Instance.UpgradeDataManager.GetUpgradeData(buildingMenuData.BuildingCategory, buildingMenuData.Resource, 0);

        if (currData == null)
        {
            Debug.LogError("Upgrade Data Missing");
            return;
        }

        ResourceQuantity cost = default;
        if (buildingMenuData.BuildingCategory == BuildingType.Storage)
        {
            cost = ((StorageBuildingUpgradeData)currData).BuildCost;
        }
        else if (buildingMenuData.BuildingCategory == BuildingType.Production)
        {
            cost = ((ProductionBuildingUpgradeData)currData).BuildCost;
        }
        if (!GameDirector.Instance.InventoryManager.EnoughResourcesAvailable(cost))
        {
            DebugPrintLog("Not Enough Resources", buildingMenuData.Resource, buildingMenuData.BuildingCategory);
            return;
        }

        GameDirector.Instance.InventoryManager.ConsumeResources(cost);
        PlaceBuilding(tile, buildingMenuData.BuildingCategory, buildingMenuData.Resource, currData);

        GameDirector.Instance.AudioManager.PlayAudio(AudioFileType.Create);
    }

    public void DestroyBuilding(Tile tile)
    {
        if(tile.CurrentBuilding != null)
        {
            tile.DestroyBuilding();

            GameDirector.Instance.AudioManager.PlayAudio(AudioFileType.Destroy);
        }
    }

    public void RepairBuilding(Tile tile)
    {
        if (tile == null || tile.CurrentBuilding == null)
        {
            return;
        }

        ResourceQuantity? cost = tile.CurrentBuilding.GetRepairCost();
        if (cost == null)
        {
            DebugPrintLog("No repair cost found. ", tile.CurrentBuilding);
            return;        
        }

        if (!GameDirector.Instance.InventoryManager.EnoughResourcesAvailable(cost.Value))
        {
            DebugPrintLog("Not Enough Resources", tile.CurrentBuilding);
            return;
        }
        if (tile.CurrentBuilding.CurrHealth == tile.CurrentBuilding.MaxHealth)
        {
            DebugPrintLog("Building Health Max Out", tile.CurrentBuilding);
            return;
        }
       

        GameDirector.Instance.InventoryManager.ConsumeResources(cost.Value);
        tile.RepairBuilding();

        GameDirector.Instance.AudioManager.PlayAudio(AudioFileType.Repair);
    }

    public void UpgradeBuilding(Tile tile)
    {
        if (tile == null || tile.CurrentBuilding == null)
        {
            return;
        }

        ResourceQuantity? cost = tile.CurrentBuilding.GetUpgradeCost();
        if (cost == null)
        {
            DebugPrintLog("No upgrade cost found. ", tile.CurrentBuilding);
            return;
        }

        if (!GameDirector.Instance.InventoryManager.EnoughResourcesAvailable(cost.Value))
        {
            DebugPrintLog("Not Enough Resources", tile.CurrentBuilding);
            return;
        }

        if (tile.CurrentBuilding.CurrLevel + 1 == GameDirector.Instance.UpgradeDataManager.GetMaxBuildingLevel(tile.CurrentBuilding.BuildingResourceType, tile.CurrentBuilding.BuildingType))
        {
            DebugPrintLog("Building Level Max Out", tile.CurrentBuilding);
            return;
        }

        GameDirector.Instance.InventoryManager.ConsumeResources(cost.Value);
        tile.UpgradeBuilding(GameDirector.Instance.UpgradeDataManager.GetUpgradeData(tile.CurrentBuilding.BuildingType, tile.CurrentBuilding.BuildingResourceType, tile.CurrentBuilding.CurrLevel + 1));
    }

    /// <summary>
    /// Damages a building on a given tile.
    /// </summary>
    /// <param name="tile">Target tile</param>
    /// <param name="damage">Amount of damage</param>
    public void DamageBuilding(Tile tile, int damage)
    {
        if (tile == null || tile.CurrentBuilding == null)
        {
            return;
        }
        tile.DamageBuilding(damage);

        // destroy only if the health value drops to or below zero
        if (tile.CurrentBuilding.CurrHealth <= 0)
            DestroyBuilding(tile);
    }

    /// <summary>
    /// Check for every tile in the game having a building and damage it.
    /// </summary>
    /// <param name="damage">Amount of damage</param>
    public void CalamityDamage(Tile[] tiles, int damage)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (_tiles[i] != null && tiles[i].CurrentBuilding != null)
            {
                DamageBuilding(tiles[i], damage);
            }
        }
    }

    public void SelectTile(Tile tile, bool toggle)
    {
        if(!tile.IsFloor && !tile.IsWater)
            tile.ShowSelectionUI(toggle);
    }

    private Building InstantiateBuilding(ResourceType resource, BuildingType category)
    {
        Building building = null;
        if (category == BuildingType.Production)
        {
            building = Instantiate(_productionBuildingArchetypes[resource]);
        }
        else if (category == BuildingType.Storage)
        {
            building = Instantiate(_storageBuildingArchetypes[resource]);
        }

        return building;
    }

    private void DebugPrintLog(string reason, ResourceType resource, BuildingType category)
    {
        Debug.Log($"{reason} : [CATEGORY : {category}] [TYPE : {resource}]");
    }

    private void DebugPrintLog(string reason, Building building)
    {
        Debug.Log($"{reason} : [Building : {building.name}]");
    }

    private void CreateFloor(Tile tile)
    {
        ResourceQuantity tileCost = GameDirector.Instance.UpgradeDataManager.GetFloorTileBuildCost();
        if (!GameDirector.Instance.InventoryManager.EnoughResourcesAvailable(tileCost))
        {
            DebugPrintLog("Not Enough Resources", tile.CurrentBuilding);
            return;
        }

        GameDirector.Instance.InventoryManager.ConsumeResources(tileCost);
        tile.MakeFloor(Instantiate(_floorTile));

        GameDirector.Instance.AudioManager.PlayAudio(AudioFileType.Ground);
    }


    /// <summary>
    /// Passes a new building instance of the requested category and resource type to the target tile, which in turn places it over itself.
    /// </summary>
    /// <param name="tile">Target tile</param>
    /// <param name="category">Requested building category</param>
    /// <param name="resource">Requested building resource type</param>
    /// <param name="currData">Upgrade data for the building</param>
    private void PlaceBuilding(Tile tile, BuildingType category, ResourceType resource, IBuildingUpgradeData currData)
    {
        Building building = InstantiateBuilding(resource, category);
        tile.CreateBuilding(building, currData);
    }

    public Tile[] GetAllTiles()
    {
        return _tiles;
    }
}
