using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The mostly virtual class all familiars (allies that follow the player) inherit from
/// </summary>
public class CombatFamiliar : MonoBehaviour
{
    [Header("Referecnes")]
    [SerializeField] protected GameObject player;
    [SerializeField] protected NavMeshAgent agent;
    public GameObject target;
    protected Animator anim;
    [SerializeField] protected BasicMonsterData monsterData;
    [Header("Stats")]
    [SerializeField]protected  float specialAttackCooldownMax;
    [SerializeField]protected float ultimateAttackCooldownMax;
    [SerializeField]protected float AttackCooldownMax;
    protected float specialAttackCooldowncurrent;
    protected float AttackCooldowncurrent;
    protected float ultimateAttackCooldowncurrent;
    [SerializeField] float maxDistanceToPlayer;
    [SerializeField] float maxDistanceToTarget;
    [SerializeField] float respawnTimeMax;
    [SerializeField] float delayBeforeLookingForAnotherTargetMax;
    float delayBeforeLookingForAnotherTargetCurrent;
    float respawnTimeCurrent;
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
    /// <summary>
    /// Causes the Familiar to walk towards the player if they have no current target
    /// </summary>
    public virtual void FollowPlayer()
    {
        if (target)
            return;
        if (Vector3.Distance(this.transform.position, player.transform.position) > maxDistanceToPlayer)
        {
            agent.SetDestination(player.transform.position);
        }
    }
    /// <summary>
    /// Will cause the familiar to find the nearest enemy
    /// </summary>
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

    /// <summary>
    /// The most basic attack the familar knows
    /// </summary>
    public virtual void Attack()
    {

    }
    /// <summary>
    /// The more character specific attack the familar knows, same as thier enemy counterpart
    /// </summary>
    public virtual void SpecialAttack()
    {

    }
    /// <summary>
    /// This is a move the player can perform, it is usually a combo attack with the famiiar 
    /// </summary>
    public virtual void UltimateAttack()
    {

    }
}
