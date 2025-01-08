using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Damage colliders for traps
/// </summary>
public class TrapDamageCollider : MonoBehaviour
{
    [Tooltip("Total damage of the trap")]
    public float damage;
    [Tooltip("Can this trap hit enemies too? or only players?")]
    public bool canDamageEnemies;
    [Tooltip("What element will the damage be")]
    public Element element;
    [Tooltip("Will the damage be mystical")]
    public bool isMysicalDamage;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            other.gameObject.GetComponent<CombatPlayerMovement>().TakeDamage(damage, 0, element, 0, this.gameObject,isMysicalDamage);
        }
        else if(other.tag == "Familiar")
        {
            other.gameObject.GetComponent<CombatFamiliar>().TakeDamage(damage, 0, element, 0, this.gameObject);
        }
        if(canDamageEnemies)
        {
            if (other.tag == "Enemy")
            {
                if (other.gameObject.TryGetComponent<BasicEnemy>(out BasicEnemy basicEnemyRef))
                {
                    basicEnemyRef.ApplyDamage(damage, 0, element, 0, this.gameObject,"",isMysicalDamage);

                }
            }
        }
    }
}
