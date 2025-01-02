using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonFollower : BasicFollower
{
    public bool canUseSpecialAttack;
    public bool isRanged;
    public MMMiniObjectPooler specialAttackPool;
    public MMMiniObjectPooler rangedAttackPool;
    public Transform rangedSpawn;
    public override void Attack()
    {
       
        if (!target)
            return;
        if (Vector3.Distance(transform.position, target.transform.position) > attackDistance)
            return;

        if(isRanged)
        {
            transform.LookAt(target.transform);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            GameObject obj = rangedAttackPool.GetPooledGameObject();
            obj.transform.position = rangedSpawn.position;
            obj.transform.rotation = rangedSpawn.rotation;
            obj.GetComponent<FamiliarProjectile>().damage = specialDamage;
            obj.SetActive(true);

            return;
        }
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy")
            {
                hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(basicDamage,basicStun,myElement,0,this.gameObject);
               
            }
        }
    }
    /// <summary>
    /// The more character specific attack the familar knows, same as thier enemy counterpart
    /// </summary>
    public override void SpecialAttack()
    {
        if (!canUseSpecialAttack)
            return;

        if (specialAttackPool)
        {
            GameObject obj = specialAttackPool.GetPooledGameObject();
            obj.transform.position = transform.position;
            obj.GetComponent<ProjectileExplosion>().damage = specialDamage*5;
            obj.SetActive(true);
        }
    }
}
