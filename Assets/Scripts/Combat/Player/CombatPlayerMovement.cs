using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using UnityEngine.InputSystem;
public class CombatPlayerMovement : MonoBehaviour
{
    //FFYL stats
    [SerializeField] bool isInSaveYourSoulMode;
    [SerializeField] bool isDying;
    [SerializeField]float maxSaveYourSoulTime;
    [SerializeField]float currentSaveYourSoulTime;
    [SerializeField] List<GameObject> saveYourSoulObjects=new List<GameObject>();
    public int timesYouHaveDied;
    [SerializeField] float sosDecreasePerDeath;

    public StatBlock myStats;
    public float maxdashCoolDown;
    public float moveSpeed;
    public float moveSpeedModifier;
    public float dashDistance;
    public float dashCoolDown;
    public float dashTime;
    public float dashCost;
    public bool isDashing;
    public float minMeleeDistance;
    Vector3 moveInput;
    Vector3 newInput;
   [SerializeField] Vector3 dashStartPos;
    Rigidbody rb;
    float timeBeforePlayerCanMoveAfterFallingOffPlatform;
   [SerializeField] LayerMask wallMask;
   [SerializeField] LayerMask groundMask;
    [SerializeField] GameObject dashEffect;
    public CombatPlayerActions combatActions;
    //targeting and lock on
    [SerializeField] GameObject currentTarget;
    [SerializeField] bool hardLockOn;
    [SerializeField] GameObject lockOnIcon;
    [SerializeField] Transform lockOnCheckPosition;
    
    
    [SerializeField] string enemyTag;
    [SerializeField] float minDistanceBetweenRetargets;
    [SerializeField] float MaxLockOnDistance;
   
    //Stats Calculated based on Stat block
    public float maxHealth;
    public float maxMana;
    public float PhysicalAtk;
    public float MysticalAtk;
    public float PhysicalDef;
    public float MysticalDef;
    public float HealthRegenPercent;
    public float ManaRegenPercent;
    public float playerLuck;
    public List<EquipModifier> equipModifiers = new List<EquipModifier>();
    public List<EquipModifier> externalModifiers = new List<EquipModifier>();
    //Equipment
    public PlayerEquiptmentHolder myEquiptment;
    //SkillTree Abilities and Effects
    [SerializeField] SavedTalents myTalents;
    //Undead
    public PlayerSkeletonMaster mySkeltonMaster;
    [SerializeField] GameObject skullHead;
    public bool extraLife;
    bool hasUsedExtraLife;


    public float maxManaRechargeDelay;
    public float manaRechargeRate;
    [SerializeField] float currentHealth;
    float currentMana;
    float currentManaRechargeDelay;
    private GameObject tempObj;
    public GameObject levelUpEffect;
    //guard settings
    public GameObject guardObject;
    public float maxGuardTime;
    float currentGuardTime;
    public float secondsToRechargeGuardTime;
    public float guardChargeDelayMax;
    float guardChargeDelay;
    bool isGuarding;

    [Header("Interactions")]
    [Tooltip("All the objects the player is currently in range to interact with")]
    public List<GameObject> myInteractableObjects = new List<GameObject>();
    [Tooltip("The object the player is currently locked onto")]
    [SerializeField] GameObject interactableObjectTarget;
    [Tooltip("REFERENCE to gameobject used to show what you are locked onto")]
    [SerializeField] GameObject interactableObjectLockOnObject;

    [Header("UI")]
    public MMProgressBar healthBar;
    public MMProgressBar shieldBar;
    public MMProgressBar manaBar;
    public MMProgressBar familiarHealthBar;
    public AudioClip dashAudio;
    [Header("Inputs")]
    public InputActionMap playerActionMap;
    private InputAction movement;
    private bool InteractHeld;
    //used to take control of object when player 1 joins
    public void SetUpControls(PlayerInput myInput)
    {
        playerActionMap = myInput.actions.FindActionMap("Player");
        movement= playerActionMap.FindAction("Movement");
        playerActionMap.FindAction("Dash").performed+= OnDash;
        playerActionMap.FindAction("StartAction").performed+= OnPause;
        playerActionMap.FindAction("YAction").performed+= InteractPressed;
        playerActionMap.FindAction("YAction").canceled+= InteractReleased;
        combatActions.EnableActions();


        playerActionMap.Enable();
    }

