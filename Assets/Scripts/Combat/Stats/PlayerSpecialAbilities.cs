using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSpecialAbilityReference
{
    public PlayerSpecialAttack theSpecialAttack;
    public string name_;
    public string description_;
    public bool isLocked=true;
    public Sprite abilitySprite;
}
/// <summary>
/// The saved Talents of a player
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerSpecialAbilities", order = 1)]
public class PlayerSpecialAbilities : ScriptableObject
{
    public List<PlayerSpecialAbilityReference> allAbilities= new List<PlayerSpecialAbilityReference>();
    public List<PlayerSpecialAbilityReference> currentlyEquipt = new List<PlayerSpecialAbilityReference>();
    public void SetEquiptPower(int index_,string name)
    {
        PlayerSpecialAbilityReference ref_=null;
        Debug.Log("NameSupplied "+ name);
        foreach (PlayerSpecialAbilityReference reference in allAbilities)
        {
            if(reference.name_==name)
            {
                ref_ = reference;
                Debug.Log("NameMatch");
                break;
            }
        }
        if(ref_!=null)
        {
            currentlyEquipt[index_].abilitySprite = ref_.abilitySprite;
            currentlyEquipt[index_].theSpecialAttack = ref_.theSpecialAttack;
            currentlyEquipt[index_].name_ = ref_.name_;
            currentlyEquipt[index_].description_ = ref_.description_;
        }
    }
}
