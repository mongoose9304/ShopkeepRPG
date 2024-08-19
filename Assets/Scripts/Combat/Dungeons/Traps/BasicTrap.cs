using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTrap : MonoBehaviour
{
    public float damage;
    [SerializeField] TrapDamageCollider[] damageColliders;
    
    public virtual void EnableTrap()
    {
        gameObject.SetActive(true);
        if(damageColliders.Length!=0)
        { 
        foreach(TrapDamageCollider col in damageColliders)
            {
                col.damage = damage;
            }
        }
    }
    public virtual void DisableTrap()
    {
        gameObject.SetActive(true);
    }
}
