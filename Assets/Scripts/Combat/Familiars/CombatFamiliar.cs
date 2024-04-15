using MoreMountains.Feedbacks;
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
    private CombatPlayerMovement combatPlayerMovement;
    private CombatPlayerActions combatPlayerActions;
    [SerializeField] protected BasicMonsterData monsterData;
    [SerializeField] GameObject deathEffect;

    Element myWeakness;
    [Header("Stats")]
    [SerializeField]protected  float specialAttackCooldownMax;
    [SerializeField]protected float ultimateAttackCooldownMax;
    [SerializeField]protected float AttackCooldownMax;
    [SerializeField]public float RespawnTimeMax;
    protected float specialAttackCooldowncurrent;
    protected float AttackCooldowncurrent;
    protected float ultimateAttackCooldowncurrent;
    [SerializeField] float maxDistanceToPlayer;
    [SerializeField] float maxDistanceToTarget;
    [SerializeField] float respawnTimeMax;
    [SerializeField] float delayBeforeLookingForAnotherTargetMax;
    float delayBeforeLookingForAnotherTargetCurrent;
    float currentHealth;
    float damage;
    bool hasLookedForNewtarget;
    [Header("Feel")]
    [SerializeField] MMF_Player textSpawner;
    [SerializeField] MMF_Player hitEffects;
    public MMF_FloatingText floatingText;
    protected virtual void Start()
    {
        specialAttackCooldowncurrent = specialAttackCooldownMax;
        ultimateAttackCooldowncurrent = ultimateAttackCooldownMax;
        currentHealth = monsterData.CalculateHealth();
        damage = monsterData.CalculateDamage();
        player = GameObject.FindGameObjectWithTag("Player");
        if(player)
        {
            combatPlayerMovement = player.GetComponent<CombatPlayerMovement>();
            combatPlayerActions = player.GetComponent<CombatPlayerActions>();
        }
        anim = GetComponent<Animator>();
        CalculateWeakness();
        floatingText = textSpawner.GetFeedbackOfType<MMF_FloatingText>();
        //fix this later, if the enemies have the same channel their damage numbers will appear even if they are not hit =(
        floatingText.Channel = Random.Range(0, 1000000);
        textSpawner.GetComponent<MMFloatingTextSpawner>().Channel = floatingText.Channel;
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
            agent.SetDestination(player.transform.position+new Vector3(0,0,2));
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
            if (!target.activeInHierarchy)
                target = null;
        }
        else
        {
            target = null;
        }
       
    }
    public void TakeDamage(float damage_, float hitstun_, Element element_, float knockBack_ = 0, GameObject knockBackObject = null)
    {
        if (element_ == myWeakness && element_ != Element.Neutral)
        {
            damage_ *= 1.5f;
        }
        currentHealth -= damage_;
        if (currentHealth <= 0)
        {
            Death();
            return;
        }
        floatingText.Value = damage_.ToString();
        if (textSpawner)
            textSpawner.PlayFeedbacks();
        if (hitEffects)
            hitEffects.PlayFeedbacks();
        combatPlayerMovement.UpdateFamiliarHealth(currentHealth/monsterData.CalculateHealth());
    }
    private void Death()
    {
        combatPlayerActions.FamiliarDeath(respawnTimeMax);
        gameObject.SetActive(false);
        Instantiate(deathEffect,transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
    }
    public void Respawn()
    {
        specialAttackCooldowncurrent = specialAttackCooldownMax;
        ultimateAttackCooldowncurrent = ultimateAttackCooldownMax;
        currentHealth = monsterData.CalculateHealth();
        combatPlayerMovement.UpdateFamiliarHealth(currentHealth / monsterData.CalculateHealth());
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
    private void CalculateWeakness()
    {
        switch (monsterData.element)
        {
            case Element.Fire:
                myWeakness = Element.Water;
                break;
            case Element.Water:
                myWeakness = Element.Earth;
                break;
            case Element.Air:
                myWeakness = Element.Earth;
                break;
            case Element.Earth:
                myWeakness = Element.Fire;
                break;
        }
    }
    public float GetUltimateAttackCooldown()
    {
        return (ultimateAttackCooldownMax - ultimateAttackCooldowncurrent) / ultimateAttackCooldownMax;
    }
}
