using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptable Objects/BuildingMenu Data")]
public class BuildingsMenuDataScriptableObject : ScriptableObject
{
    [SerializeField] public List<BuildingMenuData> BuildingDataList = new List<BuildingMenuData>();
}

[Serializable]
public class BuildingMenuData
{
    public BuildingType BuildingCategory;
    public ResourceType Resource;
    public Tile Building;
    public string BuildingTitle;
    public Image BuildingIcon;
}
