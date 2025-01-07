using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkeletonCombatControls : FamiliarCombatControls
{
    [Header("Referecnes")]
    [Tooltip("REFERENCE to the pool of ranged projectiles the player has")]
    [SerializeField] protected MMMiniObjectPooler rangedProjectilePool;

    [Tooltip("REFERENCE to the pool of explosive projectiles the player has")]
    [SerializeField] protected MMMiniObjectPooler explosiveProjectilePool;
    public MMMiniObjectPooler specialAttackPool;
    [Tooltip("REFERENCE where I create ranged projectiles")]
    public GameObject rangedAttackSpawn;
    [Tooltip("REFERENCE to the particle effect that plays when I use my basic attack")]
    public ParticleSystem basicAttackSystem;

    public float rushCooldownMax;
    public float rushCooldown;
    public float rushSpawns;
    public float hexCooldownMax;
    public float hexCooldown;
    public float meleeCooldown;
    public float meleeCooldownMax;
    public float rangedCooldown;
    public float rangedCooldownMax;
    public float ultimateCooldownMax;
    public float ultimateCooldown;
    public float specialRange;
    public float specialHexTime;
    public float ultDurationMax;
    float ultDurationCurrent;
    bool isUlting;
    public GameObject ultObject;
    public GameObject normalObject;

    [Header("Audios")]
    public AudioClip meleeAudio;
    public AudioClip rangedAudio;
    public AudioClip ultimateAudio;
    [Header("Inputs")]
    public bool isHoldingMelee;
    public bool isHoldingRanged;
    bool isUltimateMode;
    public float ultimateAttackSpeed;
    public override void EnableActions(InputActionMap playerActionMap)
    {
        playerActionMap.FindAction("LBAction").performed += HexPressed;
        playerActionMap.FindAction("RBAction").performed += RushPressed;
        playerActionMap.FindAction("XAction").performed += MeleePressed;
        playerActionMap.FindAction("XAction").canceled += MeleeReleased;
        playerActionMap.FindAction("AAction").performed += RangedPressed;
        playerActionMap.FindAction("AAction").canceled += RangedReleased;
        playerActionMap.FindAction("LTAction").performed += UltimatePressed;
    }
    public override void CalculateDamage(float pAttack, float mAttack)
    {
        meleeDamage = pAttack * 1f;
        rangedDamage = mAttack * 1.5f;
        specialADamage = mAttack * 3f;
    }

    protected void Update()
    {
        if (TempPause.instance.isPaused)
            return;

        CoolDowns();
        if (isHoldingMelee)
        {
            if (meleeCooldown <= 0)
            {
                MeleeAttack();
            }
        }
        else if (isHoldingRanged)
        {
            if (rangedCooldown <= 0)
                RangedAttack();
        }
    }
    /// <summary>
    /// All the cooldown counters 
    /// </summary>
    private void CoolDowns()
    {
        

        if (isUlting)
        {
            if (meleeCooldown > 0)
                meleeCooldown -= (Time.deltaTime*ultimateAttackSpeed);
            if (rangedCooldown > 0)
                rangedCooldown -= (Time.deltaTime * ultimateAttackSpeed);
            if (hexCooldown > 0)
                hexCooldown -= (Time.deltaTime * ultimateAttackSpeed);
            if (rushCooldown > 0)
                rushCooldown -= (Time.deltaTime * ultimateAttackSpeed);
            if (ultDurationCurrent > 0)
                ultDurationCurrent -= Time.deltaTime;
            if (ultDurationCurrent <= 0)
            {
                SetUltimateMode(false);
            }
        }
        else
        {
            if (ultimateCooldown > 0)
                ultimateCooldown -= Time.deltaTime;
            if (meleeCooldown > 0)
                meleeCooldown -= Time.deltaTime;
            if (rangedCooldown > 0)
                rangedCooldown -= Time.deltaTime;
            if (hexCooldown > 0)
                hexCooldown -= Time.deltaTime;
            if (rushCooldown > 0)
                rushCooldown -= Time.deltaTime;
        }
    }
    public void MeleeAttack()
    {
        meleeCooldown = meleeCooldownMax;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * 2, 2);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy")
            {
                hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(meleeDamage, 0.5f, Element.Neutral, 0, this.gameObject, "Melee");
            }
        }
        basicAttackSystem.Play();
        MMSoundManager.Instance.PlaySound(meleeAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
         false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
         1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    /// <summary>
    /// Basic Ranged Attack
    /// </summary>
    public void RangedAttack()
    {
        rangedCooldown = rangedCooldownMax;
        GameObject objB = rangedProjectilePool.GetPooledGameObject();
        objB.transform.position = rangedAttackSpawn.transform.position;
        objB.transform.rotation = rangedAttackSpawn.transform.rotation;
        //add real damage here
        objB.GetComponent<MagicMissile>().damage = rangedDamage;
        if ( currentTarget!= null)
        {
            if(currentTarget.activeInHierarchy)
            objB.GetComponent<HomingAttack>().target = currentTarget.transform;
        }
        objB.SetActive(true);
        MMSoundManager.Instance.PlaySound(rangedAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
         false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
         1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    /// <summary>
    /// Start my ultimate attack, tranform into a larger guy
    /// </summary>
    private void UltimateAttack()
    {
        if (isBusy || CombatPlayerManager.instance.GetPlayer(0).isBusy)
            return;

        ultimateCooldown = ultimateCooldownMax;
        ultDurationCurrent = ultDurationMax;
        SetUltimateMode(true);
        MMSoundManager.Instance.PlaySound(ultimateAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
         false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
         1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    private void HexAttack()
    {
        hexCooldown = hexCooldownMax;
        Collider[] hitCollidersB = Physics.OverlapSphere(transform.position, specialRange);
        GameObject obj = specialAttackPool.GetPooledGameObject();
        obj.transform.position = transform.position + new Vector3(0, 0.1f, 0);
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);
        foreach (var hitCollider in hitCollidersB)
        {
            if (hitCollider.tag == "Enemy")
            {
                hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyStatusEffect(Status.Hexed, specialHexTime);
            }
        }
    }

    private void RushAttack()
    {
        rushCooldown = rushCooldownMax;
        StartCoroutine(RushRepeat());
    }
    IEnumerator RushRepeat()
    {
        for (int i = 0; i < rushSpawns; i++)
        {
            RushSpawn();
            yield return new WaitForSeconds(.1f);
        }
        
    }
    private void RushSpawn()
    {
        GameObject objB = explosiveProjectilePool.GetPooledGameObject();
        objB.transform.position = rangedAttackSpawn.transform.position;
        objB.transform.rotation = rangedAttackSpawn.transform.rotation;
        //add real damage here
        objB.GetComponent<FamiliarProjectile>().damage = specialADamage;
        if (currentTarget != null)
        {
            if (currentTarget.activeInHierarchy)
                objB.GetComponent<HomingAttack>().target = currentTarget.transform;
        }
        objB.SetActive(true);
    }
    public void SetUltimateMode(bool ult_)
    {
        if (ult_)
        {
            ultDurationCurrent = ultDurationMax;
            isUlting = true;
            normalObject.SetActive(false);
            ultObject.SetActive(true);
        }
        else
        {
            ultDurationCurrent = 0;
            isUlting = false;
            normalObject.SetActive(true);
            ultObject.SetActive(false);
        }
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
        if (ultimateCooldown <= 0)
            UltimateAttack();
    }
    private void HexPressed(InputAction.CallbackContext objdd)
    {
        if (isBusy)
            return;
        if(hexCooldown<=0)
        { HexAttack(); }
    }
    private void RushPressed(InputAction.CallbackContext objdd)
    {
        if (isBusy)
            return;
        if (rushCooldown <= 0)
        { RushAttack(); }
    }
}
