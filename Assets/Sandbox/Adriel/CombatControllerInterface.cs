using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControllerInterface : MonoBehaviour
{
    protected CombatCurseAura curseAuraRef;
    protected CombatCurseAura CreateCurseAura() {
        GameObject g = new GameObject("Curse Aura");
        g.transform.parent = gameObject.transform;
        g.AddComponent<CombatCurseAura>();
        return g.GetComponent<CombatCurseAura>();
    }
}
