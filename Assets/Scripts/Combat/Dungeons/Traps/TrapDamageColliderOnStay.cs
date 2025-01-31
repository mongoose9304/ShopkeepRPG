using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamageColliderOnStay : TrapDamageCollider
{
    public float MaxTimeInterval;
    float currentTimeInterval;
    public float onEnableDamageDelay;
    float onEnableDamageDelayCurrent;
    private void OnEnable()
    {
        onEnableDamageDelayCurrent = onEnableDamageDelay;
    }
    protected override void OnTriggerEnter(Collider other)
    {

    }
    protected virtual void OnTriggerStay(Collider other)
    {
        if (onEnableDamageDelayCurrent > 0)
        {
            onEnableDamageDelayCurrent -= Time.deltaTime;
            return;
        }
            if (other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerFamiliar" || other.gameObject.tag == "Familiar" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "Follower")
        {
            currentTimeInterval -= Time.deltaTime;
            if (currentTimeInterval > 0)
            {
                return;
            }
        }
        if (other.gameObject.tag == "Player")
        {

            other.gameObject.GetComponent<CombatPlayerMovement>().TakeDamage(damage, 0, element, 0, this.gameObject, isMysicalDamage);
            currentTimeInterval = MaxTimeInterval;

        }
        else if (other.gameObject.tag == "PlayerFamiliar")
        {

            other.gameObject.GetComponent<CombatCoopFamiliar>().TakeDamage(damage, 0, element, 0, this.gameObject, isMysicalDamage);
            currentTimeInterval = MaxTimeInterval;

        }
        else if (other.gameObject.tag == "Familiar")
        {

            other.gameObject.GetComponent<CombatFamiliar>().TakeDamage(damage, 0, element, 0, this.gameObject, isMysicalDamage);
            currentTimeInterval = MaxTimeInterval;

        }
        else if (other.tag == "Enemy")
        {
            if (!canDamageEnemies)
            {
                return;
            }
                other.gameObject.GetComponent<BasicEnemy>().ApplyDamage(damage, 0, element, 0, this.gameObject, "", isMysicalDamage);
            currentTimeInterval = MaxTimeInterval;
        }
    }
}
