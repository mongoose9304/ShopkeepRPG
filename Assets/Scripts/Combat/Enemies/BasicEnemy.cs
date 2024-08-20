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
    public bool isElite;
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
    LootDropper lootDropper;

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
    bool superArmor;

    [Header("References")]
    Element myWeakness;
    public GameObject stunIcon;
    public GameObject player;
    public EnemyCounter myEnemyCounter;
    [SerializeField]protected NavMeshAgent agent;
    [SerializeField] protected TextMeshProUGUI damageText;
    [SerializeField] float maxTimeBeforeDamageTextFades;
    [SerializeField] float currentTimeBeforeDamageTextFades;
    [SerializeField] float fadeTimeMultiplier;
    [SerializeField] GameObject[] deathEffects;


    [Header("Feel")]
   [SerializeField] MMF_Player textSpawner;
    [SerializeField] MMF_Player hitEffects;
    public MMF_FloatingText floatingText;
    [SerializeField] protected MMMiniObjectPooler attackIconPooler;
    public bool useFlicker;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        LoadMonsterData();
        lootDropper = GetComponent<LootDropper>();
        currentHealth = maxHealth;
        //determine my weakness
        switch(myElement)
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
        currentHealth = maxHealth;
        floatingText = textSpawner.GetFeedbackOfType<MMF_FloatingText>();
        //fix this later, if the enemies have the same channel thier damage numbers will appear even if they are not hit =(
        floatingText.Channel = Random.Range(0, 1000000);
        textSpawner.GetComponent<MMFloatingTextSpawner>().Channel = floatingText.Channel;
        attackIconPooler = GetComponent<MMMiniObjectPooler>();
    }

    protected virtual void Update()
    {
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
        LoadMonsterData();
        currentHealth = maxHealth;
        if(useFlicker)
        hitEffects.GetFeedbackOfType<MMF_Flicker>().ForceInitialValue(hitEffects.transform.position);
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
    /// Application of damage. (Includes stuns, element weaknesses, knockback and the direction of said knockback)
    /// </summary>
    /// <param name="damage_">The damage to take before defence or weakness calculations</param>
    /// <param name="hitstun_">The time the enemy will be stunned for after this attack</param>
    /// <param name="element_">The element of the damage</param>
    /// <param name="knockBack_">The magnitude of the knockback</param>
    /// <param name="knockBackObject">The object initiating the knockback effect</param>
    public void ApplyDamage(float damage_,float hitstun_,Element element_,float knockBack_=0,GameObject knockBackObject=null)
    {
       
        if (!superArmor)
        {
            if (!isElite)
            {
                currentHitstun += hitstun_;
                KnockBack(knockBack_, knockBackObject);
            }
        }
        if(element_==myWeakness&&element_!=Element.Neutral)
        {
            damage_ *= 1.5f;
        }
        currentHealth -= damage_;
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
    /// <summary>
    /// This will happen when the enemy runs out of health
    /// </summary>
    public virtual void Death()
    {
        gameObject.SetActive(false);
        attackIconPooler.ResetAllObjects();
        lootDropper.DropItems();
        CoinSpawner.instance_.CreateDemonCoins(DungeonManager.instance.currentDungeon.GetBasicEnemyValue(),this.transform);
        if (myEnemyCounter)
            myEnemyCounter.currentEnemies -= 1;
        EnemyManager.instance.EnemyDeath();
        //death effects buggy RN, add later -Rob
      //  Instantiate(deathEffects[Random.Range(0,deathEffects.Length)], transform.position+new Vector3(0,1,0), Quaternion.Euler(new Vector3(0, 0, 0)));
    }

    /// <summary>
    /// The enemy's basic attack
    /// </summary>
    public virtual void Attack()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
            return;
    }

    /// <summary>
    /// Reset super armor once the enemy has got a a chance to attack
    /// </summary>
    public void EndAttack()
    {
        superArmor = false;
        attackIconPooler.ResetAllObjects();
    }
    /// <summary>
    /// Move towards the player when allowed to
    /// </summary>
    public virtual void Move()
    {
        if (canMove)
            agent.SetDestination(player.transform.position);
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
        maxHealth = myBaseData.CalculateHealth(false,Level);
        damage = myBaseData.CalculateDamage(false, Level);
    }

}
