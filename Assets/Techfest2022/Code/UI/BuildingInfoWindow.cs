using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buildingName;
    [SerializeField] private TextMeshProUGUI _buildingLevel;
    [SerializeField] private TextMeshProUGUI _buildingHealth;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _repairButton;
    [SerializeField] private TextMeshProUGUI _repairCost;
    [SerializeField] private TextMeshProUGUI _upgradeCost;
    [SerializeField] private TextMeshProUGUI _nextLevelUpdateRequired;
    [SerializeField] private TextMeshProUGUI _nextHealthUpdateRequired;


    private Tile _data;
    private ResourceQuantity? _upgradeResourceQuantity;
    private ResourceQuantity? _reapirResourceQuantity;
    private int _maxBuildingLevel;

    public static Action BuildingInfoUIUpdate;
    public void SetContext(Tile tile)
    {
        _data = tile;
        _maxBuildingLevel = GameDirector.Instance.UpgradeDataManager.GetMaxBuildingLevel(_data.CurrentBuilding.BuildingResourceType, _data.CurrentBuilding.BuildingType);
        RefreshUI();
    }

    void RefreshUI()
    {
        _buildingName.text = string.Format($"{_data.CurrentBuilding.name}");
        _buildingLevel.text = string.Format($"Level - {_data.CurrentBuilding.CurrLevel + 1}/{_maxBuildingLevel}");
        _buildingHealth.text = string.Format($"Health - {_data.CurrentBuilding.CurrHealth}/{_data.CurrentBuilding.MaxHealth}");
        UpdatePrice();
        ResourceQuantity repairResources;
        ResourceQuantity upgradeResources;
        GameDirector.Instance.UpgradeDataManager.GetUpgradeResourceQuantity(_data.CurrentBuilding.BuildingType, _data.CurrentBuilding.BuildingResourceType, _data.CurrentBuilding.CurrLevel, out repairResources, out upgradeResources);

        string nextLevelRequired = _maxBuildingLevel == _data.CurrentBuilding.CurrLevel + 1 ? "Max Level Reached!" :
            string.Format($"Upgrade Requirements: Water {upgradeResources.Water}, Food {upgradeResources.Food}, Metal {upgradeResources.Metal}, Energy {upgradeResources.Energy}");

        string nextHealthRequired = _data.CurrentBuilding.MaxHealth == _data.CurrentBuilding.CurrHealth ? "Max Health Reached!" :
            string.Format($"Repair Requirements:  Water {repairResources.Water}, Food {repairResources.Food}, Metal {repairResources.Metal}, Energy {repairResources.Energy}");

        _nextHealthUpdateRequired.text = nextHealthRequired;
        _nextLevelUpdateRequired.text = nextLevelRequired;
    }

    private void UpdatePrice()
    {
        _upgradeResourceQuantity = _data.CurrentBuilding.GetUpgradeCost();
        _reapirResourceQuantity = _data.CurrentBuilding.GetRepairCost();
        if (_upgradeResourceQuantity.HasValue)
        {
            _upgradeCost.text = string.Format($"Upgrade");
        }
        if (_reapirResourceQuantity.HasValue)
        {
            _repairCost.text = string.Format($"Repair");
        }
    }

    public void UpgradeBuilding()
    {
        if (_data != null)
        {
            GameDirector.Instance.GameTileManager.UpgradeBuilding(_data);
            CheckAndRefresh(_upgradeButton, _upgradeResourceQuantity);
        }
    }

    public void RepairBuilding()
    {
        if (_data != null)
        {
            GameDirector.Instance.GameTileManager.RepairBuilding(_data);
            CheckAndRefresh(_upgradeButton, _reapirResourceQuantity);
        }
    }

    void CheckAndRefresh(Button button, ResourceQuantity? cost)
    {
        UpdatePrice();
        //BuildingInfoUIUpdate?.Invoke();
        RefreshUI();
    }
}
public class BuildingInfoData
{
    public Tile Tile;

    public BuildingInfoData(Tile tile)
    {
        Tile = tile;
    }
}
