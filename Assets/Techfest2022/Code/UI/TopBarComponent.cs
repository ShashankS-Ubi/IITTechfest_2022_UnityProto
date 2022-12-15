using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TopBarComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _waterText;
    [SerializeField] private TextMeshProUGUI _foodText;
    [SerializeField] private TextMeshProUGUI _metalText;
    [SerializeField] private TextMeshProUGUI _energyText;
    [SerializeField] private TextMeshProUGUI _calamityText;

    InventoryManager _inventoryManager;

    private void OnEnable()
    {
        BuildingInfoWindow.BuildingInfoUIUpdate += OnBuildingInfoUIUpdate;
        InventoryManager.ResourcesStatsChanged += OnInventoryUpdate;
        VFXManager.VfxToggled += OnVfxToggled;
    }

    private void OnVfxToggled(string calamityType)
    {
        _calamityText.text = calamityType;
    }

    private void OnInventoryUpdate()
    {
        RefreshUI();
    }

    private void OnBuildingInfoUIUpdate()
    {
        RefreshUI();
    }

    void Start()
    {
        _inventoryManager = GameDirector.Instance.InventoryManager;
        RefreshUI();

        _calamityText.text = string.Empty;
    }

    public void RefreshUI()
    {
        _waterText.text = string.Format($"Water {_inventoryManager.GetAvailableResource().Water}/{_inventoryManager.GetMaxResourceCapacity().Water}");
        _foodText.text = string.Format($"Food {_inventoryManager.GetAvailableResource().Food}/{_inventoryManager.GetMaxResourceCapacity().Food}");
        _metalText.text = string.Format($"Metal {_inventoryManager.GetAvailableResource().Metal}/{_inventoryManager.GetMaxResourceCapacity().Metal}");
        _energyText.text = string.Format($"Energy {_inventoryManager.GetAvailableResource().Energy}/{_inventoryManager.GetMaxResourceCapacity().Energy}");
    }
    private void OnDisable()
    {
        BuildingInfoWindow.BuildingInfoUIUpdate -= OnBuildingInfoUIUpdate;
        InventoryManager.ResourcesStatsChanged -= OnInventoryUpdate;
    }
}
