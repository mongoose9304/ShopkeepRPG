using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFamiliar : CombatFamiliar
{
    [Header("Referecnes")]
    public GameObject slamParticleEffect;
    bool isJumping;
    bool canMove;
    bool isSlaming;
    bool isUltimateJumping;
    [Header("Stats")]
    public float lowestJumpPercentage;
    public float slamAttackDistance;
    public float AttackDistance;
    public float jumpHeight;
    public Vector3 jumpSpeed;
    public float slamRange;
    float currentJumpPercentage;
    float jumpStart;
    float jumpEnd;
  
    public override void Attack()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > AttackDistance)
            return;
        anim.SetTrigger("basicAttack");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy")
            {
                hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(PhysicalAtk, 0, Element.Neutral, 0, this.gameObject);
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
    public override void UltimateAttack()
    {
        if (player.GetComponent<CombatPlayerActions>().isBusy||ultimateAttackCooldowncurrent>0)
            return;
        isBusy = true;
        transform.position = player.transform.position+new Vector3(1,0,0);
        isUltimateJumping = true;
        player.GetComponent<CombatPlayerActions>().isBusy = true;
        canMove = false;
        isJumping = true;
        isSlaming = false;
        agent.enabled = false;
        jumpStart = transform.position.y;
        jumpEnd = jumpStart + (jumpHeight*1.25f);
        currentJumpPercentage = 1.0f;
        ultimateAttackCooldowncurrent = ultimateAttackCooldownMax;
    }
    protected override void Update()
    {
        if (TempPause.instance.isPaused)
            return;

        if (!isJumping)
        {
            FollowPlayer();
            EnemyDetection();
            WaitForAttacks();
            RegenHealth();
            if (agent.velocity != Vector3.zero)
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
            if(isUltimateJumping)
            {
                player.transform.position = this.transform.position + new Vector3(-1, 0, 0);

              
            }
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
    /// <summary>
    /// The behavior when the slime lands thier jump (damage and particle effects)
    /// </summary>
    private void LandJump()
    {
        canMove = true;
        isSlaming = false;
        isJumping = false;
        agent.enabled = true;
        if(isUltimateJumping)
        {
            isUltimateJumping = false;
            Instantiate(slamParticleEffect, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            Instantiate(slamParticleEffect, transform.position+new Vector3(2,0,0), Quaternion.Euler(new Vector3(-90, 0, 0)));
            Instantiate(slamParticleEffect, transform.position+new Vector3(-2,0,0), Quaternion.Euler(new Vector3(-90, 0, 0)));
            Instantiate(slamParticleEffect, transform.position+new Vector3(0,0,2), Quaternion.Euler(new Vector3(-90, 0, 0)));
            Instantiate(slamParticleEffect, transform.position+new Vector3(0,0,-2), Quaternion.Euler(new Vector3(-90, 0, 0)));
            Collider[] hitCollidersB = Physics.OverlapSphere(transform.position, slamRange);
            foreach (var hitCollider in hitCollidersB)
            {
                if (hitCollider.tag == "Enemy")
                {
                    hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(UltimateSlamDamage(), 0, myElement, 0, this.gameObject,"",false);
                }
            }
            isBusy = false;
            return;
        }

        Instantiate(slamParticleEffect, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, slamRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy")
            {
                hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(SlamDamage(), 0, myElement, 0, this.gameObject,"",false);
            }
        }
    }
    /// <summary>
    /// The behavior when the slime hits the apex of thier jump
    /// </summary>
    private void SlamDown()
    {
        isSlaming = true;

    }
    /// <summary>
    /// Calculates the damage of the slam
    /// </summary>
    private float SlamDamage()
    {
        return PhysicalAtk * 3;
    }
    /// <summary>
    /// Calculates the damage of the ultimate slam
    /// </summary>
    private float UltimateSlamDamage()
    {
        return PhysicalAtk * 9;
    }
    public override void TakeDamage(float damage_, float hitstun_, Element element_, float knockBack_ = 0, GameObject knockBackObject = null, bool isMystical = false)
    {
        if (isUltimateJumping)
            return;
        base.TakeDamage(damage_, hitstun_, element_, knockBack_, knockBackObject,isMystical);
    }

    /// <summary>
    /// Cooldowns for attacks
    /// </summary>
    private void WaitForAttacks()
    {
       
        AttackCooldowncurrent -= Time.deltaTime;
        specialAttackCooldowncurrent -= Time.deltaTime;
        ultimateAttackCooldowncurrent -= Time.deltaTime;
        if (!target)
            return;
        
        if (specialAttackCooldowncurrent <= 0)
        {

            specialAttackCooldowncurrent = specialAttackCooldownMax;
            SpecialAttack();
        }
        else if (AttackCooldowncurrent <= 0)
        {

            AttackCooldowncurrent = AttackCooldownMax;
            Attack();
        }
    }
}
