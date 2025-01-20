using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The 2nd player will ocntrol the familiar of the player
/// </summary>
public class CombatCoopFamiliar : CombatControllerInterface 
{
        [Header("References")]
    [Tooltip("The saved stats of this player")]
    [SerializeField] public StatBlock monsterStats;
    [Tooltip("REFERENCE to the ground layer")]
    [SerializeField] LayerMask groundMask;
    [Tooltip("REFERENCE to the dash effect used by the player")]
    [SerializeField] GameObject dashEffect;
    [Tooltip("REFERENCE to the dash SFX used by the player")]
    public AudioClip dashAudio;
    [Tooltip("REFERENCE to the controls for the current familar, these will swap between familiars as they have diffrent attacks and such")]
    public FamiliarCombatControls combatControls;
    [Tooltip("REFERENCE to all possible controls for the familars, these will swap between familiars as they have diffrent attacks and such")]
    public FamiliarCombatControls[] combatControlsForAllFamiliars;
    [Tooltip("REFERENCE to the teleport effect used by the player")]
    public ParticleSystem teleportParticles;
    [Tooltip("REFERENCE to the first player")]
    public CombatPlayerMovement combatPlayerMovement;
    [Tooltip("REFERENCE to the lock on Icon")]
    [SerializeField] GameObject lockOnIcon;
    
    [Tooltip("REFERENCE to the effects played when the player is killed")]
    [SerializeField] GameObject deathEffect;
    [Tooltip("REFERENCE to gameobject used to show what you are locked onto")]
    [SerializeField] GameObject interactableObjectLockOnObject;
    [Tooltip("REFERENCE to the wall layers")]
    public LayerMask wallMask;

    [Header("LockOn")]
    [Tooltip("how far this player can remain locked onto an enemy")]
    public float maxLockOnDistance;
    [Tooltip("min distance before retargeting to a closer enemy")]
    [SerializeField] float minDistanceBetweenRetargets;
    [Tooltip("current enemy I am targeting")]
    [SerializeField] GameObject currentTarget;
    [Tooltip("currently unused, would be for manual lock ons")]
    [SerializeField] bool hardLockOn;
    
    [Header("Dash")]
    [Tooltip("how long before the player can move after falling off a platform")]
    public float timeBeforePlayerCanMoveAfterFallingOffPlatform;
    [Tooltip("how far the player can dash")]
    public float dashDistance;
    bool isDashing;
    float dashCoolDown;
    [Tooltip("how long must the player wait between dashes")]
    public float maxdashCoolDown;
    float dashTime;
    [Tooltip("how long before the player can teleport to the other player")]
    public float maxTeleportCoolDown;
    float currentTeleportCoolDown;
    [Tooltip("where the player started their dash in case they need to be sent back")]
    [SerializeField] Vector3 dashStartPos;

        [Header("Interactions")]
    [Tooltip("All the objects the player is currently in range to interact with")]
    public List<GameObject> myInteractableObjects = new List<GameObject>();
    [Tooltip("The object the player is currently locked onto")]
    [SerializeField] GameObject interactableObjectTarget;

        [Header("Stats")]
    [SerializeField]  float currentHealth;
    public float maxHealth;
    public Element myElement;
    public float PhysicalAtk;
    public float MysticalAtk;
    public float PhysicalDef;
    public float MysticalDef;
    [Tooltip("This will be how much stronger each stat will be based on the user's level")]
    public float LevelModifier;
    public float HealthRegenPercent;
    [Tooltip("any modifiers affecting the player's stats")]
    public List<EquipModifier> externalModifiers = new List<EquipModifier>();
    [Tooltip("How long before the 2nd player can respawn")]
    public float respawnTimeMax;

     [Header("Inputs")]
    [Tooltip("The player's controls")]
    public InputActionMap playerActionMap;
    [Tooltip("used to quickly get movement inputs ")]
    private InputAction movement;
    Vector3 moveInput;
    Vector3 newInput;
    [Tooltip("how fast the player will move")]
    public float moveSpeed;
    [Tooltip("used for temp speed buffs/debuffs")]
    public float moveSpeedModifier;
    private bool InteractHeld;
    private bool controlsEnabled;

