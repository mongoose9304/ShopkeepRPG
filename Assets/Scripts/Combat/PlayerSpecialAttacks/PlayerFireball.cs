using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireball : ProjectileWithEffectOnEnd
{
    public float damage;

    public override void Activate()
    {
        objectToSpawn.GetComponent<PlayerDamageCollider>().damage = damage;
        objectToSpawn.transform.position = transform.position;
        objectToSpawn.SetActive(true);
    }
}
