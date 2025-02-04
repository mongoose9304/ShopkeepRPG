using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This fella here only exists to interface between the CombatPlayerController and the coopFamiliar
/// </summary>
public class CombatControllerInterface : MonoBehaviour
{
    public GameObject curseAuraPrefab;
    public CombatCurseAura curseAuraRef;
    public CombatCurseAura CreateCurseAura() {
        if(curseAuraRef != null) {
            Destroy(curseAuraRef.gameObject);
        }

        GameObject g = Instantiate(curseAuraPrefab); 
        g.transform.parent = gameObject.transform;
        g.transform.localPosition = Vector3.zero;
        return g.GetComponent<CombatCurseAura>();
    }

}
