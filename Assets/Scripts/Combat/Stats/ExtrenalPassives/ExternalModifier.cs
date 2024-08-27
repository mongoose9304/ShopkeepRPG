using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data for Equipments 
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ExternalModifier", order = 1)]
public class ExternalModifier : ScriptableObject
{
    public string modsName;
    public List<EquipModifier> myModifiers = new List<EquipModifier>();
}
