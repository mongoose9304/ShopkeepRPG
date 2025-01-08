using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonFamiliar : CombatFamiliar
{
    public MMMiniObjectPooler specialAttackPool;
    public float specialRange;
    public float AttackDistance;
    public float RangedDistance;
    public float specialHexTime;
    public PlayerSkeletonMaster mySkeltonMaster;
    public float ultDurationMax;
    float ultDurationCurrent;
    bool isUlting;
    public GameObject ultObject;
    public GameObject normalObject;
    public MMMiniObjectPooler rangedAttackPool;
    public Transform rangedSpawn;
    public float ultimateAttackSpeed;
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
    public override void UltimateAttack()
    {
        if (player.GetComponent<CombatPlayerActions>().isBusy || ultimateAttackCooldowncurrent > 0)
            return;
        ultDurationCurrent = ultDurationMax;
        SetUltimateMode(true);
        ultimateAttackCooldowncurrent = ultimateAttackCooldownMax;
    }
    public void SetUltimateMode(bool ult_)
    {
        if(ult_)
        {
            ultDurationCurrent = ultDurationMax;
            isUlting = true;
            normalObject.SetActive(false); 
            ultObject.SetActive(true); 
        }
        else
        {
            ultDurationCurrent = 0;
            isUlting = false;
            normalObject.SetActive(true);
            ultObject.SetActive(false);
        }
    }
    /// <summary>
    /// Cooldowns for attacks
    /// </summary>
    private void WaitForAttacks()
    {

      
        if (isUlting)
        {
            AttackCooldowncurrent -= (Time.deltaTime*ultimateAttackSpeed);
            specialAttackCooldowncurrent -= (Time.deltaTime*ultimateAttackSpeed);
            if (ultDurationCurrent > 0)
                ultDurationCurrent -= Time.deltaTime;
            if(ultDurationCurrent<=0)
            {
                SetUltimateMode(false);
            }
        }
        else
        {
            AttackCooldowncurrent -= Time.deltaTime;
            specialAttackCooldowncurrent -= Time.deltaTime;
            ultimateAttackCooldowncurrent -= Time.deltaTime;
        }
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
        if(isUlting)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > RangedDistance)
                return;
            transform.LookAt(target.transform);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            GameObject obj = rangedAttackPool.GetPooledGameObject();
            obj.transform.position = rangedSpawn.position;
            obj.transform.rotation = rangedSpawn.rotation;
            obj.GetComponent<MagicMissile>().damage = MysticalAtk;
            obj.GetComponent<HomingAttack>().target = target.transform;
            obj.SetActive(true);

            return;
        }
        if (Vector3.Distance(transform.position, target.transform.position) > AttackDistance)
            return;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy")
            {
                hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(PhysicalAtk, 0, Element.Neutral, 0, this.gameObject,"",false);
            }
        }
    }
    public override void CalculateAllModifiers()
    {
        base.CalculateAllModifiers();
        mySkeltonMaster.SetStatsBasedOnPlayerLevel(monsterStats.Level);
    }
}
