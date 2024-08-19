using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamageCollider : MonoBehaviour
{
    public float damage;
    public bool canDamageEnemies;
    public Element element;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            other.gameObject.GetComponent<CombatPlayerMovement>().TakeDamage(damage, 0, element, 0, this.gameObject);
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
                    basicEnemyRef.ApplyDamage(damage, 0, element, 0, this.gameObject);

                }
            }
        }
    }
}
