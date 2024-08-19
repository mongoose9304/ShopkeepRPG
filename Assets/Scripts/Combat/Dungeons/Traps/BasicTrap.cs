using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTrap : MonoBehaviour
{
    public float damage;
    public float baseDamage;
    public float damagePerLevel;
    public float level;
    [SerializeField] TrapDamageCollider[] damageColliders;

    private void Start()
    {
        CalculateDamage();
    }
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
    public void CalculateDamage()
    {
        level = DungeonManager.instance.currentDungeon.GetTrapLevel();
        damage = baseDamage + damagePerLevel * level;
        foreach (TrapDamageCollider col in damageColliders)
        {
            col.damage = damage;
        }
    }
}
