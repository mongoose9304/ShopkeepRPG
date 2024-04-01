using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatFamiliar : MonoBehaviour
{
    [SerializeField] protected GameObject player;
    public GameObject target;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField]protected  float specialAttackCooldownMax;
    [SerializeField]protected float ultimateAttackCooldownMax;
    [SerializeField]protected float AttackCooldownMax;
    protected float specialAttackCooldowncurrent;
    protected float AttackCooldowncurrent;
    protected float ultimateAttackCooldowncurrent;
    protected Animator anim;
    [SerializeField] float maxDistanceToPlayer;
    [SerializeField] float maxDistanceToTarget;
    [SerializeField] float respawnTimeMax;
    [SerializeField] float delayBeforeLookingForAnotherTargetMax;
    float delayBeforeLookingForAnotherTargetCurrent;
    float respawnTimeCurrent;
   [SerializeField] protected BasicMonsterData monsterData;
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
        anim = GetComponent<Animator>();
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
            else
            {
                delayBeforeLookingForAnotherTargetCurrent -= Time.deltaTime;
                if(delayBeforeLookingForAnotherTargetCurrent <= 0)
                {
                    hasLookedForNewtarget = false;
                    delayBeforeLookingForAnotherTargetCurrent = delayBeforeLookingForAnotherTargetMax;
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
        if(!target.activeInHierarchy)
            target = null;
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
