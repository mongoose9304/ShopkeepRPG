using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class SlimeEnemy : BasicEnemy
{
    [SerializeField] Vector3 jumpSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float lowestJumpPercentage;
    public float slamRange;
    float jumpStart;
     float jumpEnd;
    float currentJumpPercentage;
    float currentMoveSpeedPercent;
    bool isJumping;
    bool isSlaming;
    [SerializeField] GameObject slamParticleEffect;
    [SerializeField] MMF_Player JumpEffect;
    public SkinnedMeshRenderer rend;
    public override void Attack()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > attackDistance)
            return;
        canMove = false;
        isJumping = true ;
        isSlaming = false;
        agent.enabled = false;
        jumpStart = transform.position.y;
        jumpEnd = jumpStart + jumpHeight;
        currentJumpPercentage = 1.0f;
        JumpEffect.PlayFeedbacks();
    }

    private void LandJump()
    {
        canMove = true;
        isSlaming = false;
        isJumping = false;
        agent.enabled = true;
        EndAttack();
        Instantiate(slamParticleEffect, transform.position, Quaternion.Euler(new Vector3(-90,0,0)));
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, slamRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Player")
            {
                hitCollider.gameObject.GetComponent<CombatPlayerMovement>().TakeDamage(damage,0,myElement,0,this.gameObject,isMysticalDamage);
            }
            else if (hitCollider.tag == "Familiar")
            {
                hitCollider.gameObject.GetComponent<CombatFamiliar>().TakeDamage(damage, 0, myElement, 0, this.gameObject,isMysticalDamage);
            }
            else if (hitCollider.tag == "PlayerFamiliar")
            {
                hitCollider.gameObject.GetComponent<CombatCoopFamiliar>().TakeDamage(damage, 0, myElement, 0, this.gameObject, isMysticalDamage);
            }
            else if (hitCollider.tag == "Enemy")
            {
                if(CheckTeam(hitCollider.gameObject))
                    hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(damage, 0, myElement, 0, this.gameObject);
                
            }
            else if (hitCollider.tag == "Follower")
            {
                if (CheckTeam(hitCollider.gameObject))
                    hitCollider.gameObject.GetComponent<BasicFollower>().TakeDamage(damage, 0, myElement, 0, this.gameObject);

            }
        }
    }
    private void SlamDown()
    {
        isSlaming = true;

    }

    protected override void Update()
    {
        if (TempPause.instance.isPaused)
            return;
        if (!isJumping)
        {
            if (currentHitstun > 0)
            {
                CheckStun();
                return;
            }
            Move();
            WaitingToAttack();
        }
        else
        {
            if (!isSlaming)
            {
                if (currentJumpPercentage > lowestJumpPercentage)
                    currentJumpPercentage -= Time.deltaTime*0.5f;
                transform.position += jumpSpeed * Time.deltaTime*currentJumpPercentage;
                if (transform.position.y >= jumpEnd)
                {
                    SlamDown();
                }
            }
            else
            {
                transform.position -= jumpSpeed * Time.deltaTime*2;
                if (transform.position.y <= jumpStart)
                {
                    LandJump();
                }

            }
        }
    }
    protected override void OnEnable()
    {
        ResetEnemy();
        FindTarget();
        isJumping = false;
        isSlaming = false;
       if(rend)
        {
            rend.material.color = Color.white;
        }
    }
    public override void Move()
    {
        if (!agent.isActiveAndEnabled)
            return;
        if (canMove)
        {
            if (!target)
               target= CheckIfPlayerIsCloserThanFamiliar();
            if (!target.activeInHierarchy)
               target= CheckIfPlayerIsCloserThanFamiliar();
            agent.SetDestination(target.transform.position);
            agent.speed = moveSpeed * currentMoveSpeedPercent;
            currentMoveSpeedPercent -= Time.deltaTime;
            if (currentMoveSpeedPercent <= 0)
            {
                currentMoveSpeedPercent = 1;
            }
        }
        else
            agent.ResetPath();
    }
    
}
