using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Place to save/Hold current equiptment 
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerEquiptmentHolder", order = 1)]
public class PlayerEquiptmentHolder : ScriptableObject
{
    public List<EquipmentStatBlock> Rings = new List<EquipmentStatBlock>();
    public EquipmentStatBlock MeleeWeapon;
    public EquipmentStatBlock RangedWeapon;
    public EquipmentStatBlock Armor;
}
