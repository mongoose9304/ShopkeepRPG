using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamiliarDetection : MonoBehaviour
{
    [SerializeField] CombatFamiliar combatFamiliar;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Enemy")
        {
            if(combatFamiliar.target==null)
            combatFamiliar.target = other.gameObject;
        }
    }
}
