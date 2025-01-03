using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonFamiliar : CombatFamiliar
{
    public MMMiniObjectPooler specialAttackPool;
    public float specialRange;
    public float AttackDistance;
    public float specialHexTime;
    public PlayerSkeletonMaster mySkeltonMaster;
    protected override void Update()
    {
        if (TempPause.instance.isPaused)
            return;
        FollowPlayer();
        EnemyDetection();
        RegenHealth();
        WaitForAttacks();
    }
    public override void SpecialAttack()
    {
        Collider[] hitCollidersB = Physics.OverlapSphere(transform.position, specialRange);
        GameObject obj = specialAttackPool.GetPooledGameObject();
        obj.transform.position = transform.position + new Vector3(0, 0.1f, 0) ;
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);
        foreach (var hitCollider in hitCollidersB)
        {
            if (hitCollider.tag == "Enemy")
            {
                hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyStatusEffect(Status.Hexed, specialHexTime);
            }
        }
    }
    /// <summary>
    /// Cooldowns for attacks
    /// </summary>
    private void WaitForAttacks()
    {

        AttackCooldowncurrent -= Time.deltaTime;
        specialAttackCooldowncurrent -= Time.deltaTime;
        ultimateAttackCooldowncurrent -= Time.deltaTime;
        if (!target)
            return;

        if (specialAttackCooldowncurrent <= 0)
        {

            specialAttackCooldowncurrent = specialAttackCooldownMax;
            SpecialAttack();
        }
        else if (AttackCooldowncurrent <= 0)
        {

            AttackCooldowncurrent = AttackCooldownMax;
            Attack();
        }
    }
    public override void Attack()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > AttackDistance)
            return;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy")
            {
                hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(PhysicalAtk, 0, Element.Neutral, 0, this.gameObject);
            }
        }
    }
    public override void CalculateAllModifiers()
    {
        base.CalculateAllModifiers();
        mySkeltonMaster.SetStatsBasedOnPlayerLevel(monsterStats.Level);
    }
}
