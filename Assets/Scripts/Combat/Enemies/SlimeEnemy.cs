using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : BasicEnemy
{
    [SerializeField] Vector3 jumpSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float lowestJumpPercentage;
    float jumpStart;
     float jumpEnd;
    float currentJumpPercentage;
    bool isJumping;
    bool isSlaming;
 
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
    }

    private void LandJump()
    {
        canMove = true;
        isSlaming = false;
        isJumping = false;
        agent.enabled = true;
        EndAttack();
    }
    private void SlamDown()
    {
        isSlaming = true;

    }

    protected override void Update()
    {
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
}
