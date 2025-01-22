using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This fella here only exists to interface between the CombatPlayerController and the coopFamiliar
/// </summary>
public class CombatControllerInterface : MonoBehaviour
{
    protected CombatCurseAura curseAuraRef;
    protected CombatCurseAura CreateCurseAura() {
        GameObject g = Instantiate(Resources.Load<GameObject>("Combat/Curse Aura")); 
        g.transform.parent = gameObject.transform;
        g.transform.localPosition = Vector3.zero;
        return g.GetComponent<CombatCurseAura>();
    }
}
