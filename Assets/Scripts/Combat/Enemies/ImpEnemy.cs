using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Blobcreate.ProjectileToolkit;

public class ImpEnemy : BasicEnemy
{
    [SerializeField] protected MMMiniObjectPooler attackProjectilesPool;
    [SerializeField] Transform attackSpawn;
    public bool canAttack;
    public UnityEvent endAttackEvent;
    public override void Attack()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > attackDistance&&canAttack)
            return;

        canAttack = false;
        GameObject obj = attackIconPooler.GetPooledGameObject();
         obj.transform.position = target.transform.position;
         obj.SetActive(true);
        GameObject objB = attackProjectilesPool.GetPooledGameObject();
        objB.transform.position = attackSpawn.position;
        //objB.GetComponent<LopProjectile>().target = target.transform.position;
        //var v = Projectile.VelocityByTime(myRigid.position, predictedPos, timeOfFlight);
        // myRigid.AddForce(v, ForceMode.VelocityChange);
        Debug.Log("V+ " + Projectile.VelocityByA(objB.transform.position, target.transform.position, -0.01f));
        objB.GetComponent<EnemyProjectile>().damage = damage;
        objB.GetComponent<EnemyProjectile>().myTeam = GetTeam();
        objB.GetComponent<OnDisableEvent>().endEvent = endAttackEvent;
        objB.SetActive(true);
        objB.GetComponent<Rigidbody>().velocity = Vector3.zero;
        objB.GetComponent<Rigidbody>().AddForce(Projectile.VelocityByA(objB.transform.position, target.transform.position, -0.1f), ForceMode.VelocityChange);

    }
    public void AttackHit()
    {
        attackIconPooler.ResetAllObjects();
        canAttack = true;
    }
    protected override void OnEnable()
    {
        ResetEnemy();
        FindTarget();
        canAttack = true;
    }
}
