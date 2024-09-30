using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Mostly virtual class the traps inherit from 
/// </summary>
public class BasicTrap : MonoBehaviour
{
    [Tooltip("Total damage calculated by level and base damage")]
    public float damage;
    [Tooltip("Base damage of this trap at level 1")]
    public float baseDamage;
    [Tooltip("Extra damage per level")]
    public float damagePerLevel;
    [Tooltip("Level of the trap based on what dungeon you are in")]
    public float level;
    [Tooltip("REFERENCE to all the damage colliders")]
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
