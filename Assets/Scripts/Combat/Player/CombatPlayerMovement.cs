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
    [SerializeField] GameObject saveYourSoulUI;
    public int timesYouHaveDied;
    [SerializeField] float sosDecreasePerDeath;


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
    [SerializeField] CombatPlayerActions combatActions;
    //targeting and lock on
    [SerializeField] GameObject currentTarget;
    [SerializeField] bool hardLockOn;
    [SerializeField] GameObject lockOnIcon;
    [SerializeField] Transform lockOnCheckPosition;
    
    
    [SerializeField] string enemyTag;
    [SerializeField] float minDistanceBetweenRetargets;
    [SerializeField] float MaxLockOnDistance;
   
    //put stats in a script where all player stats can be held later. Only here for temp testing
    public float maxHealth;
    public Element myWeakness;
    public float maxMana;
    public float maxManaRechargeDelay;
    public float manaRechargeRate;
    float currentHealth;
    float currentMana;
    float currentManaRechargeDelay;
    private GameObject tempObj;
    [Header("UI")]
    public MMProgressBar healthBar;
    public MMProgressBar manaBar;
    public MMProgressBar familiarHealthBar;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        currentMana = maxMana;
    }
    // Update is called once per frame

    void Update()
    {
        if (isDying)
            return;
        if(isInSaveYourSoulMode)
        {
            SaveYourSoulUpdate();
        }
        ChargeMana();
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
    public void TakeDamage(float damage_, float hitstun_, Element element_, float knockBack_ = 0, GameObject knockBackObject = null)
    {
        if(isInSaveYourSoulMode){ return; }
        if (element_ == myWeakness && element_ != Element.Neutral)
        {
            damage_ *= 1.5f;
        }
        currentHealth -= damage_;
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
            saveYourSoulUI.SetActive(true);
            timesYouHaveDied += 1;
        }
    }
    public void TrueDeath()
    {
        isDying = true;
        saveYourSoulUI.SetActive(false);
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
        saveYourSoulUI.SetActive(false);
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

        if (currentMana >= maxMana)
            currentMana = maxMana;
        manaBar.UpdateBar01(currentMana / maxMana);
    }
}
