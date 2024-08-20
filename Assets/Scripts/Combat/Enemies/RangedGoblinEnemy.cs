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
    public override void Attack()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > attackDistance||isPreparingShot)
            return;
        isPreparingShot = true;
        canMove = false;
        GameObject obj = attackIconPooler.GetPooledGameObject();
        obj.transform.position = transform.position;
        obj.SetActive(true);
        shotPrepTimecurrent = shotPrepTimeMax;
        
    }
    protected override void Update()
    {
        if (currentHitstun > 0)
        {
            CheckStun();
            return;
        }
        Move();
        WaitingToAttack();
        PrepShot();

    }
    private void PrepShot()
    {
        if (!isPreparingShot) return;
        shotPrepTimecurrent -= Time.deltaTime;

        if(shotPrepTimecurrent<=0)
        {
            GameObject obj = attackProjectilesPool.GetPooledGameObject();
            obj.transform.position = attackSpawn.position;
            obj.transform.rotation = attackSpawn.rotation;
            Debug.Log("Shoot");
        }
      
    }
    /// <summary>
    /// Move towards the player when allowed to
    /// </summary>
    public override void Move()
    {
        if (canMove)
        {
            if(Vector3.Distance(transform.position,player.transform.position)>=optimalDistanceToPlayer)
            agent.SetDestination(player.transform.position);
            else
            {
                agent.ResetPath();
            }
        }
        else
            agent.ResetPath();
    }
}
