using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Production Building Upgrade Data")]
public class ProductionBuildingUpgradeDataScriptableObject : ScriptableObject
{
    [Header("Storage Building Upgrade Data")]
    [field: SerializeField]
    public List<ProductionBuildingUpgradeData> UpgradeData = new List<ProductionBuildingUpgradeData>();
}
