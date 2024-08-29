using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ImpEnemy : BasicEnemy
{
    [SerializeField] protected MMMiniObjectPooler attackProjectilesPool;
    [SerializeField] Transform attackSpawn;
    public bool canAttack;
    public UnityEvent endAttackEvent;
    public override void Attack()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > attackDistance&&canAttack)
            return;

        canAttack = false;
        GameObject obj = attackIconPooler.GetPooledGameObject();
         obj.transform.position = player.transform.position;
         obj.SetActive(true);
        GameObject objB = attackProjectilesPool.GetPooledGameObject();
        objB.transform.position = attackSpawn.position;
        objB.GetComponent<LopProjectile>().target = player.transform.position;
        objB.GetComponent<EnemyProjectile>().damage = damage;
        objB.GetComponent<OnDisableEvent>().endEvent = endAttackEvent;
        objB.SetActive(true);

    }
    public void AttackHit()
    {
        attackIconPooler.ResetAllObjects();
        canAttack = true;
    }
    protected override void OnEnable()
    {
        ResetEnemy();
        canAttack = true;
    }
}
