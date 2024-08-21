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
    [SerializeField] Transform attackSpawn;
    Vector3 lookAt;
    public override void Attack()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > attackDistance||isPreparingShot)
            return;
        
        isPreparingShot = true;
        canMove = false;
        //GameObject obj = attackIconPooler.GetPooledGameObject();
       // obj.transform.position = transform.position;
       // obj.SetActive(true);
        shotPrepTimecurrent = shotPrepTimeMax;
        //transform.rotation = new Quaternion(0, transform.rotation.y, 0, 0);
        
    }
    protected override void OnEnable()
    {
        agent.enabled = true;
        LoadMonsterData();
        currentHealth = maxHealth;
        shotPrepTimecurrent = shotPrepTimeMax;
    }
    protected override void Update()
    {
        if (currentHitstun > 0)
        {
            CheckStun();
            return;
        }
        transform.LookAt(lookAt, Vector3.up);
        lookAt = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        if (isPreparingShot)
        {
            PrepShot();
        }
        else
        {
            Move();
            WaitingToAttack();
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
            obj.SetActive(true);
            Debug.Log("Shoot");
            isPreparingShot = false;
            currentAttackCooldown = maxAttackCooldown;
            EndAttack();
        }
      
    }
  
}
