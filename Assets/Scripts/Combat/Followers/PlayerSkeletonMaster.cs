using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkeletonMaster : FollowerMaster
{
    public float skeletonExplosionDamage;
    public override void SetStats(float health, float damage, float specialDamage_, float physicalDef_, float mysticalDef_)
    {
         maxHealth=health*0.2f;
     regularDamage=damage*0.2f;
    specialDamage=specialDamage_*0.2f;
        physicalDef = physicalDef_*0.2f;
        mysticalDef = mysticalDef_ * 0.2f;
        skeletonExplosionDamage = specialDamage * 3;
     }
    public override void FollowerDeath(Transform location_)
    {
        if (deathEffectPool)
        {
            GameObject obj = deathEffectPool.GetPooledGameObject();
            obj.transform.position = location_.position;
            obj.GetComponent<ProjectileExplosion>().damage = skeletonExplosionDamage;
            obj.SetActive(true);
        }
    }
}
