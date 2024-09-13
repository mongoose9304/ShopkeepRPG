using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
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
    Vector3 moveInput;
    Vector3 newInput;
    Vector3 dashStartPos;
    Rigidbody rb;
    float timeBeforePlayerCanMoveAfterFallingOffPlatform;
   [SerializeField] LayerMask wallMask;
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
    public Element myWeakness;
    public float PhysicalAtk;
    public float MysticalAtk;
    public float PhysicalDef;
    public float MysticalDef;
    public float LevelModifier;
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
    float currentHealth;
    float currentMana;
    float currentManaRechargeDelay;
    private GameObject tempObj;
    public GameObject levelUpEffect;
    [Header("UI")]
    public MMProgressBar healthBar;
    public MMProgressBar manaBar;
    public MMProgressBar familiarHealthBar;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        AddAllEquipmentMods();
        CalculateAllModifiers();
        SetArmorElements();
        
        currentHealth = maxHealth;
        currentMana = maxMana;
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
        if (combatActions.isBusy)
            return;
        GetInput();
     moveInput=PreventGoingThroughWalls(moveInput);
        CheckForSoftLockOn();
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
    private void OnDash()
    {
        if (dashCoolDown <= 0)
        {
            dashCoolDown = maxdashCoolDown;
            DashAction();
        }
    }
    void GetInput()
    {
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
       if(Input.GetButtonDown("Fire2"))
        {
            OnDash();
        }
        
    }
    private void DashAction()
    {
        if (currentMana < dashCost)
            return;
        if (Physics.Raycast(transform.position - new Vector3(0f, 0f, -1), transform.TransformDirection(Vector3.down), 10))
            dashStartPos = transform.position;
        UseMana(dashCost);
        if (moveInput != Vector3.zero)
            transform.forward = moveInput;
        isDashing = true;
        dashTime = 0.2f;
        Instantiate(dashEffect, transform.position, transform.rotation);
       
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
        // Up

        if (Physics.Raycast(transform.position + new Vector3(0f, 5.0f, -0.5f), dir, 15, wallMask))
            if (newInput.z < 0)
                newInput.z = 0;
        // Down
        if (Physics.Raycast(transform.position + new Vector3(0f, 5.0f, .5f), dir, 15, wallMask))
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
        if (Physics.Raycast(transform.position, dir, 0.5f, wallMask))
            return true;
        dir = transform.TransformDirection(Vector3.left);
        if (Physics.Raycast(transform.position, dir, 0.5f, wallMask))
            return true;
        return false;


    }
    private void GroundCheck()
    {
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 10))
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
        float newDamage = damage_;
        if (element_ == myWeakness && element_ != Element.Neutral)
        {
            newDamage *= 1.5f;
        }
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
        maxHealth = (myStats.Vitality * 10) * (1 + (myStats.Level * LevelModifier));
        maxMana = (myStats.Soul * 10) * (1 + (myStats.Level * LevelModifier));
        PhysicalAtk = (myStats.PhysicalProwess) * (1 + (myStats.Level * LevelModifier));
        MysticalAtk = (myStats.MysticalProwess) * (1 + (myStats.Level * LevelModifier));
        PhysicalDef = (myStats.PhysicalDefense) * (1 + (myStats.Level * LevelModifier));
        MysticalDef = (myStats.MysticalDefense) * (1 + (myStats.Level * LevelModifier));
        playerLuck = (myStats.Luck) * (1+(myStats.Level * LevelModifier));
        HealthRegenPercent = 0;
        ManaRegenPercent = 0;
        combatActions.attackSpeedMod = 1;
        combatActions.fireRateMod = 1;
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
        mySkeltonMaster.SetStats(maxHealth, PhysicalAtk, MysticalAtk, PhysicalDef, MysticalDef);
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
                return;
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
        }
    }
    private void SetArmorElements()
    {
        switch (myEquiptment.Armor.myElement)
        {
            case Element.Fire:
                myWeakness = Element.Water;
                break;
            case Element.Water:
                myWeakness = Element.Earth;
                break;
            case Element.Air:
                myWeakness = Element.Earth;
                break;
            case Element.Earth:
                myWeakness = Element.Fire;
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

        foreach (Talent tal_ in myTalents.talents)
        {
            switch(tal_.ID)
            {
                case "Necromancer":
                    mySkeltonMaster.enabled = false;
                    mySkeltonMaster.maxSuperFollowers = 0;
                    mySkeltonMaster.maxFollowers = 0;
                    mySkeltonMaster.superSkeletonPower = 0;
                    extraLife = false;
                    for(int i=0;i<tal_.levelInvested;i++)
                    {
                        if(i<5)
                        {
                            mySkeltonMaster.enabled = true;
                            mySkeltonMaster.maxFollowers += 1;
                        }
                        else if (i == 5)
                        {
                            mySkeltonMaster.maxSuperFollowers += 1;
                            mySkeltonMaster.superSkeletonPower += 1.0f;
                        }
                        else if (i < 10)
                        {
                            mySkeltonMaster.superSkeletonPower += 1.0f;
                        }
                        else if (i == 10)
                        {
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
        AddExternalMod(slimeHealthRegen);
        AddExternalMod(slimeIncreasedMDef);
        AddExternalMod(slimeIncreasedPDef);

        //Dragon
        combatActions.myFamiliar.AddExternalMod(dragonMDamage);
        AddExternalMod(dragonSpeed);
        AddExternalMod(dragonMDamage);
        //Sword mods
        combatActions.myFamiliar.AddExternalMod(swordPDamage);
        AddExternalMod(swordSpeed);
        AddExternalMod(swordPDamage);


        combatActions.myFamiliar.CalculateAllModifiers();

    }
    public void UndeadExtraLife()
    {
        currentHealth = maxHealth;
        healthBar.SetBar01(currentHealth / maxHealth);
        skullHead.SetActive(true);
    }
}
