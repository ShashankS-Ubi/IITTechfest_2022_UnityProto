using System;
using UnityEngine;

public enum ResourceType
{
    None,
    Water,
    Metal,
    Food,
    Energy
}

[Serializable]
public struct ResourceQuantity
{
    public int Water;
    public int Metal;
    public int Food;
    public int Energy;

    public ResourceQuantity(int water, int metal,int food,int energy)
    {
        Water = water;
        Metal = metal;
        Food = food;
        Energy = energy;
    }
}

public abstract class Building : MonoBehaviour
{
    [SerializeField]
    private ResourceType _buildingResourceType;
    [SerializeField]
    private MeshFilter _buildingMeshFilter;

    public ResourceType BuildingResourceType => _buildingResourceType;

    protected BuildingType _buildingType;
    public BuildingType BuildingType => _buildingType;

    public int CurrLevel { get; private set; }

    public int CurrHealth { get; private set; }
    public int MaxHealth { get; private set; }

    // create a new buidling of the repsective type and initialize data
    public abstract void CreateBuilding(IBuildingUpgradeData upgradeData);

    // Use the data available to set the current health to max health.
    public abstract void RepairBuilding();

    // upgrade the values of the respective building using the data available for the building type
    public abstract void UpgradeBuilding(IBuildingUpgradeData upgradeData);

    public abstract ResourceQuantity? GetUpgradeCost();

    public abstract ResourceQuantity? GetRepairCost();

    public void SetMaxHealth(int val)
    {
        MaxHealth = val;
    }

    public void SetHealth(int val)
    {
        CurrHealth = Mathf.Min(val, MaxHealth);
    }

    public void IncreaseLevel()
    {
        CurrLevel += 1;
    }

    public void DamageBuilding(int val)
    {
        CurrHealth -= val;
    }

    // replace the older mesh with a new one.
    public void ChangeBuildingMesh(Mesh mesh)
    {
        _buildingMeshFilter.mesh = mesh;
    }

    public virtual void DestroyBuilding()
    {
        Destroy(gameObject);
    }
}
