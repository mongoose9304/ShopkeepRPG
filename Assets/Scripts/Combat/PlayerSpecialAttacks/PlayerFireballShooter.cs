using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class PlayerFireballShooter : PlayerSpecialAttack
{
   public MMMiniObjectPooler fireballPool;
   public MMMiniObjectPooler fireballExplosionPool;
    public Transform spawn;

    public override void OnPress(GameObject obj_)
    {
        GameObject obj = fireballPool.GetPooledGameObject();
        obj.transform.position = spawn.position;
        obj.transform.rotation = spawn.rotation;
        obj.GetComponent<PlayerFireball>().damage = baseDamage;
        obj.GetComponent<PlayerFireball>().objectToSpawn = fireballExplosionPool.GetPooledGameObject();
        obj.SetActive(true);
    }
    public override void CalculateDamage(float PATK, float MATK)
    {
        baseDamage = MATK * 10;
    }
}
