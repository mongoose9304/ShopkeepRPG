using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public float Health;
    float currentHitstun;
    //count for how long an enemy has been stunned, after this passes the max an enemy should be able to act regardless of if the player could stun them again
    float currentTimeStunned;
    public float maxHitstun;
    public float maxAttackCooldown;
    float currentAttackCooldown;
    bool superArmor;
    public GameObject stunIcon;
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (currentHitstun > 0)
        {
            CheckStun();
            return;
        }
        Move();
        WaitingToAttack();
        
    }




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

    public void ApplyDamage(float damage_,float hitstun_,Element element_)
    {
        if (!superArmor)
            currentHitstun += hitstun_;
    }
    public void Attack()
    {
    
    }

    public void EndAttack()
    {
        superArmor = false;
    }
    public virtual void Move()
    {

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
}
