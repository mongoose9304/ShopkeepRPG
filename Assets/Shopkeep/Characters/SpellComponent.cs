using UnityEngine;
using Shopkeeper;
using System.Collections;

public class SpellComponent : MonoBehaviour
{
    public void CastSpell(Spell s) {
        if(s.cooldown != null) { return; } //Returns if the spell on cooldown

        s.Cast(gameObject);
        s.cooldown = StartCoroutine(s.CooldownCoroutine()); 
    }
}
