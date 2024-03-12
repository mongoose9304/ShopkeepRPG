using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class SlimeEnemy : BasicEnemy
{
    [SerializeField] Vector3 jumpSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float lowestJumpPercentage;
    float jumpStart;
     float jumpEnd;
    float currentJumpPercentage;
    float currentMoveSpeedPercent;
    bool isJumping;
    bool isSlaming;
    [SerializeField] GameObject slamParticleEffect;
    [SerializeField] MMF_Player JumpEffect;
    public override void Attack()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
            return;
        canMove = false;
        isJumping = true ;
        isSlaming = false;
        agent.enabled = false;
        jumpStart = transform.position.y;
        jumpEnd = jumpStart + jumpHeight;
        currentJumpPercentage = 1.0f;
        GameObject obj = GetAvailableAttackIcon();
        obj.transform.position = transform.position;
        obj.SetActive(true);
       // JumpEffect.PlayFeedbacks();
    }

    private void LandJump()
    {
        canMove = true;
        isSlaming = false;
        isJumping = false;
        agent.enabled = true;
        EndAttack();
        Instantiate(slamParticleEffect, transform.position, Quaternion.Euler(new Vector3(-90,0,0)));
    }
    private void SlamDown()
    {
        isSlaming = true;

    }

    protected override void Update()
    {
        FadeDamageText();
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
    public override void Move()
    {
        if (canMove)
        {
            agent.SetDestination(player.transform.position);
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
