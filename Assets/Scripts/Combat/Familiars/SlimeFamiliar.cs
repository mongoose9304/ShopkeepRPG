using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFamiliar : CombatFamiliar
{
    bool isJumping;
    bool canMove;
    bool isSlaming;
    float jumpStart;
    float jumpEnd;
    float currentJumpPercentage;
    public float lowestJumpPercentage;
    public float slamAttackDistance;
    public float AttackDistance;
    public float jumpHeight;
    public Vector3 jumpSpeed;
    public GameObject slamParticleEffect;
    public float slamRange;
  
    public override void Attack()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > AttackDistance&&target)
            return;
        anim.SetTrigger("basicAttack");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy")
            {
                hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(monsterData.CalculateDamage(), 0, monsterData.element, 0, this.gameObject);
            }
        }
    }
    public override void SpecialAttack()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > slamAttackDistance && target)
            return;
        canMove = false;
        isJumping = true;
        isSlaming = false;
        agent.enabled = false;
        jumpStart = transform.position.y;
        jumpEnd = jumpStart + jumpHeight;
        currentJumpPercentage = 1.0f;
    }
    protected override void Update()
    {
      

        if (!isJumping)
        {
            FollowPlayer();
            EnemyDetection();
            WaitToSpecialAttack();
            WaitToAttack();
           if(agent.velocity != Vector3.zero)
            {
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }
        }
        else
        {
            anim.SetBool("isWalking", false);
            anim.ResetTrigger("basicAttack");
            if (!isSlaming)
            {
                if (currentJumpPercentage > lowestJumpPercentage)
                    currentJumpPercentage -= Time.deltaTime * 0.5f;
                transform.position += jumpSpeed * Time.deltaTime * currentJumpPercentage;
                if (transform.position.y >= jumpEnd)
                {
                    SlamDown();
                }
            }
            else
            {
                transform.position -= jumpSpeed * Time.deltaTime * 2;
                if (transform.position.y <= jumpStart)
                {
                    LandJump();
                }

            }
        }
    }
    private void LandJump()
    {
        canMove = true;
        isSlaming = false;
        isJumping = false;
        agent.enabled = true;
        Instantiate(slamParticleEffect, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, slamRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy")
            {
                hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(SlamDamage(), 0, monsterData.element, 0, this.gameObject);
            }
        }
    }
    private void SlamDown()
    {
        isSlaming = true;

    }
    private float SlamDamage()
    {
        return monsterData.CalculateDamage() * 3;
    }
    private void WaitToSpecialAttack()
    {
        specialAttackCooldowncurrent -= Time.deltaTime;

        if (specialAttackCooldowncurrent <= 0)
        {

            specialAttackCooldowncurrent = specialAttackCooldownMax;
            SpecialAttack();
        }
    }
    private void WaitToAttack()
    {
        AttackCooldowncurrent -= Time.deltaTime;

        if (AttackCooldowncurrent <= 0)
        {

            AttackCooldowncurrent = AttackCooldownMax;
            Attack();
        }
    }
}
