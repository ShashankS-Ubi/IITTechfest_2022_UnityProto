using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu(menuName = "Scriptable Objects/Calamity Data")]
public class CalamityDataScriptableObject : ScriptableObject
{
    [SerializeField] public List<CalamityData> CalamityDataList = new List<CalamityData>();
}


public enum CalamityType
{
    THUNDERSTORM = 1,
    WINDSTORM = 2,
    RAID = 3,
    lastValue = 4

}
[Serializable]
public class CalamityData
{
    public CalamityType CalamityType;
    public ParticleSystem Vfx;
    public int DamageCofficient;
}
