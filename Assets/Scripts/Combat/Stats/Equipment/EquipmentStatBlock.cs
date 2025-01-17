using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipType
{
    Ring, Armor, Melee, Ranged, Helmet
};
public enum Stat
{
    HP,SP,PATK,PDEF,MATK,MDEF,LUCK
};
public enum UniqueEquipEffect
{
    None, LifeSteal,SoulSteal,Thorns,HealthRegen,ManaRegen,basicRangedSpeed,basicMeleeSpeed
};
[System.Serializable]
public struct EquipModifier
{
    public string modName;
    public bool isMultiplicative;
    public Stat affectedStat;
    public float amount;
    public UniqueEquipEffect uniqueEffect;


};
/// <summary>
/// Data for Equipments 
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EquipmentStatBlock", order = 1)]
public class EquipmentStatBlock : ScriptableObject
{
    public string equiptName;
    [TextArea]
    public string description;
    public EquipType EquipmentType;
    public Element myElement;
    public List<EquipModifier> myModifiers = new List<EquipModifier>();
}
