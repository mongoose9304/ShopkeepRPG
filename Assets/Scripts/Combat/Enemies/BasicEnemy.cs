using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using MoreMountains.Feedbacks;
// this is a mostly virtual class all the enemies should inherit from. Contains all the default things enemies need. 
public class BasicEnemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth;
    public float maxHitstun;
    public float maxAttackCooldown;
    public Element myElement;
    public float moveSpeed;
    public float attackDistance;
    public float knockBackMax;
    //count for how long an enemy has been stunned, after this passes the max an enemy should be able to act regardless of if the player could stun them again
    [Header("CurrentValues")]
    public bool canMove;
    float currentTimeStunned;
    protected float currentHitstun;
    float currentAttackCooldown=0.2f;
    float currentHealth;
    Element myWeakness;
    float currentKnockbackPower;
    float currentKnockbackTime;
    private Vector3 knockbackRefVector;
    Vector3 knockBackDirection;
    bool superArmor;
    [Header("References")]
    public GameObject stunIcon;
    public GameObject player;
    [SerializeField]protected NavMeshAgent agent;
    [SerializeField] protected TextMeshProUGUI damageText;
    [SerializeField] float maxTimeBeforeDamageTextFades;
    [SerializeField] float currentTimeBeforeDamageTextFades;
    [SerializeField] float fadeTimeMultiplier;
    [SerializeField] GameObject[] deathEffects;
    float currentDamageTextAlpha;
    [Header("Feel")]
   [SerializeField] MMF_Player textSpawner;
    [SerializeField] MMF_Player hitEffects;
    public MMF_FloatingText floatingText;
    [SerializeField] protected MMMiniObjectPooler attackIconPooler;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
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
    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    //Check if the enemy is stunned and activate superarmor if it has been too much time stunned
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

    //Application of damage
    public void ApplyDamage(float damage_,float hitstun_,Element element_,float knockBack_=0,GameObject knockBackObject=null)
    {

        if (!superArmor)
        {
            currentHitstun += hitstun_;
            KnockBack(knockBack_,knockBackObject);
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
    public virtual void Death()
    {
        gameObject.SetActive(false);
        attackIconPooler.ResetAllObjects();
        Instantiate(deathEffects[Random.Range(0,deathEffects.Length)], transform.position+new Vector3(0,1,0), Quaternion.Euler(new Vector3(0, 0, 0)));
    }
   
    public virtual void Attack()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
            return;
    }

    public void EndAttack()
    {
        superArmor = false;
        attackIconPooler.ResetAllObjects();
    }
    public virtual void Move()
    {
        if (canMove)
            agent.SetDestination(player.transform.position);
        else
            agent.ResetPath();
    }
    public void WaitingToAttack()
    {
        
        currentAttackCooldown -= Time.deltaTime;

        if(currentAttackCooldown<=0)
        {

            currentAttackCooldown = maxAttackCooldown;
            Attack();
        }
    }
   
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

}
