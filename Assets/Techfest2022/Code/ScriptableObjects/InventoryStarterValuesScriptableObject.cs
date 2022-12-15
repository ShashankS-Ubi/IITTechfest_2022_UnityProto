using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Inventory Starter Data")]
public class InventoryStarterValuesScriptableObject : ScriptableObject
{
    [field:SerializeField,Tooltip("Starting available resources")]
    public ResourceQuantity InitialAvailableResources = new ResourceQuantity(50, 50, 50, 50);

    [field: SerializeField,Tooltip("Starting total capacity")]
    public ResourceQuantity InitialCapacity = new ResourceQuantity(50, 50, 50, 50);
}
