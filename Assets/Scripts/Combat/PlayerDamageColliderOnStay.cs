using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageColliderOnStay : PlayerDamageCollider
{
    protected override void OnTriggerEnter(Collider other)
    {
        
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            if (other.gameObject.TryGetComponent<BasicEnemy>(out basicEnemyRef))
            {
                basicEnemyRef.ApplyDamage(damage, hitStun, element, knockBack, this.gameObject);

            }
        }
    }
}
