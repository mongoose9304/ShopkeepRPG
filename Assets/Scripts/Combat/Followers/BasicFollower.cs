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
    [SerializeField] protected Element myElement;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float knockBackMax;
    [SerializeField] protected float damage;
    [SerializeField] protected bool isMysticalDamage;
    [SerializeField] float maxDistanceToMyMaster;
    [Header("CurrentValues")]
    public bool canMove;
    float currentTimeStunned;
    //count for how long an enemy has been stunned, after this passes the max an enemy should be able to act regardless of if the player could stun them again
    protected float currentHitstun;
    protected float currentAttackCooldown = 0.2f;
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

    /// <summary>
    /// Causes the Familiar to walk towards the player if they have no current target
    /// </summary>
    public virtual void FollowPlayer()
    {
        if (Vector3.Distance(this.transform.position, myMaster.transform.position) > maxDistanceToMyMaster)
        {
            agent.SetDestination(myMaster.transform.position + new Vector3(0, 0, 2));
        }

    }
    /// <summary>
    /// Causes the Familiar to walk towards the target 
    /// </summary>
    public virtual void FollowTarget()
    {
        if (!target.activeInHierarchy)
            target = null;
            agent.SetDestination(target.transform.position);
        
        
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
}
