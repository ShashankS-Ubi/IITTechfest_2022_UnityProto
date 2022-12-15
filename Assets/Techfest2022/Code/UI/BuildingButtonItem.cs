using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingButtonItem : MonoBehaviour, IContext, IPointerUpHandler
{
    [SerializeField] private Text Title;
    private Camera _mainCamera = null;
    private Button _button = null;
    private BuildingMenuData _buildingData;
    public static Action<BuildingMenuData, Tile> BuildingPlaced;
    public void SetContext(object context)
    {
        if (context != null && context is BuildingMenuData)
        {
            _buildingData = context as BuildingMenuData;
            RefreshUI(_buildingData);
        }
    }

    void RefreshUI(BuildingMenuData data)
    {
        if (Title != null)
        {
            Title.text = data.BuildingTitle;
        }
    }


    private void Start()
    {
        _button = GetComponent<Button>();
        _mainCamera = Camera.main;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.collider.gameObject.layer == 3)
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                if (tile != null)
                {
                    BuildingPlaced?.Invoke(_buildingData, tile);
                }
            }
        }
    }
}
