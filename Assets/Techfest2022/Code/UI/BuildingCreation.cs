using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingCreation : MonoBehaviour, IPointerUpHandler
{
    [SerializeField]
    private BuildingType _buildingCategory;
    [SerializeField]
    private ResourceType _resource;

    private Camera _mainCamera = null;

    private void Start()
    {
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
                    //BuildingPlaced?.Invoke(_buildingCategory, _resource, tile);
                }
            }
        }
    }
}