    private void OnDisable()
    {
        if(playerActionMap!=null)
        {
            playerActionMap.FindAction("Dash").performed -= OnDash;
            playerActionMap.FindAction("StartAction").performed -= OnPause;
            playerActionMap.FindAction("YAction").performed -= InteractPressed;
            playerActionMap.FindAction("YAction").canceled-= InteractReleased;
            playerActionMap.Disable();
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        AddAllEquipmentMods();
        CalculateAllModifiers();
        currentHealth = maxHealth;
        currentMana = maxMana;
        healthBar.SetBar01(currentHealth / maxHealth);

    }
    // Update is called once per frame

    void Update()
    {
        if (TempPause.instance.isPaused)
            return;
        if (isDying)
            return;
        if(isInSaveYourSoulMode)
        {
            SaveYourSoulUpdate();
        }
        ChargeMana();
        RegenHealth();
        ChargeGuardTime();
        if (combatActions.isBusy)
            return;
        GetClosestInteractableObject();
        if (InteractHeld)
            InteractAction();
        CheckForSoftLockOn();
        GetInput();
        if (combatActions.isUsingBasicAttackRanged||combatActions.isUsingBasicAttackMelee)
        {
            if(combatActions.isUsingBasicAttackMelee)
            {
                moveInput /= 1.2f;
                TryGuarding();
               
            }
            else if(combatActions.isUsingBasicAttackRanged)
            {
                moveInput /= 1.75f;
            }

        }
        if(!combatActions.isUsingBasicAttackMelee)
        {
            StopGuarding();
        }
     moveInput=PreventGoingThroughWalls(moveInput);
       
        if (!isDashing)
        {


            if (dashCoolDown > 0)
                dashCoolDown -= Time.deltaTime;


            if (timeBeforePlayerCanMoveAfterFallingOffPlatform <= 0)
            {
              transform.position = transform.position + PreventFalling() * moveSpeed * moveSpeedModifier * Time.deltaTime;
            }
            else
                timeBeforePlayerCanMoveAfterFallingOffPlatform -= Time.deltaTime;
            if (moveInput != Vector3.zero)
                transform.forward = moveInput;
            LookAtCurrentTarget();
        }
        else
        {
            if (dashTime > 0)
            {
                dashTime -= Time.deltaTime;
                if (CheckForWallHit())
                {
                    dashTime = 0;

                }
                if (dashTime <= 0)
                {
                    isDashing = false;
                    GroundCheck();
                    return;
                }
                Vector3 temp = transform.position + (transform.forward * moveSpeed * Time.deltaTime * dashDistance);
                // transform.position = Vector3.SmoothDamp(transform.position, PreventGoingThroughWalls(temp), ref velocity, dampModifier);
                transform.position = PreventGoingThroughWalls(temp);


            }




        }
    }
    private void OnDash(InputAction.CallbackContext obj)
    {
        if (TempPause.instance.isPaused)
            return;
        if (dashCoolDown <= 0)
        {
            dashCoolDown = maxdashCoolDown;
            DashAction();
        }
    }
    private void OnPause(InputAction.CallbackContext obj)
    {
        if(TempPause.instance&&!combatActions.isBusy)
        {
            TempPause.instance.TogglePause();
        }
    }
    void GetInput()
    {
        moveInput = new Vector3(movement.ReadValue<Vector2>().x, 0, movement.ReadValue<Vector2>().y);
    }
    private void DashAction()
    {
        if (currentMana < dashCost)
            return;
       
            dashStartPos = transform.position;
        UseMana(dashCost);
        if (moveInput != Vector3.zero)
            transform.forward = moveInput;
        isDashing = true;
        dashTime = 0.2f;
        Instantiate(dashEffect, transform.position, transform.rotation);
        MMSoundManager.Instance.PlaySound(dashAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
          false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.9f, 1.1f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
          1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    private Vector3 PreventFalling()
    {
        // Stop walking
        var dir = transform.TransformDirection(Vector3.down);
        newInput = moveInput;
        // Up

        if (!Physics.Raycast(transform.position - new Vector3(0f, 0f, 1), dir, 10))
            if (newInput.z < 0)
                newInput.z = 0;
        // Down
        if (!Physics.Raycast(transform.position - new Vector3(0f, 0f, -1), dir, 10))
            if (newInput.z > 0)
                newInput.z = 0;
        //Left
        if (!Physics.Raycast(transform.position - new Vector3(-1, 0f, 0f), dir, 10))
            if (newInput.x > 0)
                newInput.x = 0;
        //Right
        if (!Physics.Raycast(transform.position - new Vector3(1, 0f, 0f), dir, 10))
            if (newInput.x < 0)
                newInput.x = 0;
        return newInput;
    }
    private Vector3 PreventGoingThroughWalls(Vector3 temp_)
    {

        var dir = transform.TransformDirection(Vector3.down);
        newInput = temp_;
        // Down
        
        if (Physics.Raycast(transform.position + new Vector3(0f, 5.0f, -0.5f), dir, 15, wallMask))
            if (newInput.z < 0)
                newInput.z = 0;
        // Up
        
        if (Physics.Raycast(transform.position + new Vector3(0f, 5.0f, 0.5f), dir, 15, wallMask))
            if (newInput.z > 0)
                newInput.z = 0;
        //Left
       
        if (Physics.Raycast(transform.position + new Vector3(0.5f, 5.0f, 0f), dir, 15, wallMask))
            if (newInput.x > 0)
                newInput.x = 0;
        //Right
       
        if (Physics.Raycast(transform.position + new Vector3(-0.5f, 5.0f, 0f), dir, 15, wallMask))
            if (newInput.x < 0)
                newInput.x = 0;
        return newInput;


    }
    private bool CheckForWallHit()
    {

        var dir = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, dir, 1.0f, wallMask))
            return true;
        dir = transform.TransformDirection(Vector3.right);
        if (Physics.Raycast(transform.position, dir, 1.0f, wallMask))
            return true;
        dir = transform.TransformDirection(Vector3.left);
        if (Physics.Raycast(transform.position, dir, 1.0f, wallMask))
            return true;
        return false;


    }
    private void GroundCheck()
    {
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 10,groundMask))
        {
            // transform.position = new Vector3.(0, 0.66f, 0);
            transform.position = dashStartPos;
            moveInput = Vector3.zero;
            timeBeforePlayerCanMoveAfterFallingOffPlatform = 0.1f;
        }
    }

    void CheckForSoftLockOn()
    {
        if (hardLockOn||EnemyManager.instance.currentEnemiesList.Count==0)
            return;

        if (!currentTarget)
            currentTarget = EnemyManager.instance.currentEnemiesList[0];

       foreach(GameObject obj in EnemyManager.instance.currentEnemiesList)
        {
            if (!obj.activeInHierarchy)
                continue;
            if (Vector3.Distance(lockOnCheckPosition.position, obj.transform.position) < Vector3.Distance(transform.position, currentTarget.transform.position)-minDistanceBetweenRetargets)
                currentTarget = obj;
        }
        if (Vector3.Distance(lockOnCheckPosition.position, currentTarget.transform.position) > MaxLockOnDistance)
            currentTarget = null;

        if(currentTarget)
        {
            lockOnIcon.transform.position = currentTarget.transform.position;
            lockOnIcon.SetActive(true);
            if (!currentTarget.activeInHierarchy)
            {
                currentTarget = null;
                lockOnIcon.SetActive(false);
            }
        }
        else
        {

            lockOnIcon.SetActive(false);
        }
       

    }
    void LookAtCurrentTarget()
    {
        if (!currentTarget)
            return;
       
        transform.LookAt(currentTarget.transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    
   
    public GameObject GetCurrentTarget()
    {
        return currentTarget;
    }
    public void TakeDamage(float damage_,float hitstun_, Element element_, float knockBack_ = 0, GameObject knockBackObject = null,bool isMystical=false)
    {
        if(isInSaveYourSoulMode){ return; }
        if (isGuarding) { return; }
        float newDamage = damage_;
        if(isMystical)
        {
            newDamage -= MysticalDef;
        }
        else
        {
            newDamage -= PhysicalDef;
        }
        if (newDamage < damage_ * 0.05f)
            newDamage = damage_ * 0.05f;
        currentHealth -= newDamage;
        if (currentHealth <= 0)
        {
            Death();
            return;
        }
        healthBar.UpdateBar01(currentHealth/maxHealth);
       
    }
    public void HealthPickup(float amount_)
    {
        
       
            currentHealth += (amount_*maxHealth);
            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
            healthBar.UpdateBar01(currentHealth / maxHealth);
        
    }
    public void ManaPickup(float amount_)
    {

            currentMana += (amount_ * maxMana);
            if (currentMana >= maxMana)
            {
            currentMana = maxMana;
            }
        manaBar.UpdateBar01(currentMana / maxMana);

    }
    public void Death()
    {
        if(!isInSaveYourSoulMode)
        {
            isInSaveYourSoulMode = true;
            currentSaveYourSoulTime = maxSaveYourSoulTime-timesYouHaveDied*sosDecreasePerDeath;
            if (currentSaveYourSoulTime <= maxSaveYourSoulTime / 4)
                currentSaveYourSoulTime = maxSaveYourSoulTime / 4;
            currentMana = maxMana;
            foreach(GameObject obj in saveYourSoulObjects)
            {
                obj.SetActive(true);
            }
            timesYouHaveDied += 1;
        }
    }
    public void TryGuarding()
    {
        guardChargeDelay = guardChargeDelayMax;
        if(currentGuardTime>0)
        {
            currentGuardTime -= Time.deltaTime;
            shieldBar.SetBar01(currentGuardTime/maxGuardTime);
            guardObject.SetActive(true);
            isGuarding = true;
        }
        else
        {
            StopGuarding();
        }
        
    }
    public void StopGuarding()
    {
        guardObject.SetActive(false);
        isGuarding = false;
    }
    private void ChargeGuardTime()
    {
        if (combatActions.isUsingBasicAttackMelee)
        {
            return;

        }
        if(guardChargeDelay>0)
        {
            guardChargeDelay -= Time.deltaTime;
            return;
        }
        currentGuardTime += Time.deltaTime*(maxGuardTime / secondsToRechargeGuardTime);
        if (currentGuardTime > maxGuardTime)
            currentGuardTime = maxGuardTime;
        shieldBar.SetBar01(currentGuardTime / maxGuardTime);
    }
    public void TrueDeath()
    {
        if(extraLife)
        {
            if(!hasUsedExtraLife)
            {
                hasUsedExtraLife = true;
                UndeadExtraLife();
                return;
            }
        }
        isDying = true;
        foreach (GameObject obj in saveYourSoulObjects)
        {
            obj.SetActive(false);
        }
        DungeonManager.instance.WinLevel(true);
    }
    private void ExitSaveYourSoul()
    {
        if (isDying)
            return;
        isInSaveYourSoulMode = false;
        float x = currentSaveYourSoulTime / maxSaveYourSoulTime;
        if (x <= .5)
            x = .5f;

        currentHealth = maxHealth * x;
        currentMana = maxMana * x;
        healthBar.SetBar01(currentHealth / maxHealth);
        manaBar.SetBar01(currentMana / maxMana);
        foreach (GameObject obj in saveYourSoulObjects)
        {
            obj.SetActive(false);
        }
    }
    private void SaveYourSoulUpdate()
    {
        currentSaveYourSoulTime -= Time.deltaTime;
        healthBar.SetBar01(currentSaveYourSoulTime / maxSaveYourSoulTime);
        if(currentSaveYourSoulTime<=0)
        {
            TrueDeath();
        }
    }
    public void GetAKill()
    {
        if (isInSaveYourSoulMode)
        {
            ExitSaveYourSoul();
        }
    }
    public float GetCurrentMana()
    {
        return currentMana;
    }
    public void UseMana(float manaToUse_)
    {
        currentMana -= manaToUse_;
        manaBar.UpdateBar01(currentMana / maxMana);
        currentManaRechargeDelay = maxManaRechargeDelay;

    }
    public void UpdateFamiliarHealth(float health_)
    {
        familiarHealthBar.UpdateBar01(health_);
    }
    public void SetFamiliarHealth(float health_)
    {
        familiarHealthBar.SetBar01(health_);
    }
    private void ChargeMana()
    {
        if (currentManaRechargeDelay > 0)
        {
            currentManaRechargeDelay -= Time.deltaTime;
            return;
        }
      
        currentMana += manaRechargeRate * Time.deltaTime;
        if(ManaRegenPercent!=0)
        {
            currentMana += ManaRegenPercent*maxMana * Time.deltaTime;
        }
        if (currentMana >= maxMana)
            currentMana = maxMana;
        manaBar.SetBar01(currentMana / maxMana);
    }
    private void RegenHealth()
    {
        if (HealthRegenPercent == 0)
            return;
        currentHealth += HealthRegenPercent*maxHealth * Time.deltaTime;

        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;
        healthBar.SetBar01(currentHealth / maxHealth);

    }
    private void CalculateStats()
    {
        maxHealth = (myStats.Vitality * 5);
        maxMana = (myStats.Soul * 5);
        PhysicalAtk = (myStats.PhysicalProwess);
        MysticalAtk = (myStats.MysticalProwess);
        PhysicalDef = (myStats.PhysicalDefense);
        MysticalDef = (myStats.MysticalDefense);
        playerLuck = (myStats.Luck);
        HealthRegenPercent = 0;
        ManaRegenPercent = 0;
        combatActions.attackSpeedMod = 1;
        combatActions.fireRateMod = 1;
        combatActions.lifeStealPercent = 0;
    }
    public void CalculateAllModifiers()
    {
        CalculateStats();
        SkillTreeEffects();
        List<EquipModifier> mods_ = new List<EquipModifier>();
        for(int i=0;i< equipModifiers.Count;i++)
        {
            if(!equipModifiers[i].isMultiplicative)
            {
                mods_.Insert(0, equipModifiers[i]);
            }
            else
            {
                mods_.Add(equipModifiers[i]);
            }
        }
        for (int i = 0; i < externalModifiers.Count; i++)
        {
            if (!externalModifiers[i].isMultiplicative)
            {
                mods_.Insert(0, externalModifiers[i]);
            }
            else
            {
                mods_.Add(externalModifiers[i]);
            }
        }


        for (int i = 0; i < mods_.Count; i++)
        {
            ApplyModifier(mods_[i]);
        }
        if(currentHealth>maxHealth)
            currentHealth = maxHealth;
        if (currentMana > maxMana)
            currentMana = maxMana;
        healthBar.SetBar01(currentHealth / maxHealth);
        manaBar.SetBar01(currentMana / maxMana);
        combatActions.SetSpecialDamages(PhysicalAtk,MysticalAtk);
        combatActions.SetStats(PhysicalAtk, MysticalAtk, myEquiptment.RangedWeapon.myElement, myEquiptment.MeleeWeapon.myElement);
        //mySkeltonMaster.SetStats(maxHealth, PhysicalAtk, MysticalAtk, PhysicalDef, MysticalDef);
        mySkeltonMaster.SetStatsBasedOnPlayerLevel(myStats.Level);
    }
    //public List<EquipmentStatBlock> Rings = new List<EquipmentStatBlock>();
    //public EquipmentStatBlock MeleeWeapon;
   // public EquipmentStatBlock RangedWeapon;
    //public EquipmentStatBlock Armor;
    private void AddAllEquipmentMods()
    {
        equipModifiers.Clear();
        equipModifiers.AddRange(myEquiptment.MeleeWeapon.myModifiers);
        equipModifiers.AddRange(myEquiptment.RangedWeapon.myModifiers);
        equipModifiers.AddRange(myEquiptment.Armor.myModifiers);
        foreach(EquipmentStatBlock block in myEquiptment.Rings)
        {
            equipModifiers.AddRange(block.myModifiers);
        }
    }
    public void ClearExternalMods(bool recalculate_)
    {
        externalModifiers.Clear();
        if(recalculate_)
        {
            CalculateAllModifiers();
        }
    }
    public void AddExternalMod(EquipModifier mod_)
    {
       foreach(EquipModifier modX in externalModifiers)
        {
            if (modX.modName == mod_.modName)
            {
                externalModifiers.Remove(modX);
                externalModifiers.Add(mod_);
                return;
            }
        }
        externalModifiers.Add(mod_);
        CalculateAllModifiers();
    }
    public void RemoveExternalMod(EquipModifier mod_)
    {
        for(int i=0;i<externalModifiers.Count;i++)
        {
            if (externalModifiers[i].modName == mod_.modName)
            {
                externalModifiers.Remove(externalModifiers[i]);
                CalculateAllModifiers();
                return;
            }
        }
    }
    private void ApplyModifier(EquipModifier mod_)
    {
        if (mod_.uniqueEffect == UniqueEquipEffect.None)
        {
            switch (mod_.affectedStat)
            {
                case Stat.HP:
                    maxHealth = AddOrMultiply(mod_.isMultiplicative, maxHealth, mod_.amount);
                    break;
                case Stat.SP:
                    maxMana = AddOrMultiply(mod_.isMultiplicative, maxMana, mod_.amount);
                    break;
                case Stat.PATK:
                    PhysicalAtk = AddOrMultiply(mod_.isMultiplicative, PhysicalAtk, mod_.amount);
                    break;
                case Stat.MATK:
                    MysticalAtk = AddOrMultiply(mod_.isMultiplicative, MysticalAtk, mod_.amount);
                    break;
                case Stat.PDEF:
                    PhysicalDef = AddOrMultiply(mod_.isMultiplicative, PhysicalDef, mod_.amount);
                    break;
                case Stat.MDEF:
                    MysticalDef = AddOrMultiply(mod_.isMultiplicative, MysticalDef, mod_.amount);
                    break;
                case Stat.LUCK:
                    playerLuck = AddOrMultiply(mod_.isMultiplicative, playerLuck, mod_.amount);
                    break;
            }
        }
        switch (mod_.uniqueEffect)
        {
            case UniqueEquipEffect.None:
                return;
            case UniqueEquipEffect.HealthRegen:
                HealthRegenPercent += mod_.amount;
                break;
            case UniqueEquipEffect.ManaRegen:
                ManaRegenPercent += mod_.amount;
                break;
            case UniqueEquipEffect.basicMeleeSpeed:
                combatActions.attackSpeedMod += mod_.amount;
                break;
            case UniqueEquipEffect.basicRangedSpeed:
                combatActions.fireRateMod += mod_.amount;
                break;
            case UniqueEquipEffect.LifeSteal:
                combatActions.lifeStealPercent += mod_.amount;
                break;
        }
    }

    private float AddOrMultiply(bool multiply_,float A,float B)
    {
        if(multiply_)
        {
            return A * B;
        }
        else
        {
            return A + B;
        }
    }
    public void LevelUp()
    {
        myStats.Level += 1;
        myStats.remainingSkillPoints += 5;
        myStats.totalSkillPoints += 5;
        combatActions.myFamiliar.monsterStats.Level += 1;
        combatActions.myFamiliar.monsterStats.remainingSkillPoints += 5;
        combatActions.myFamiliar.monsterStats.totalSkillPoints += 5;
        CalculateAllModifiers();
        Instantiate(levelUpEffect,transform.position,transform.rotation);
    }
    public int GetExpToNextLevel()
    {
        return myStats.GetEXPToLevelUp();
    }
    public void SkillTreeEffects()
    {
        //Create the empty mods first here , then in the for loops you set up the exact stats based on points invested 
        //Slime
        EquipModifier slimeIncreasedPDef = new EquipModifier();
        slimeIncreasedPDef.isMultiplicative = true;
        slimeIncreasedPDef.modName = "slimeIncreasedPDefense";
        slimeIncreasedPDef.affectedStat = Stat.PDEF;
        slimeIncreasedPDef.amount = 1;
        slimeIncreasedPDef.uniqueEffect = UniqueEquipEffect.None;
        EquipModifier slimeIncreasedMDef = new EquipModifier();
        slimeIncreasedMDef.isMultiplicative = true;
        slimeIncreasedMDef.modName = "slimeIncreasedMDefense";
        slimeIncreasedMDef.affectedStat = Stat.MDEF;
        slimeIncreasedMDef.amount = 1;
        slimeIncreasedMDef.uniqueEffect = UniqueEquipEffect.None;
        EquipModifier slimeHealthRegen = new EquipModifier();
        slimeHealthRegen.isMultiplicative = false;
        slimeHealthRegen.modName = "slimeHealthRegen";
        slimeHealthRegen.amount = 0;
        slimeHealthRegen.uniqueEffect = UniqueEquipEffect.HealthRegen;

        //Sword
        EquipModifier swordSpeed = new EquipModifier();
        swordSpeed.isMultiplicative = false;
        swordSpeed.modName = "swordAttackSpeed";
        swordSpeed.amount = 0;
        swordSpeed.uniqueEffect = UniqueEquipEffect.basicMeleeSpeed;

        EquipModifier swordPDamage = new EquipModifier();
        swordPDamage.isMultiplicative = true;
        swordPDamage.modName = "swordPhysicalDamage";
        swordPDamage.amount = 1;
        swordPDamage.uniqueEffect = UniqueEquipEffect.None;

        EquipModifier swordLifeSteal = new EquipModifier();
        swordPDamage.isMultiplicative = false;
        swordPDamage.modName = "swordLifeSteal";
        swordPDamage.amount = 0;
        swordPDamage.uniqueEffect = UniqueEquipEffect.LifeSteal;

        //Dragon
        EquipModifier dragonSpeed = new EquipModifier();
        dragonSpeed.isMultiplicative = false;
        dragonSpeed.modName = "dragonAttackSpeed";
        dragonSpeed.amount = 0;
        dragonSpeed.uniqueEffect = UniqueEquipEffect.basicRangedSpeed;

        EquipModifier dragonMDamage = new EquipModifier();
        dragonMDamage.isMultiplicative = true;
        dragonMDamage.modName = "dragonMysticalDamage";
        dragonMDamage.amount = 1;
        dragonMDamage.uniqueEffect = UniqueEquipEffect.None;
        combatActions.rangedPierce = false;

        foreach (Talent tal_ in myTalents.talents)
        {
            switch(tal_.ID)
            {
                case "Necromancer":
                    mySkeltonMaster.enabled = false;
                    mySkeltonMaster.maxMageFollowers = 0;
                    mySkeltonMaster.maxFollowers = 0;
                    mySkeltonMaster.maxSuperFollowers = 0;
                    extraLife = false;
                    for(int i=0;i<tal_.levelInvested;i++)
                    {
                        if(i<5)
                        {
                            mySkeltonMaster.enabled = true;
                            mySkeltonMaster.maxFollowers += 1;
                        }
                        else if (i < 10)
                        {
                            mySkeltonMaster.maxMageFollowers += 1;
                        }
                        else if (i == 10)
                        {
                            mySkeltonMaster.maxSuperFollowers += 1;
                            extraLife = true;
                        }
                    }
                    mySkeltonMaster.Reset();
                    break;
                case "Slime":
                    
                    for (int i = 0; i < tal_.levelInvested; i++)
                    {
                        if (i < 5)
                        {
                            slimeIncreasedPDef.amount += 0.04f;
                            slimeIncreasedMDef.amount += 0.04f;
                        }
                        else if (i < 10)
                        {
                            slimeHealthRegen.amount += 0.01f;
                        }
                        else if (i == 10)
                        {
                            EquipModifier unkillable = new EquipModifier();
                            unkillable.isMultiplicative = true;
                            slimeIncreasedMDef.affectedStat = Stat.HP;
                            unkillable.modName = "unkillable";
                            unkillable.amount = 4;
                            unkillable.uniqueEffect = UniqueEquipEffect.None;
                            combatActions.myFamiliar.AddExternalMod(unkillable);
                            combatActions.myCoopFamiliar.AddExternalMod(unkillable);
                        }
                    }
                    break;
                case "Dragon":

                    for (int i = 0; i < tal_.levelInvested; i++)
                    {
                        if (i < 5)
                        {
                            dragonSpeed.amount += 0.1f;
                        }
                        else if (i < 10)
                        {
                            dragonMDamage.amount += 0.05f;
                        }
                        else if (i == 10)
                        {
                            combatActions.rangedPierce = true;
                        }
                    }
                    break;
                case "Sword":

                    for (int i = 0; i < tal_.levelInvested; i++)
                    {
                        if (i < 5)
                        {
                            swordSpeed.amount += 0.1f;
                        }
                        else if (i < 10)
                        {
                            swordPDamage.amount += 0.05f;
                        }
                        else if (i == 10)
                        {
                            swordPDamage.amount = 0.25f;
                        }
                    }
                    break;



            }
        }
        //add the mods to players and fams here
        //Slime mods
        combatActions.myFamiliar.AddExternalMod(slimeHealthRegen);
        combatActions.myFamiliar.AddExternalMod(slimeIncreasedMDef);
        combatActions.myFamiliar.AddExternalMod(slimeIncreasedPDef);
        combatActions.myCoopFamiliar.AddExternalMod(slimeHealthRegen);
        combatActions.myCoopFamiliar.AddExternalMod(slimeIncreasedMDef);
        combatActions.myCoopFamiliar.AddExternalMod(slimeIncreasedPDef);
        AddExternalMod(slimeHealthRegen);
        AddExternalMod(slimeIncreasedMDef);
        AddExternalMod(slimeIncreasedPDef);

        //Dragon
        combatActions.myFamiliar.AddExternalMod(dragonMDamage);
        combatActions.myCoopFamiliar.AddExternalMod(dragonMDamage);
        AddExternalMod(dragonSpeed);
        AddExternalMod(dragonMDamage);
        //Sword mods
        combatActions.myFamiliar.AddExternalMod(swordPDamage);
        combatActions.myCoopFamiliar.AddExternalMod(swordPDamage);
        AddExternalMod(swordSpeed);
        AddExternalMod(swordPDamage);
        AddExternalMod(swordLifeSteal);


        combatActions.myFamiliar.CalculateAllModifiers();
        combatActions.myCoopFamiliar.CalculateAllModifiers();

    }
    public void UndeadExtraLife()
    {
        currentHealth = maxHealth;
        healthBar.SetBar01(currentHealth / maxHealth);
        skullHead.SetActive(true);
    }
    /// <summary>
    /// Calculates the nearest interactable object and sets that as the interactable target that will be used for lock ons
    /// </summary>
    private void GetClosestInteractableObject()
    {
        if (myInteractableObjects.Count == 0)
        {
            interactableObjectTarget = null;
            interactableObjectLockOnObject.SetActive(false);
            return;
        }
        for (int i = 0; i < myInteractableObjects.Count; i++)
        {
            if (!myInteractableObjects[i].activeInHierarchy)
            {
                if (interactableObjectTarget == myInteractableObjects[i])
                    interactableObjectTarget = null;
                myInteractableObjects.RemoveAt(i);
                continue;
            }
            if (!interactableObjectTarget)
            {
                interactableObjectTarget = myInteractableObjects[i];
            }
            if (Vector3.Distance(transform.position, myInteractableObjects[i].transform.position) < Vector3.Distance(transform.position, interactableObjectTarget.transform.position))
                interactableObjectTarget = myInteractableObjects[i];
            interactableObjectLockOnObject.SetActive(true);
            interactableObjectLockOnObject.transform.position = interactableObjectTarget.transform.position;
        }
        foreach (GameObject obj in myInteractableObjects)
        {
            if (Vector3.Distance(transform.position, obj.transform.position) < Vector3.Distance(transform.position, interactableObjectTarget.transform.position))
                interactableObjectTarget = obj;
        }
    }
    /// <summary>
    /// The actions taken when the player presses the interact button
    /// </summary>
    private void InteractAction()
    {
        if (interactableObjectTarget)
        {
            if (interactableObjectTarget.TryGetComponent<InteractableObject>(out InteractableObject obj))
            {
                obj.Interact();
            }
        }
    }
    private void InteractPressed(InputAction.CallbackContext objdd)
    {
        InteractHeld = true;
    }
    private void InteractReleased(InputAction.CallbackContext objdd)
    {
        InteractHeld = false;
    }
    public void RemoveInteractableObject(GameObject obj_)
    {
        myInteractableObjects.Remove(obj_);
        if (interactableObjectTarget = obj_)
        {
            interactableObjectTarget = null;
            interactableObjectLockOnObject.SetActive(false);
        }
    }
}
