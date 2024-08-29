using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageColliderOnStay : PlayerDamageCollider
{
    public float MaxTimeInterval;
    float currentTimeInterval;
    protected override void OnTriggerEnter(Collider other)
    {
        
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            currentTimeInterval -= Time.deltaTime;
            if (currentTimeInterval <= 0)
            {
                if (other.gameObject.TryGetComponent<BasicEnemy>(out basicEnemyRef))
                {
                    basicEnemyRef.ApplyDamage(damage, hitStun, element, knockBack, this.gameObject);
                    currentTimeInterval = MaxTimeInterval;
                }
            }
        }
    }
}
