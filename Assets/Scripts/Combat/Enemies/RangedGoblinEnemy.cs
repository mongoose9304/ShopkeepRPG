using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedGoblinEnemy : BasicEnemy
{
    public bool isPreparingShot;
    [SerializeField] float shotPrepTimeMax;
    [SerializeField] float shotPrepTimecurrent;
    [SerializeField] float optimalDistanceToPlayer;
    [SerializeField] protected MMMiniObjectPooler attackProjectilesPool;
    [SerializeField] string specialAttackPoolName;
    [SerializeField] Transform attackSpawn;
    public float maxSpecialCooldown;
    float SpecialCooldown;
    public Animator anim;
    Vector3 lookAt;
    public override void Attack()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > attackDistance||isPreparingShot)
            return;
        if(anim)
        {
            anim.SetTrigger("BasicAttack");
        }
        isPreparingShot = true;
        //GameObject obj = attackIconPooler.GetPooledGameObject();
       // obj.transform.position = transform.position;
       // obj.SetActive(true);
        shotPrepTimecurrent = shotPrepTimeMax;
        //transform.rotation = new Quaternion(0, transform.rotation.y, 0, 0);
        
    }

    public void TornadoAttack()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > attackDistance || isPreparingShot)
            return;
        if (anim)
        {
            anim.SetTrigger("SpecialAttack");
        }
        if (!ProjectileManager.instance)
            return;

        MMMiniObjectPooler pool = ProjectileManager.instance.GetPoolFromName(specialAttackPoolName);
        if (pool == null)
            return;
        GameObject obj = pool.GetPooledGameObject();
        if (obj == null)
            return;
        obj.transform.position = attackSpawn.position;
        obj.transform.rotation = attackSpawn.rotation;
        obj.GetComponent<EnemyDamageCollider>().myElement = myElement;
        obj.GetComponent<EnemyDamageCollider>().damage = damage*0.75f;
        obj.GetComponent<EnemyDamageCollider>().myTeam = GetTeam();
        if(target)
        obj.GetComponent<HomingAttack>().target = target.transform;
        obj.SetActive(true);
        isPreparingShot = false;
        currentAttackCooldown = maxAttackCooldown;
    }
    protected override void OnEnable()
    {
        ResetEnemy();
        FindTarget();
        SpecialCooldown = maxSpecialCooldown;
        if (anim)
        {
            anim.ResetTrigger("BasicAttack");
        }
        shotPrepTimecurrent = shotPrepTimeMax;
    }
    protected override void Update()
    {
        if (TempPause.instance.isPaused)
            return;
        if (currentHitstun > 0)
        {
            CheckStun();
            return;
        }
        Move();
        if (isPreparingShot)
        {
            if (!target)
                target = CheckIfPlayerIsCloserThanFamiliar();
            if (!target.activeInHierarchy)
                target = CheckIfPlayerIsCloserThanFamiliar();
            lookAt = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.LookAt(lookAt, Vector3.up);
            PrepShot();
        }
        else
        {
            WaitingToAttack();
        }

    }

    public override void WaitingToAttack()
    {
        currentAttackCooldown -= Time.deltaTime;
        SpecialCooldown -= Time.deltaTime;

        if (currentAttackCooldown <= 0)
        {

            currentAttackCooldown = maxAttackCooldown;
            Attack();
        }
        if (SpecialCooldown <= 0)
        {

            SpecialCooldown = maxSpecialCooldown;
            TornadoAttack();
        }
    }
    private void PrepShot()
    {
       
        shotPrepTimecurrent -= Time.deltaTime;
       
        if (shotPrepTimecurrent<=0)
        {
            GameObject obj = attackProjectilesPool.GetPooledGameObject();
            obj.transform.position = attackSpawn.position;
            obj.transform.rotation = attackSpawn.rotation;
            obj.GetComponent<EnemyProjectile>().myElement = myElement;
            obj.GetComponent<EnemyProjectile>().damage = damage;
            obj.GetComponent<EnemyProjectile>().myTeam = GetTeam();
            obj.SetActive(true);
            isPreparingShot = false;
            currentAttackCooldown = maxAttackCooldown;
            EndAttack();
        }
      
    }
  
}
