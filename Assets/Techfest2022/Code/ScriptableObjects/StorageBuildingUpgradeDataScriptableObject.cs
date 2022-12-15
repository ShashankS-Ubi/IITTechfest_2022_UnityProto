using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Storage Building Upgrade Data")]
public class StorageBuildingUpgradeDataScriptableObject : ScriptableObject
{
    [Header("Storage Building Upgrade Data")]
    [field: SerializeField]
    public List<StorageBuildingUpgradeData> UpgradeData = new List<StorageBuildingUpgradeData>();
}
