using Blobcreate.ProjectileToolkit;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlimeCombatControls : FamiliarCombatControls
{
    [Header("Audios")]
    public AudioClip jumpAudio;
    public AudioClip slamAudio;
    public AudioClip meleeAudio;
    public AudioClip rangedAudio;
    [Header("Referecnes")]
    public GameObject slamParticleEffect;
    bool isJumping;
    bool isSlaming;
    bool isUltimateJumping;
    public Animator anim;
    [Tooltip("REFERENCE to the pool of ranged projectiles the player has")]
    [SerializeField] protected MMMiniObjectPooler rangedProjectilePool;
    public GameObject rangedAttackSpawn;
    public ParticleSystem basicAttackSystem;
    public SlimeWhirlwind myWhirlWindObject;
    public Vector3 spinVector;
    public Vector3 spinPosStart;
    public Vector3 spinPosEnd;
    public Quaternion spinStart;
    public float spinAngle;
    [Header("Stats")]
    public float lowestJumpPercentage;
    public float slamAttackDistance;
    public float AttackDistance;
    public float jumpHeight;
    public Vector3 jumpSpeed;
    public float slamRange;
    float currentJumpPercentage;
    public float spinTimeMax;
    float currentSpinTime;
    float jumpStart;
    float jumpEnd;
    public float slamCooldownMax;
    public float slamCooldown;
    public float whirlwindCooldownMax;
    public float whirlwindCooldown;
    public float meleeCooldown;
    public float meleeCooldownMax;
    public float rangedCooldown;
    public float rangedCooldownMax;
    public float ultimateCooldownMax;
    public float ultimateCooldown;
    [Header("Inputs")]
    public bool isHoldingMelee;
    public bool isHoldingRanged;
    private void OnEnable()
    {
        currentSpinTime = 0;
    }
    public override void EnableActions(InputActionMap playerActionMap)
    {
        playerActionMap.FindAction("LBAction").performed += SlamPressed;
        playerActionMap.FindAction("RBAction").performed += WhirlWindPressed;
        playerActionMap.FindAction("XAction").performed += MeleePressed;
        playerActionMap.FindAction("XAction").canceled += MeleeReleased;
        playerActionMap.FindAction("AAction").performed += RangedPressed;
        playerActionMap.FindAction("AAction").canceled += RangedReleased;
        playerActionMap.FindAction("LTAction").performed += UltimatePressed;
    }
    public override void CalculateDamage(float pAttack, float mAttack)
    {
        meleeDamage = pAttack * 1.5f;
        rangedDamage = mAttack * 1.5f;
        specialADamage = pAttack * 5f;
        specialBDamage = pAttack;
        ultimateDamage = pAttack * 15f;
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
        EndWhirlWind();
        MMSoundManager.Instance.PlaySound(jumpAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
         false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
         1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    public void WhirlWindAttack()
    {
        currentSpinTime = spinTimeMax;
        myWhirlWindObject.gameObject.SetActive(true);
        myWhirlWindObject.damage = specialBDamage;
        whirlwindCooldown = whirlwindCooldownMax;
        isControllingRotation = true;
        anim.transform.rotation = spinStart;
        anim.transform.localPosition = spinPosStart;
    }
    private void EndWhirlWind()
    {
        currentSpinTime = 0;
        myWhirlWindObject.gameObject.SetActive(false);
        isControllingRotation = false;
        anim.transform.rotation = new Quaternion(0, 0, 0, 0);
        anim.transform.localPosition = spinPosEnd;

    }
    private void Spin()
    {
        transform.parent.Rotate(spinVector, spinAngle);
    }
    public void MeleeAttack()
    {
        anim.SetTrigger("basicAttack");
        meleeCooldown = meleeCooldownMax;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position+transform.forward*2, 2);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy")
            {
                //add real damage later
                hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(meleeDamage, 0.5f, Element.Neutral, 0, this.gameObject,"Melee");
            }
        }
        basicAttackSystem.Play();
        MMSoundManager.Instance.PlaySound(meleeAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
         false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
         1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    public void RangedAttack(GameObject target_=null)
    {
        rangedCooldown = rangedCooldownMax;
        anim.SetTrigger("basicAttack");
        GameObject objB = rangedProjectilePool.GetPooledGameObject();
        objB.transform.position = rangedAttackSpawn.transform.position;
        //add real damage here
        objB.GetComponent<FamiliarProjectile>().damage = rangedDamage;
        objB.SetActive(true);
        objB.GetComponent<Rigidbody>().velocity = Vector3.zero;
        if(target_!=null)
        {
        objB.GetComponent<Rigidbody>().AddForce(Projectile.VelocityByA(objB.transform.position, target_.transform.position, -0.1f), ForceMode.VelocityChange);
        }
        else
        {
            objB.GetComponent<Rigidbody>().AddForce(Projectile.VelocityByA(objB.transform.position,transform.position+transform.forward*5, -0.1f), ForceMode.VelocityChange);
        }
        MMSoundManager.Instance.PlaySound(rangedAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
         false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
         1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    protected  void Update()
    {
        if (TempPause.instance.isPaused)
            return;
       
        if (!isJumping)
        {
            if(currentSpinTime>0)
            {
                currentSpinTime -= Time.deltaTime;
                Spin();
                if(currentSpinTime<=0)
                {
                    EndWhirlWind();
                }
                return;
            }
            CoolDowns();
            if(isHoldingMelee)
            {
                if(meleeCooldown<=0)
                {
                    MeleeAttack();
                }
            }
            else if(isHoldingRanged)
            {
                if(rangedCooldown<=0)
                RangedAttack();
            }
        }
        else
        {
            if (isUltimateJumping)
            {
                CombatPlayerManager.instance.GetPlayer(0).transform.position = this.transform.position + new Vector3(1, 0, 0);


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
        if(slamCooldown>0)
        slamCooldown -= Time.deltaTime;
        if(meleeCooldown>0)
        meleeCooldown -= Time.deltaTime;
        if(rangedCooldown>0)
        rangedCooldown -= Time.deltaTime;
        if (ultimateCooldown > 0)
            ultimateCooldown -= Time.deltaTime;
        if (whirlwindCooldown > 0)
            whirlwindCooldown -= Time.deltaTime;
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
        MMSoundManager.Instance.PlaySound(slamAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
         false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
         1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
        isSlaming = false;
        isJumping = false;
        isBusy = false;
        transform.parent.position = new Vector3(transform.parent.position.x, jumpStart, transform.parent.position.z);
        CombatPlayerManager.instance.GetPlayer(0).transform.position=this.transform.position + new Vector3(1, 0, 0);
        if (isUltimateJumping)
        {
            isUltimateJumping = false;
            damageImmune = false;
            bothPlayersBusy = false;
            Instantiate(slamParticleEffect, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            Instantiate(slamParticleEffect, transform.position + new Vector3(2, 0, 0), Quaternion.Euler(new Vector3(-90, 0, 0)));
            Instantiate(slamParticleEffect, transform.position + new Vector3(-2, 0, 0), Quaternion.Euler(new Vector3(-90, 0, 0)));
            Instantiate(slamParticleEffect, transform.position + new Vector3(0, 0, 2), Quaternion.Euler(new Vector3(-90, 0, 0)));
            Instantiate(slamParticleEffect, transform.position + new Vector3(0, 0, -2), Quaternion.Euler(new Vector3(-90, 0, 0)));
            Collider[] hitCollidersB = Physics.OverlapSphere(transform.position, slamRange*1.5f);
            foreach (var hitCollider in hitCollidersB)
            {
                if (hitCollider.tag == "Enemy")
                {
                    hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(ultimateDamage, 1.5f, Element.Neutral, 0, this.gameObject);
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
                hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(specialADamage, 1.5f, Element.Water, 0, this.gameObject, "Special");
            }
        }
    }
    private void UltimateAttack()
    {
        if (isBusy||CombatPlayerManager.instance.GetPlayer(0).isBusy)
            return;

        isBusy = true;
        bothPlayersBusy = true;
        CombatPlayerManager.instance.GetPlayer(0).transform.position = transform.position + new Vector3(1, 0, 0);
        isUltimateJumping = true;
        isJumping = true;
        isSlaming = false;
        jumpStart = transform.position.y;
        jumpEnd = jumpStart + (jumpHeight * 1.25f);
        currentJumpPercentage = 1.0f;
        ultimateCooldown = ultimateCooldownMax;
        damageImmune = true;
        EndWhirlWind();
        MMSoundManager.Instance.PlaySound(jumpAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
         false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
         1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    private void SlamPressed(InputAction.CallbackContext objdd)
    {
        if (isBusy)
            return;
        if(slamCooldown<=0)
            SlamAttack();
    }
    private void WhirlWindPressed(InputAction.CallbackContext objdd)
    {
        if (isBusy)
            return;
        if (whirlwindCooldown <= 0)
            WhirlWindAttack();
    }
    private void MeleePressed(InputAction.CallbackContext objdd)
    {
        isHoldingMelee = true;
    }
    private void MeleeReleased(InputAction.CallbackContext objdd)
    {
        isHoldingMelee = false;
    }
    private void RangedPressed(InputAction.CallbackContext objdd)
    {
        isHoldingRanged = true;
    }
    private void RangedReleased(InputAction.CallbackContext objdd)
    {
        isHoldingRanged = false;
    }
    private void UltimatePressed(InputAction.CallbackContext objdd)
    {
        if(ultimateCooldown<=0)
        UltimateAttack();
    }
}
