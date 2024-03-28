using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatFamiliar : MonoBehaviour
{
    [SerializeField] GameObject player;
    public GameObject target;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] float specialAttackCooldownMax;
    [SerializeField] float ultimateAttackCooldownMax;
    float specialAttackCooldowncurrent;
    float ultimateAttackCooldowncurrent;
    [SerializeField] float maxDistanceToPlayer;
    [SerializeField] float maxDistanceToTarget;
    [SerializeField] float respawnTimeMax;
    float respawnTimeCurrent;
    [SerializeField] BasicMonsterData monsterData;
    float currentHealth;
    float damage;
    bool hasLookedForNewtarget;
    protected virtual void Start()
    {
        specialAttackCooldowncurrent = specialAttackCooldownMax;
        ultimateAttackCooldowncurrent = ultimateAttackCooldownMax;
        currentHealth = monsterData.CalculateHealth();
        damage = monsterData.CalculateDamage();
        player = GameObject.FindGameObjectWithTag("Player");        
    }
    protected virtual void Update()
    {
        FollowPlayer();
        EnemyDetection();
    }
    public virtual void FollowPlayer()
    {
        if (target)
            return;
        if (Vector3.Distance(this.transform.position, player.transform.position) > maxDistanceToPlayer)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    public virtual void EnemyDetection()
    {

        if(!target)
        {
            if(!hasLookedForNewtarget)
            {
                hasLookedForNewtarget = true;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxDistanceToTarget-1);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.tag == "Enemy")
                    {
                        if (!target)
                            target = hitCollider.gameObject;
                        else
                        {
                            if (Vector3.Distance(this.transform.position, target.transform.position) > Vector3.Distance(this.transform.position, hitCollider.transform.position))
                            {
                                target = hitCollider.gameObject;
                            }
                        }
                    }
                }
            }
            return;
        }
        hasLookedForNewtarget = false;
        if (Vector3.Distance(this.transform.position, target.transform.position) < maxDistanceToTarget)
        {
            agent.SetDestination(target.transform.position);
        }
        else
        {
            target = null;
        }
    }


    public virtual void Attack()
    {

    }
    public virtual void SpecialAttack()
    {

    }
    public virtual void UltimateAttack()
    {

    }
}
