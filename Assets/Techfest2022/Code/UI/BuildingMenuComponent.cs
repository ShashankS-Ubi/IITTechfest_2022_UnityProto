using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuComponent : MonoBehaviour
{
    [SerializeField] private BuildingsMenuDataScriptableObject _buildingMenuItemsData;
    [SerializeField] private ScrollList _scrollList;

    void Awake()
    {
        _scrollList.Draw<BuildingMenuData>(_buildingMenuItemsData.BuildingDataList);
    }
}
