using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfoManager : MonoBehaviour
{

    [SerializeField] private BuildingInfoWindow  _infoWindow;

    public void OnClose()
    {
        _infoWindow.gameObject.SetActive(false);
    }

    public void SetTile(Tile tile)
    {
        //TODO info window should not fo out of screen area, need to work on that
        //InfoWindow.transform.position =  Camera.main.WorldToScreenPoint(buildingInfo.position);
        _infoWindow.gameObject.SetActive(true);
        _infoWindow.SetContext(tile);
    }
}
