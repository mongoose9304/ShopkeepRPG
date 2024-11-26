using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlimeCombatControls : FamiliarCombatControls
{
    [Header("Referecnes")]
    public GameObject slamParticleEffect;
    bool isJumping;
    bool isSlaming;
    bool isUltimateJumping;
    public Animator anim;
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
    public float slamCooldownMax;
    public float slamCooldown;
    public float meleeCooldown;
    public float meleeCooldownMax;
    [Header("Inputs")]
    public bool isHoldingMelee;
    public override void EnableActions(InputActionMap playerActionMap)
    {
        playerActionMap.FindAction("RBAction").performed += SlamPressed;
        playerActionMap.FindAction("XAction").performed += MeleePressed;
        playerActionMap.FindAction("XAction").canceled += MeleeReleased;
    }
    public void SlamAttack()
    {
        slamCooldown = slamCooldownMax;
        isJumping = true;
        isSlaming = false;
        isBusy = true;
        jumpStart = transform.position.y;
        jumpEnd = jumpStart + jumpHeight;
        currentJumpPercentage = 1.0f;
    }
    public void MeleeAttack()
    {
        anim.SetTrigger("basicAttack");
        meleeCooldown = meleeCooldownMax;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy")
            {
                //add real damage later
                hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(40, 0, Element.Neutral, 0, this.gameObject);
            }
        }
    }
    protected  void Update()
    {
        if (TempPause.instance.isPaused)
            return;
       
        if (!isJumping)
        {
            CoolDowns();
            if(isHoldingMelee)
            {
                if(meleeCooldown<=0)
                {
                    MeleeAttack();
                }
            }
        }
        else
        {
            if (isUltimateJumping)
            {
               // player.transform.position = this.transform.position + new Vector3(-1, 0, 0);


            }
            if (!isSlaming)
            {
                if (currentJumpPercentage > lowestJumpPercentage)
                    currentJumpPercentage -= Time.deltaTime * 0.5f;
                transform.parent.position += jumpSpeed * Time.deltaTime * currentJumpPercentage;
                if (transform.parent.position.y >= jumpEnd)
                {
                    SlamDown();
                }
            }
            else
            {
                transform.parent.position -= jumpSpeed * Time.deltaTime * 2;
                if (transform.parent.position.y <= jumpStart)
                {
                    LandJump();
                }

            }
        }
    }
    private void CoolDowns()
    {
        slamCooldown -= Time.deltaTime;
        meleeCooldown -= Time.deltaTime;
    }
    /// <summary>
    /// The behavior when the slime hits the apex of thier jump
    /// </summary>
    private void SlamDown()
    {
        isSlaming = true;

    }
    /// <summary>
    /// The behavior when the slime lands thier jump (damage and particle effects)
    /// </summary>
    private void LandJump()
    {
        isSlaming = false;
        isJumping = false;
        isBusy = false;
        transform.parent.position = new Vector3(transform.parent.position.x, jumpStart, transform.parent.position.z);
        if (isUltimateJumping)
        {
            isUltimateJumping = false;
            Instantiate(slamParticleEffect, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            Instantiate(slamParticleEffect, transform.position + new Vector3(2, 0, 0), Quaternion.Euler(new Vector3(-90, 0, 0)));
            Instantiate(slamParticleEffect, transform.position + new Vector3(-2, 0, 0), Quaternion.Euler(new Vector3(-90, 0, 0)));
            Instantiate(slamParticleEffect, transform.position + new Vector3(0, 0, 2), Quaternion.Euler(new Vector3(-90, 0, 0)));
            Instantiate(slamParticleEffect, transform.position + new Vector3(0, 0, -2), Quaternion.Euler(new Vector3(-90, 0, 0)));
            Collider[] hitCollidersB = Physics.OverlapSphere(transform.position, slamRange);
            foreach (var hitCollider in hitCollidersB)
            {
                if (hitCollider.tag == "Enemy")
                {
                    //hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(UltimateSlamDamage(), 0, myElement, 0, this.gameObject);
                }
            }
            return;
        }

        Instantiate(slamParticleEffect, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, slamRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy")
            {
                //add actual element here
                hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(SlamDamage(), 0, Element.Water, 0, this.gameObject);
            }
        }
    }
    /// <summary>
    /// Calculates the damage of the slam
    /// </summary>
    private float SlamDamage()
    {
        //add actual damage here
        return 20 * 3;
    }
    private void SlamPressed(InputAction.CallbackContext objdd)
    {
        if (isBusy)
            return;
        if(slamCooldown<=0)
            SlamAttack();
    }
    private void MeleePressed(InputAction.CallbackContext objdd)
    {
        isHoldingMelee = true;
    }
    private void MeleeReleased(InputAction.CallbackContext objdd)
    {
        isHoldingMelee = false;
    }
}
