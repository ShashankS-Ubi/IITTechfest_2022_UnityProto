using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct UpgradeData
{
    public ResourceType ResourceType;
    public StorageBuildingUpgradeDataScriptableObject StorageBuildingUpgradeData;
    public ProductionBuildingUpgradeDataScriptableObject ProductionBuildingUpgradeData;
}

public interface IBuildingUpgradeData{ }

[Serializable]
public struct StorageBuildingUpgradeData : IBuildingUpgradeData
{
    public int MaxStorage;

    [Header("General Data")]
    public int MaxHealth;
    public ResourceQuantity BuildCost;
    public ResourceQuantity RepairCost;
    public ResourceQuantity UpgradeCost;
    public Mesh UpgradedBuildingMesh;
}

[Serializable]
public struct ProductionBuildingUpgradeData : IBuildingUpgradeData
{
    public int ProductionRate;

    [Header("General Data")]
    public int MaxHealth;
    public ResourceQuantity BuildCost;
    public ResourceQuantity RepairCost;
    public ResourceQuantity UpgradeCost;
    public Mesh UpgradedBuildingMesh;
}

public class BuildingUpgradeDataManager : MonoBehaviour
{
    [SerializeField]
    private List<UpgradeData> _upgradeData = new List<UpgradeData>();
    [SerializeField]
    private ResourceQuantity _floorTileBuildCost;

    public void Init()
    {
        
    }

    private StorageBuildingUpgradeData? GetStorageUpgradeData(ResourceType type,int level)
    {
        UpgradeData data = _upgradeData.Find(data => data.ResourceType == type);
        return data.StorageBuildingUpgradeData.UpgradeData[level];
    }

    private ProductionBuildingUpgradeData? GetProductionUpgradeData(ResourceType type, int level)
    {
        UpgradeData data = _upgradeData.Find(data => data.ResourceType == type);
        return data.ProductionBuildingUpgradeData.UpgradeData[level];
    }

    public IBuildingUpgradeData GetUpgradeData(BuildingType category, ResourceType type, int level)
    {
        if (category == BuildingType.Production)
        {
            return GetProductionUpgradeData(type, level);
        }
        else if (category == BuildingType.Storage)
        {
            return GetStorageUpgradeData(type, level);
        }

        return null;
    }

    public ResourceQuantity GetFloorTileBuildCost()
    {
        return _floorTileBuildCost;
    }

    public int GetMaxBuildingLevel(ResourceType type, BuildingType category)
    {
        int level = 0;
        UpgradeData data = _upgradeData.Find(data => data.ResourceType == type);
        if (category == BuildingType.Production)
        {
            level = data.ProductionBuildingUpgradeData.UpgradeData.Count;
        }
        else if (category == BuildingType.Storage)
        {
            level = data.StorageBuildingUpgradeData.UpgradeData.Count;
        }
        return level;
    }

    public void GetUpgradeResourceQuantity(BuildingType category, ResourceType type, int level, out ResourceQuantity repairResource, out ResourceQuantity upgradeResource)
    {
        repairResource = default;
        upgradeResource = default;
        if (category == BuildingType.Production)
        {
             var data = GetProductionUpgradeData(type, level);
            repairResource = data.Value.RepairCost;
            upgradeResource = data.Value.UpgradeCost;
        }
        else if (category == BuildingType.Storage)
        {
            var data = GetStorageUpgradeData(type, level);
            repairResource = data.Value.RepairCost;
            upgradeResource = data.Value.UpgradeCost;
        }
    }
}
