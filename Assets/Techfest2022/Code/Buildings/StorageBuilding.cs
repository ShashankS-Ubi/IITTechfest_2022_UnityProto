using UnityEngine;

public class StorageBuilding : Building
{
    private StorageBuildingUpgradeData? _currData;
    private bool _addedToInventory = false;

    public override void CreateBuilding(IBuildingUpgradeData upgradeData)
    {
        _buildingType = BuildingType.Storage;
        _currData = upgradeData as StorageBuildingUpgradeData?;
        if (_currData == null || !_currData.HasValue)
            return;

        GameDirector.Instance.InventoryManager.ModifyCapacity(BuildingResourceType, _currData.Value.MaxStorage);
        SetMaxHealth(_currData.Value.MaxHealth);
        SetHealth(_currData.Value.MaxHealth);
        _addedToInventory = true;

        //DebugPrintLog("Create Successful");
    }

    public override void UpgradeBuilding(IBuildingUpgradeData upgradeData)
    {
        StorageBuildingUpgradeData? nextData = upgradeData as StorageBuildingUpgradeData?;
        if (nextData == null || !nextData.HasValue)
            return;

        GameDirector.Instance.InventoryManager.ModifyCapacity(BuildingResourceType, nextData.Value.MaxStorage - _currData.Value.MaxStorage);
        SetMaxHealth(nextData.Value.MaxHealth);
        SetHealth(nextData.Value.MaxHealth);
        _currData = nextData;
        IncreaseLevel();

        if (nextData.Value.UpgradedBuildingMesh != null)
        {
            ChangeBuildingMesh(nextData.Value.UpgradedBuildingMesh);
        }
    }

    public override void RepairBuilding()
    {
        if (!_currData.HasValue)
        {
            Debug.LogError("Repair Data Missing");
            return;
        }

        SetHealth(_currData.Value.MaxHealth);
    }

    public override void DestroyBuilding()
    {
        if (_currData.HasValue && _addedToInventory)
        {
            GameDirector.Instance.InventoryManager.ModifyCapacity(BuildingResourceType, -1 * _currData.Value.MaxStorage);
            GameDirector.Instance.InventoryManager.LimitAvailableCapacity(BuildingResourceType);
        }

        _addedToInventory = false;
        base.DestroyBuilding();
    }

    public override ResourceQuantity? GetUpgradeCost()
    {
        if (!_currData.HasValue)
        {
            return null;
        }

        return _currData.Value.UpgradeCost;
    }

    public override ResourceQuantity? GetRepairCost()
    {
        if (!_currData.HasValue)
        {
            return null;
        }

        return _currData.Value.RepairCost;
    }
}