    /// <summary>
    /// Attatch the controller to this scripts inputs
    /// </summary>
    public void SetUpControls(PlayerInput myInput)
    {
        playerActionMap = myInput.actions.FindActionMap("Player");
        movement = playerActionMap.FindAction("Movement");
        playerActionMap.FindAction("YAction").performed += InteractPressed;
        playerActionMap.FindAction("YAction").canceled += InteractReleased;
        playerActionMap.FindAction("Dash").performed += OnDash;
        playerActionMap.FindAction("RTAction").performed += TeleportPressed;
        controlsEnabled = true;
    }
    public void ChangeFamiliar(Familiar fam_)
    {
        foreach(FamiliarCombatControls control in combatControlsForAllFamiliars)
        {
            control.gameObject.SetActive(false);
        }
        switch (fam_)
        {
            case Familiar.Slime:
                combatControls = combatControlsForAllFamiliars[0];
                break;
            case Familiar.Skeleton:
                combatControls = combatControlsForAllFamiliars[1];
                break;

        }
        combatControls.EnableActions(playerActionMap);
        combatControls.gameObject.SetActive(true);

    }
    private void OnEnable()
    {
        currentHealth = maxHealth;
        if(!controlsEnabled)
        {
            playerActionMap.FindAction("YAction").performed += InteractPressed;
            playerActionMap.FindAction("YAction").canceled += InteractReleased;
            playerActionMap.FindAction("Dash").performed += OnDash;
            playerActionMap.FindAction("RTAction").performed += TeleportPressed;
            controlsEnabled = true;
        }
        playerActionMap.Enable();
    }
    private void OnDisable()
    {
        playerActionMap.Disable();
        playerActionMap.FindAction("YAction").performed -= InteractPressed;
        playerActionMap.FindAction("YAction").canceled -= InteractReleased;
        playerActionMap.FindAction("Dash").performed -= OnDash;
        playerActionMap.FindAction("RTAction").performed -= TeleportPressed;
        controlsEnabled = false;
    }
    private void Update()
    {
        if (combatControls.isBusy)
            return;
        GetClosestInteractableObject();
        if (InteractHeld)
            InteractAction();
        moveInput = new Vector3(movement.ReadValue<Vector2>().x, 0, movement.ReadValue<Vector2>().y);
        //transform.position = transform.position + PreventFalling() * moveSpeed * moveSpeedModifier * Time.deltaTime;
        if (moveInput != Vector3.zero)
        {
            if(!combatControls.isControllingRotation)
            transform.forward = moveInput;
        }
        CheckForSoftLockOn();
        if (currentTeleportCoolDown > 0)
            currentTeleportCoolDown -= Time.deltaTime;
        moveInput = PreventGoingThroughWalls(moveInput);
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
            {
                if (!combatControls.isControllingRotation)
                    transform.forward = moveInput;
            }
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
    /// <summary>
    /// Cause the player to face its current target
    /// </summary>
    void LookAtCurrentTarget()
    {
        if (combatControls)
        {
            combatControls.SetCurrentTarget(currentTarget);
        }
        if (!currentTarget)
            return;

        transform.LookAt(currentTarget.transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    /// <summary>
    /// Find the closest target
    /// </summary>
    void CheckForSoftLockOn()
    {
        if (hardLockOn || EnemyManager.instance.currentEnemiesList.Count == 0)
            return;

        if (!currentTarget)
            currentTarget = EnemyManager.instance.currentEnemiesList[0];

        foreach (GameObject obj in EnemyManager.instance.currentEnemiesList)
        {
            if (!obj.activeInHierarchy)
                continue;
            if (Vector3.Distance(transform.position, obj.transform.position) < Vector3.Distance(transform.position, currentTarget.transform.position) - minDistanceBetweenRetargets)
                currentTarget = obj;
        }
        if (Vector3.Distance(transform.position, currentTarget.transform.position) > maxLockOnDistance)
            currentTarget = null;

        if (currentTarget)
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
    /// <summary>
    /// Get hit by an attack and calculate the damage
    /// </summary>
    public void TakeDamage(float damage_, float hitstun_, Element element_, float knockBack_ = 0, GameObject knockBackObject = null, bool isMystical = false)
    {
        if (combatControls.damageImmune)
            return;
        float newDamage = damage_;
        if (isMystical)
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
        if (combatControls.hitEffects)
            combatControls.hitEffects.PlayFeedbacks();
        if (combatPlayerMovement)
            combatPlayerMovement.UpdateFamiliarHealth(currentHealth / maxHealth);
    }
    /// <summary>
    /// Die and respawn after some time
    /// </summary>
    private void Death()
    {
        combatPlayerMovement.combatActions.FamiliarDeath(respawnTimeMax);
        gameObject.SetActive(false);
        Instantiate(deathEffect, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
    }
    /// <summary>
    /// Check if player is above the ground
    /// </summary>
    private void GroundCheck()
    {
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 10, groundMask))
        {
            // transform.position = new Vector3.(0, 0.66f, 0);
            transform.position = dashStartPos;
            moveInput = Vector3.zero;
            timeBeforePlayerCanMoveAfterFallingOffPlatform = 0.1f;
        }
    }
    /// <summary>
    /// Check if a player is hitting a wall
    /// </summary>
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
    /// <summary>
    /// Adjust a players input to stop walking through walls
    /// </summary>
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
    /// <summary>
    /// Prevent a player walking off an edge
    /// </summary>
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
            if(!myInteractableObjects[i])
            {
                myInteractableObjects.RemoveAt(i);
                continue;
            }
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
    /// <summary>
    /// Move the player forward in a burst
    /// </summary>
    private void DashAction()
    {
        dashStartPos = transform.position;
        if (moveInput != Vector3.zero)
            transform.forward = moveInput;
        isDashing = true;
        dashTime = 0.2f;
        Instantiate(dashEffect, transform.position, transform.rotation);
        MMSoundManager.Instance.PlaySound(dashAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
          false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.9f, 1.1f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
          1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    /// <summary>
    /// Remove an object from the interaction list if its being disabled to prevent errors
    /// </summary>
    public void RemoveInteractableObject(GameObject obj_)
    {
        myInteractableObjects.Remove(obj_);
        if (interactableObjectTarget = obj_)
        {
            interactableObjectTarget = null;
            interactableObjectLockOnObject.SetActive(false);
        }
    }
    /// <summary>
    /// Calculate the familliars stats
    /// </summary>
    protected virtual void CalculateStats()
    {
        maxHealth = (monsterStats.Vitality * 5);
        PhysicalAtk = (monsterStats.PhysicalProwess);
        MysticalAtk = (monsterStats.MysticalProwess);
        PhysicalDef = (monsterStats.PhysicalDefense);
        MysticalDef = (monsterStats.MysticalDefense);
        HealthRegenPercent = 0;
    }
    /// <summary>
    /// Apply all stat modifiers and adjust the players stats. Additive stats will be applied first, then multiplicative.
    /// </summary>
    public virtual void CalculateAllModifiers()
    {
        CalculateStats();
        List<EquipModifier> mods_ = new List<EquipModifier>();
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
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        if (combatPlayerMovement)
            combatPlayerMovement.SetFamiliarHealth(currentHealth / maxHealth);
        combatControls.CalculateDamage(PhysicalAtk,MysticalAtk);
    }
    /// <summary>
    /// Apply a single modifer
    /// </summary>
    private void ApplyModifier(EquipModifier mod_)
    {
        if (mod_.uniqueEffect == UniqueEquipEffect.None)
        {
            switch (mod_.affectedStat)
            {
                case Stat.HP:
                    maxHealth = AddOrMultiply(mod_.isMultiplicative, maxHealth, mod_.amount);
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
            }
        }
        switch (mod_.uniqueEffect)
        {
            case UniqueEquipEffect.None:
                return;
            case UniqueEquipEffect.HealthRegen:
                HealthRegenPercent += mod_.amount;
                break;
        }
    }
    /// <summary>
    /// Hellper function to add or multiply 2 values
    /// </summary>
    private float AddOrMultiply(bool multiply_, float A, float B)
    {
        if (multiply_)
        {
            return A * B;
        }
        else
        {
            return A + B;
        }
    }
    /// <summary>
    /// Add an external mod to the list
    /// </summary>
    public void AddExternalMod(EquipModifier mod_)
    {
        foreach (EquipModifier modX in externalModifiers)
        {
            if (modX.modName == mod_.modName)
            {
                externalModifiers.Remove(modX);
                externalModifiers.Add(mod_);
                return;
            }
        }
        externalModifiers.Add(mod_);
    }
    /// <summary>
    /// Regenerate the players health over time
    /// </summary>
    protected void RegenHealth()
    {
        if (HealthRegenPercent == 0)
            return;
        currentHealth += HealthRegenPercent * maxHealth * Time.deltaTime;

        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;
        combatPlayerMovement.SetFamiliarHealth(currentHealth / maxHealth);

    }





    private void InteractPressed(InputAction.CallbackContext objdd)
    {
        InteractHeld = true;
    }
    private void InteractReleased(InputAction.CallbackContext objdd)
    {
        InteractHeld = false;
    }
    private void  TeleportPressed(InputAction.CallbackContext objdd)
    {
        if (currentTeleportCoolDown > 0||TempPause.instance.isPaused|| combatControls.isBusy)
            return;
        currentTeleportCoolDown = maxTeleportCoolDown;
        CombatPlayerManager.instance.TeleportCoopPlayerToMainPlayer();
        teleportParticles.Play();
    }
    private void OnDash(InputAction.CallbackContext obj)
    {
        if (Time.timeScale <= 0)
            return;
        if (dashCoolDown <= 0)
        {
            dashCoolDown = maxdashCoolDown;
            DashAction();
        }
    }

}
