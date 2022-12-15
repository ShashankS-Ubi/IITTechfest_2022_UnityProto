using System;
using UnityEngine;

[RequireComponent(typeof(TileSelectionUI))]
public class Tile : MonoBehaviour
{
    [SerializeField]
    private GameObject _defaultTile = null;

    private bool _isFloor = false;
    public bool IsFloor => _isFloor;

    private bool _isWater = true;
    public bool IsWater => _isWater;

    private Building _currentBuilding = null;
    public Building CurrentBuilding => _currentBuilding;
    private GameObject _floorTile = null;


    private TileSelectionUI _selectionUI = null; //for future
    public static Action<Tile> BuildingClick;

#if UNITY_EDITOR
    public Transform[] TileSockets = new Transform[6];
#endif
    private void Awake()
    {
        _selectionUI = GetComponent<TileSelectionUI>();
        _selectionUI.ToggleSelection(false);
    }

    /// <summary>
    /// Water reclaims it's terri...tile!
    /// Degrades a tile to turn it back into water.
    /// </summary>
    public void MakeWater()
    {
        _isFloor = false;
        _isWater = true;
        if (_currentBuilding != null)
        {
            Destroy(_currentBuilding.gameObject);
        }

        if (_floorTile != null)
        {
            Destroy(_floorTile.gameObject);
        }

        _defaultTile.SetActive(true);
    }

    /// <summary>
    /// Builds a floor upon water tile.
    /// Conditions of whether it's a water tile or has a building, are checked in TileManager.
    /// </summary>
    /// <param name="floor"></param>
    public void MakeFloor(GameObject floor)
    {
        _isFloor = true;
        _isWater = false;
        _floorTile = floor;
        _floorTile.transform.position = transform.position;
        _floorTile.SetActive(true);
        _defaultTile.SetActive(false);
    }

    public void CreateBuilding(Building building, IBuildingUpgradeData upgradeData)
    {
        if (!_isFloor || _isWater)
            return;

        _currentBuilding = building;
        _currentBuilding.transform.position = transform.position + new Vector3(0,.001f,0);
        _currentBuilding.gameObject.SetActive(true);
        _floorTile.SetActive(false);
        _isFloor = false;
        building.CreateBuilding(upgradeData);
    }


    public void DestroyBuilding()
    {
        if (_currentBuilding != null)
        {
            _currentBuilding.gameObject.SetActive(false);
            _currentBuilding.DestroyBuilding();
        }

        //make the tile floor again, now since the building is destroyed.
        MakeFloor(_floorTile);
    }

    public void RepairBuilding()
    {
        if (_currentBuilding == null)
            return;

        _currentBuilding.RepairBuilding();
    }

    public void UpgradeBuilding(IBuildingUpgradeData upgradeData)
    {
        if (_currentBuilding == null)
            return;

        _currentBuilding.UpgradeBuilding(upgradeData);
    }

    public void DamageBuilding(int damage)
    {
        if (_currentBuilding == null)
            return;

        _currentBuilding.DamageBuilding(damage);
    }
    public void ShowSelectionUI(bool toggle)
    {
        _selectionUI.ToggleSelection(toggle);
        if (toggle)
        {
            BuildingClick?.Invoke(this);
        }
    }

}
