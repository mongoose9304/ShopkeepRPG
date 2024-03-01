using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    //count for how long an enemy has been stunned, after this passes the max an enemy should be able to act regardless of if the player could stun them again
    [Header("CurrentValues")]
    public bool canMove;
    float currentTimeStunned;
    protected float currentHitstun;
    float currentAttackCooldown;
    float currentHealth;
    Element myWeakness;
    bool superArmor;
    [Header("References")]
    public GameObject stunIcon;
    public GameObject player;
    [SerializeField]protected NavMeshAgent agent;
    [SerializeField] List<GameObject> attackIcons;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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


//Check if the enemy is stunned and activate superarmor if it has been too much time stunned
    protected void CheckStun()
    {
        stunIcon.SetActive(true);
        currentHitstun -= Time.deltaTime;
        currentTimeStunned += Time.deltaTime;
        if(currentTimeStunned>=maxHitstun||currentHitstun<=0)
        {
            superArmor = true;
            currentTimeStunned = 0;
            currentHitstun = 0;
            stunIcon.SetActive(false);
        }


    }

    //Application of damage
    public void ApplyDamage(float damage_,float hitstun_,Element element_)
    {
       
        if (!superArmor)
            currentHitstun += hitstun_;
        if(element_==myWeakness&&element_!=Element.Neutral)
        {
            damage_ *= 1.5f;
        }
        currentHealth -= damage_;
    }
    public virtual void Attack()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
            return;
    }

    public void EndAttack()
    {
        superArmor = false;
        ResetAttackIcons();
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
   protected GameObject GetAvailableAttackIcon()
    {
        foreach(GameObject obj in attackIcons)
        {
            if(!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
    }
    protected void ResetAttackIcons()
    {
        foreach (GameObject obj in attackIcons)
        {
            obj.SetActive(false);
        }
      
    }

}
