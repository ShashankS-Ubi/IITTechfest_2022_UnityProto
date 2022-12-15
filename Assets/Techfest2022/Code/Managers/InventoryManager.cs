using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private InventoryStarterValuesScriptableObject _starterValues = null;

    private ResourceQuantity _available;
    private ResourceQuantity _capacity;

    public static Action ResourcesStatsChanged;

    public void Init()
    {
        _available = _starterValues.InitialAvailableResources;
        _capacity = _starterValues.InitialCapacity;
    }

    // to check if enough resources are available to build, repair or upgrade.
    public bool EnoughResourcesAvailable(ResourceQuantity cost)
    {
        return (cost.Water <= _available.Water &&
                cost.Metal <= _available.Metal &&
                cost.Food <= _available.Food &&
                cost.Energy <= _available.Energy);
    }

    // Use resources to build, repair or upgrade after checking the availability
    public void ConsumeResources(ResourceQuantity cost)
    {
        _available.Water -= cost.Water;
        _available.Metal -= cost.Metal;
        _available.Food -= cost.Food;
        _available.Energy -= cost.Energy;

        ResourcesStatsChanged?.Invoke();
    }

    // Modify total capacity when a storage building is created or destroyed.
    public void ModifyCapacity(ResourceType type, int quantity)
    {
        switch(type)
        {
            case ResourceType.Water:
                _capacity.Water += quantity;
                break;
            case ResourceType.Metal:
                _capacity.Metal += quantity;
                break;
            case ResourceType.Food:
                _capacity.Food += quantity;
                break;
            case ResourceType.Energy:
                _capacity.Energy += quantity;
                break;
        }

        ResourcesStatsChanged?.Invoke();
    }

    /// <summary>
    /// to remove additional resources if the quantity is greater than the storage available 
    /// after a storage building is destroyed
    /// </summary>
    public void LimitAvailableCapacity(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Water:
                if (_available.Water > _capacity.Water)
                    _available.Water = _capacity.Water;
                break;
            case ResourceType.Metal:
                if (_available.Metal > _capacity.Metal)
                    _available.Metal = _capacity.Metal;
                break;
            case ResourceType.Food:
                if (_available.Food > _capacity.Food)
                    _available.Food = _capacity.Food;
                break;
            case ResourceType.Energy:
                if (_available.Energy > _capacity.Energy)
                    _available.Energy = _capacity.Energy;
                break;
        }

        ResourcesStatsChanged?.Invoke();
    }

    // To check if space is available to store the produced resources and returns the available space.
    public bool IsResourceSpaceAvailable(ResourceType type, out int spaceAvailable)
    {
        switch (type)
        {
            case ResourceType.Water:
                spaceAvailable = _capacity.Water - _available.Water;
                return _available.Water < _capacity.Water;
            case ResourceType.Metal:
                spaceAvailable = _capacity.Metal - _available.Metal;
                return _available.Metal < _capacity.Metal;
            case ResourceType.Food:
                spaceAvailable = _capacity.Food - _available.Food;
                return _available.Food < _capacity.Food;
            case ResourceType.Energy:
                spaceAvailable = _capacity.Energy - _available.Energy;
                return _available.Energy < _capacity.Energy;
            default:
                spaceAvailable = 0;
                return false;
        }
    }

    // Adds the produced resources.
    public void AddResources(ResourceType type, int quantity)
    {
        switch (type)
        {
            case ResourceType.Water:
                _available.Water += quantity;
                break;
            case ResourceType.Metal:
                _available.Metal += quantity;
                break;
            case ResourceType.Food:
                _available.Food += quantity;
                break;
            case ResourceType.Energy:
                _available.Energy += quantity;
                break;
        }

        ResourcesStatsChanged?.Invoke();
    }

    public ResourceQuantity GetAvailableResource()
    {
        return _available;
    }

    public ResourceQuantity GetMaxResourceCapacity()
    {
        return _capacity;
    }
}
