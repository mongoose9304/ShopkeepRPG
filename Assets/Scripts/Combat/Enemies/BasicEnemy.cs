using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using MoreMountains.Feedbacks;
/// <summary>
/// This is a mostly virtual class all the enemies should inherit from. Contains all the default things enemies need. 
/// </summary>
public class BasicEnemy : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Elite enemies are immmune to hitstun ")]
    public bool isElite;
    public int Level;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float knockBackMax;
    [SerializeField] protected float maxAttackCooldown;
    [Tooltip("The data for a monsters stats. All the be")]
    public BasicMonsterData myBaseData;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float maxHitstun;
    [SerializeField] protected Element myElement;
    [SerializeField] protected float damage;
    [SerializeField] protected float physicalDefence;
    [SerializeField] protected float mysticalDefence;
    [SerializeField] protected bool isMysticalDamage;
    [Header("Status Effects")]
    protected bool isHexed;
    protected float hexTime;
   

    [Header("CurrentValues")]
    public bool canMove;
    float currentTimeStunned;
    //count for how long an enemy has been stunned, after this passes the max an enemy should be able to act regardless of if the player could stun them again
    protected float currentHitstun;
    protected float currentAttackCooldown=0.2f;
    protected float currentHealth;
    float currentKnockbackPower;
    float currentKnockbackTime;
    private Vector3 knockbackRefVector;
    Vector3 knockBackDirection;
    //is the user currentlt immune to hitstun
    bool superArmor;

    [Header("References")]
    [Tooltip("REFERENCE to the icon that displays I am stunned")]
    public GameObject stunIcon;
    [Tooltip("REFERENCE to the player object for targeting")]
    public GameObject player;
    [Tooltip("REFERENCE to the 2ndplayer object for targeting")]
    public GameObject playerFamiliar;
    [Tooltip("The current object I am trying to move towards and fight")]
    public GameObject target;
    [Tooltip("Used for rooms where the player must kill enemies to leave and such")]
    public EnemyCounter myEnemyCounter;
    [SerializeField]protected NavMeshAgent agent;
    [Tooltip("REFERENCE to the text that pops up when taking damage")]
    [SerializeField] protected TextMeshProUGUI damageText;
    [SerializeField] float maxTimeBeforeDamageTextFades;
    [SerializeField] float currentTimeBeforeDamageTextFades;
    [SerializeField] float fadeTimeMultiplier;
    [Tooltip("REFERNCE to the team I am on")]
    [SerializeField] TeamUser myTeamUser;
    public GameObject hexStatusEffect;
    [Tooltip("REFERNCE to the script that allows for items to drop ")]
    LootDropper lootDropper;


    [Header("Feel")]
    [Tooltip("REFERENCE to the text that pops up when taking damage")]
    [SerializeField] MMF_Player textSpawner;
    [Tooltip("REFERENCE to the effects that happen when i am hit ")]
    [SerializeField]protected  MMF_Player hitEffects;
    [Tooltip("REFERENCE to the text that pops up when taking damage")]
    public MMF_FloatingText floatingText;
    public bool useFlicker;


    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        LoadMonsterData();
        lootDropper = GetComponent<LootDropper>();
        currentHealth = maxHealth;
        currentHealth = maxHealth;
        floatingText = textSpawner.GetFeedbackOfType<MMF_FloatingText>();
        //fix this later, if the enemies have the same channel thier damage numbers will appear even if they are not hit =(
        floatingText.Channel = Random.Range(0, 1000000);
        textSpawner.GetComponent<MMFloatingTextSpawner>().Channel = floatingText.Channel;
    }

    protected virtual void Update()
    {
        if (TempPause.instance.isPaused)
            return;
        if (currentHitstun > 0)
        {
            CheckStun();
            return;
        }
        Move();
        WaitingToAttack();
        
    }
    protected virtual void OnEnable()
    {
        ResetEnemy();
        NavMeshHit hit;
        FindTarget();
        if (NavMesh.SamplePosition(transform.position, out hit, 3.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        
    }
    /// <summary>
    /// Reset an enemy post death
    /// </summary>
    protected virtual void ResetEnemy()
    {
        LoadMonsterData();
        currentHealth = maxHealth;
        if (useFlicker)
            hitEffects.GetFeedbackOfType<MMF_Flicker>().ForceInitialValue(hitEffects.transform.position);
        agent.enabled = true;
        currentHitstun = 0;
        canMove = true;
        stunIcon.SetActive(false);
        hexTime = 0;
        isHexed = false;
        if(hexStatusEffect)
        {
            hexStatusEffect.SetActive(false);
        }
    }

    /// <summary>
    /// Checks if the enemy is stunned and activate superarmor if it has been too much time stunned
    /// </summary>
    protected void CheckStun()
    {
        stunIcon.SetActive(true);
        currentHitstun -= Time.deltaTime;
        currentTimeStunned += Time.deltaTime;
        if(currentKnockbackTime>0)
        {
            currentKnockbackTime -= Time.deltaTime;
            transform.position = Vector3.SmoothDamp(transform.position,transform.position+(knockBackDirection*currentKnockbackPower*Time.deltaTime),ref knockbackRefVector ,0.3f);
        }
        if(currentTimeStunned>=maxHitstun||currentHitstun<=0)
        {
            superArmor = true;
            currentTimeStunned = 0;
            currentHitstun = 0;
            stunIcon.SetActive(false);
            agent.enabled = true;
        }


    }
    /// <summary>
    /// Used when outside forces affect the enemies movement such as pulls and pushes
    /// </summary>
    public void AffectMovement(Vector3 newPos)
    {
        agent.enabled = false;
        newPos.y = transform.position.y;
        transform.position = newPos;

    }

    /// <summary>
    /// Application of damage. (Includes stuns, element weaknesses, knockback and the direction of said knockback)
    /// </summary>
    /// <param name="damage_">The damage to take before defence or weakness calculations</param>
    /// <param name="hitstun_">The time the enemy will be stunned for after this attack</param>
    /// <param name="element_">The element of the damage</param>
    /// <param name="knockBack_">The magnitude of the knockback</param>
    /// <param name="knockBackObject">The object initiating the knockback effect</param>
    public virtual void ApplyDamage(float damage_,float hitstun_,Element element_,float knockBack_=0,GameObject knockBackObject=null,string playerAttackType="", bool isMystical = false)
    {
       if(knockBackObject)
        {
            if (target.TryGetComponent<BasicEnemy>(out BasicEnemy e))
            {
                if(e)
                target = e.gameObject;
            }
        }
        if (!superArmor)
        {
            if (!isElite)
            {
                currentHitstun += hitstun_;
                KnockBack(knockBack_, knockBackObject);
            }
        }
        float newDamage = damage_;
        if(isHexed)
        {
            newDamage *= 1.5f;
        }
        if(isMystical)
        {
            newDamage -= mysticalDefence;
        }
        else
        {
            newDamage -= physicalDefence;
        }
        EnemyManager.instance.ApplyHitEffect(element_,transform);
        if(newDamage<=damage_*0.05f)
        {
            newDamage = damage_ * 0.05f;
        }
        damage_ = Mathf.Round(newDamage);
        currentHealth -= newDamage;
        if(currentHealth<=0)
        {
            Death();
            return;
        }
        floatingText.Value = damage_.ToString();
        if(textSpawner)
        textSpawner.PlayFeedbacks();
        if (hitEffects)
            hitEffects.PlayFeedbacks();
        /*   //damage text 
           damageText.text = damage_.ToString();
           damageText.color = Color.white;
           currentDamageTextAlpha = 1;
           currentTimeBeforeDamageTextFades = maxTimeBeforeDamageTextFades;
        */
    }

    public virtual void ApplyStatusEffect(Status effect_,float statusTime)
    {
        switch(effect_)
        {
            case Status.Hexed:
                if(hexStatusEffect)
                {
                    hexStatusEffect.SetActive(true);
                }
                isHexed = true;
                hexTime += statusTime;
                break;
        }
    }
    public virtual void StatusEffectUpdates()
    {
        if(isHexed)
        {
            hexTime -= Time.deltaTime;
            if(hexTime<=0)
            {
                isHexed = false;
                if (hexStatusEffect)
                {
                    hexStatusEffect.SetActive(false);
                }
            }
        }
    }
    /// <summary>
    /// This will happen when the enemy runs out of health
    /// </summary>
    public virtual void Death()
    {
        gameObject.SetActive(false);
        lootDropper.DropItems();
        CoinSpawner.instance_.CreateDemonCoins(DungeonManager.instance.currentDungeon.GetBasicEnemyValue(),this.transform);
        LootManager.instance.AddExp(DungeonManager.instance.currentDungeon.GetBasicEnemyExpValue());
        if (myEnemyCounter)
            myEnemyCounter.currentEnemies -= 1;
        EnemyManager.instance.EnemyDeath(transform.position);
        agent.enabled = false;
        if(CombatPickupManager.instance)
        {

                CombatPickupManager.instance.TryForHealthPickup(transform);
                CombatPickupManager.instance.TryForManaPickup(transform);
        }
        //death effects buggy RN, add later -Rob
      //  Instantiate(deathEffects[Random.Range(0,deathEffects.Length)], transform.position+new Vector3(0,1,0), Quaternion.Euler(new Vector3(0, 0, 0)));
    }

    /// <summary>
    /// The enemy's basic attack
    /// </summary>
    public virtual void Attack()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > attackDistance)
            return;
    }

    /// <summary>
    /// Reset super armor once the enemy has got a a chance to attack
    /// </summary>
    public void EndAttack()
    {
        superArmor = false;
    }
    /// <summary>
    /// Move towards the player when allowed to
    /// </summary>
    public virtual void Move()
    {
        if (!agent.isActiveAndEnabled)
            return;
        if (canMove)
        {
            if (!target)
                target = CheckIfPlayerIsCloserThanFamiliar();
            if (!target.activeInHierarchy)
                  target = CheckIfPlayerIsCloserThanFamiliar();
            agent.SetDestination(target.transform.position);
        }
        else
            agent.ResetPath();
    }
    /// <summary>
    /// Attack cooldowns
    /// </summary>
    public void WaitingToAttack()
    {
        
        currentAttackCooldown -= Time.deltaTime;

        if(currentAttackCooldown<=0)
        {

            currentAttackCooldown = maxAttackCooldown;
            Attack();
        }
    }
    /// <summary>
    /// Kockbacked behavior 
    /// </summary>
    private void KnockBack(float knockBackPower,GameObject knockBackObject)
    {
        if (knockBackPower <= 0||!knockBackObject)
            return;

        agent.enabled = false;
        currentKnockbackTime = knockBackMax;
        currentKnockbackPower = knockBackPower;
        knockBackDirection = (transform.position - knockBackObject.transform.position).normalized;
        knockBackDirection.y = 0;

    }
    protected void LoadMonsterData()
    {
        maxHealth = myBaseData.CalculateHealth(Level);
        damage = myBaseData.CalculateDamage(Level);
        physicalDefence = myBaseData.CalculatePhysicalDefence(Level);
        mysticalDefence = myBaseData.CalculateMysticalDefence(Level);
    }
    public virtual void FindTarget()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (!playerFamiliar)
        {
            playerFamiliar = GameObject.FindGameObjectWithTag("PlayerFamiliar");
        }
        TryGetComponent<TeamUser>(out myTeamUser);
        if (!myTeamUser)
        {
            target = CheckIfPlayerIsCloserThanFamiliar();
            return;
        }
        GameObject obj=EnemyManager.instance.FindEnemyTarget(myTeamUser.myTeam, transform.position);
        if(obj!=null)
        {
            if (Vector3.Distance(transform.position,obj.transform.position)< Vector3.Distance(transform.position, player.transform.position))
            {
                target = obj;
            }
            else
            {
                target = CheckIfPlayerIsCloserThanFamiliar();
            }
        }
        else
        {
            Debug.Log("Obj Null");
            target = CheckIfPlayerIsCloserThanFamiliar();
        }
    }
    public virtual GameObject CheckIfPlayerIsCloserThanFamiliar()
    {
        if(!playerFamiliar)
        {
            return player;
        }
        if (!playerFamiliar.activeInHierarchy)
        {
            Debug.Log("Inactive");
            return player;
        }
        if (Vector3.Distance(transform.position,player.transform.position)> Vector3.Distance(transform.position, playerFamiliar.transform.position))
        {
            return playerFamiliar;
        }
        return player;
    }
    //returns true if the teams are different 
    public virtual bool CheckTeam(GameObject target)
    {
        if (target.TryGetComponent<TeamUser>(out TeamUser t_) && gameObject.TryGetComponent<TeamUser>(out TeamUser myT))
        {
            if (t_.myTeam != myT.myTeam)
                return true;
        }
        return false;
    }
    public string GetTeam()
    {
        if(gameObject.TryGetComponent<TeamUser>(out TeamUser myT))
        {
            return myT.myTeam;
        }
        return "";
    }

}
