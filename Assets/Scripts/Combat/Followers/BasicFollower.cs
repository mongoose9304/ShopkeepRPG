using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class BasicFollower : MonoBehaviour
{
    [Header("Stats")]
    public int Level;
    public BasicMonsterData myBaseData;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float maxHitstun;
    [SerializeField] protected float maxAttackCooldown;
    [SerializeField] protected float maxSpecialCooldown;
    [SerializeField] protected Element myElement;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float specialDistance;
    [SerializeField] protected float knockBackMax;
    [SerializeField] protected float basicDamage;
    [SerializeField] protected float physicalDef;
    [SerializeField] protected float mysticalDef;
    [SerializeField] protected float basicStun;
    [SerializeField] protected float specialDamage;
    [SerializeField] protected bool isMysticalDamage;
    [SerializeField] float maxDistanceToMyMaster;
    [Header("CurrentValues")]
    public bool canMove;
    float currentTimeStunned;
    //count for how long an enemy has been stunned, after this passes the max an enemy should be able to act regardless of if the player could stun them again
    protected float currentHitstun;
    protected float currentAttackCooldown = 0.2f;
    protected float currentSpecialCooldown = 0.2f;
    protected float currentHealth;
    bool superArmor;

    [Header("References")]
    public FollowerMaster myMaster;
    Element myWeakness;
    public GameObject stunIcon;
    public GameObject target;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected TextMeshProUGUI damageText; 
    [SerializeField] float maxTimeBeforeDamageTextFades;
    [SerializeField] float currentTimeBeforeDamageTextFades;
    [SerializeField] float fadeTimeMultiplier;
    [SerializeField] TeamUser myTeamUser;


    [Header("Feel")]
    [SerializeField] MMF_Player textSpawner;
    [SerializeField] MMF_Player hitEffects;
    public MMF_FloatingText floatingText;
    public bool useFlicker;
    public virtual void Update()
    {
        if (TempPause.instance.isPaused)
            return;
        if (target)
            FollowTarget();
        else
            FollowMaster();

        WaitForAttacks();

    }

    /// <summary>
    /// Causes the Familiar to walk towards the player if they have no current target
    /// </summary>
    public virtual void FollowMaster()
    {
        if (Vector3.Distance(this.transform.position, myMaster.transform.position) > maxDistanceToMyMaster)
        {
            agent.SetDestination(myMaster.transform.position + new Vector3(0, 0, 2));
        }
        if (Vector3.Distance(this.transform.position, myMaster.transform.position) > maxDistanceToMyMaster*2)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(myMaster.transform.position + new Vector3(2, 0, 0), out hit, 3.0f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }
        }

    }
    /// <summary>
    /// Causes the Familiar to walk towards the target 
    /// </summary>
    public virtual void FollowTarget()
    {
        if (!target.activeInHierarchy)
        {
            target = myMaster.TryToGetNewTarget();

            return;
        }
            agent.SetDestination(target.transform.position);
        
        
    }
    /// <summary>
    /// Causes the Familiar to attack targets
    /// </summary>
    public virtual void WaitForAttacks()
    {
        currentAttackCooldown -= Time.deltaTime;
        currentSpecialCooldown -= Time.deltaTime;
        if (currentSpecialCooldown <= 0)
        {

            currentSpecialCooldown = maxSpecialCooldown;
            SpecialAttack();
        }
        else if (currentAttackCooldown <= 0)
        {

            currentAttackCooldown = maxAttackCooldown;
            Attack();
        }

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
    public virtual void TakeDamage(float damage_, float hitstun_, Element element_, float knockBack_ = 0, GameObject knockBackObject = null,bool isMystical=false)
    {
        float newDamage = damage_;
        if (element_ == myWeakness && element_ != Element.Neutral)
        {
            newDamage *= 1.5f;
        }
        if (isMystical)
        {
            newDamage -= mysticalDef;
        }
        else
        {
            newDamage -= physicalDef;
        }
        if (newDamage < damage_ * 0.05f)
            newDamage = damage_ * 0.05f;
        currentHealth -= damage_;
        if (currentHealth <= 0)
        {
            Death();
            return;
        }

        if (textSpawner)
        {
            floatingText.Value = damage_.ToString();
            textSpawner.PlayFeedbacks();
        }
        if (hitEffects)
            hitEffects.PlayFeedbacks();
    }
    public virtual void Death()
    {
        gameObject.SetActive(false);
        myMaster.FollowerDeath(transform);
    }
    public virtual void ResetStats()
    {
        currentHealth = maxHealth;
    }
    public virtual void OnEnable()
    {
        ResetStats();
    }
    public virtual void SetStats(float health, float regularDamage, float specialDamage_, float physicalDef_, float mysticalDef_)
    {
        basicDamage = regularDamage;
        specialDamage = specialDamage_;
        maxHealth = health;
        currentHealth = maxHealth;
        physicalDef = physicalDef_;
        mysticalDef = mysticalDef_;
    }
}
