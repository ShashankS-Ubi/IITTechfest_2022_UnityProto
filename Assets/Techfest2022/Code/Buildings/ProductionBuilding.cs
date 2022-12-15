using UnityEngine;

public class ProductionBuilding : Building
{

    private ProductionBuildingUpgradeData? _currData;

    private int _productionRate;

    private bool _produce = false;
    private float _time = 0.0f;
    private const float WAIT_FOR_TIME = 1.0f;

    public override void CreateBuilding(IBuildingUpgradeData upgradeData)
    {
        _buildingType = BuildingType.Production;
        _currData = upgradeData as ProductionBuildingUpgradeData?;
        if (_currData == null || !_currData.HasValue)
            return;

        _productionRate = _currData.Value.ProductionRate;
        SetMaxHealth(_currData.Value.MaxHealth);
        SetHealth(_currData.Value.MaxHealth);

        ProduceTimer(true);
    }

    public override void UpgradeBuilding(IBuildingUpgradeData upgradeData)
    {
        _currData = upgradeData as ProductionBuildingUpgradeData?;
        if (_currData == null || !_currData.HasValue)
            return;

        ProduceTimer(false);
        _productionRate = _currData.Value.ProductionRate;
        SetMaxHealth(_currData.Value.MaxHealth);
        SetHealth(_currData.Value.MaxHealth);
        IncreaseLevel();
        ProduceTimer(true);

        if (_currData.Value.UpgradedBuildingMesh != null)
        {
            ChangeBuildingMesh(_currData.Value.UpgradedBuildingMesh);
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

    private void ProduceResources()
    {
        if (GameDirector.Instance.InventoryManager.IsResourceSpaceAvailable(BuildingResourceType, out int spaceAvailable))
        {
            GameDirector.Instance.InventoryManager.AddResources(BuildingResourceType, Mathf.Min(_productionRate, spaceAvailable));
        }
    }

    private void Update()
    {
        // produce the resources at a specified rate using a timer.
        if (!_produce)
            return;

        _time += Time.deltaTime;

        if (_time >= WAIT_FOR_TIME)
        {
            ProduceResources();
            _time = 0.0f;
        }
    }

    // resets the production timer.
    private void ProduceTimer(bool Enable)
    {
        _produce = Enable;
        _time = 0.0f;
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
