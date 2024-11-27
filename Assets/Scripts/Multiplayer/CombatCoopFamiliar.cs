using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatCoopFamiliar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public StatBlock monsterStats;
    [SerializeField] LayerMask groundMask;
    [SerializeField] GameObject dashEffect;
    public AudioClip dashAudio;
    public FamiliarCombatControls combatControls;
    public ParticleSystem teleportParticles;
    public CombatPlayerMovement combatPlayerMovement;
    [Header("LockOn")]
    public float maxLockOnDistance;
    [SerializeField] float minDistanceBetweenRetargets;
    [SerializeField] GameObject currentTarget;
    [SerializeField] bool hardLockOn;
    [SerializeField] GameObject lockOnIcon;
    [Header("Dash")]
    public float timeBeforePlayerCanMoveAfterFallingOffPlatform;
    public float dashDistance;
    bool isDashing;
    float dashCoolDown;
    public float maxdashCoolDown;
    float dashTime;
    public float maxTeleportCoolDown;
    float currentTeleportCoolDown;
    [SerializeField] Vector3 dashStartPos;
    [Header("Interactions")]
    [Tooltip("All the objects the player is currently in range to interact with")]
    public List<GameObject> myInteractableObjects = new List<GameObject>();
    [Tooltip("The object the player is currently locked onto")]
    [SerializeField] GameObject interactableObjectTarget;
    [Tooltip("REFERENCE to gameobject used to show what you are locked onto")]
    [SerializeField] GameObject interactableObjectLockOnObject;
    [Header("Stats")]
    float currentHealth;
    //Stats Calculated based on Stat block
    public float maxHealth;
    public Element myWeakness;
    public Element myElement;
    public float PhysicalAtk;
    public float MysticalAtk;
    public float PhysicalDef;
    public float MysticalDef;
    public float LevelModifier;
    public float HealthRegenPercent;
    public List<EquipModifier> externalModifiers = new List<EquipModifier>();
    [Header("Inputs")]
    public InputActionMap playerActionMap;
    private InputAction movement;
    Vector3 moveInput;
    Vector3 newInput;
    public float moveSpeed;
    public float moveSpeedModifier;
    public LayerMask wallMask;
    private bool InteractHeld;
    public void SetUpControls(PlayerInput myInput)
    {
        playerActionMap = myInput.actions.FindActionMap("Player");
        movement = playerActionMap.FindAction("Movement");
        playerActionMap.FindAction("YAction").performed += InteractPressed;
        playerActionMap.FindAction("YAction").canceled += InteractReleased;
        playerActionMap.FindAction("Dash").performed += OnDash;
        playerActionMap.FindAction("LTAction").performed += TeleportPressed;
        combatControls.EnableActions(playerActionMap);
    }
    private void OnDisable()
    {
        playerActionMap.Disable();
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
            transform.forward = moveInput;
        CheckForSoftLockOn();
        if (currentTeleportCoolDown > 0)
            currentTeleportCoolDown -= Time.deltaTime;
        moveInput = PreventGoingThroughWalls(moveInput);
        if (!isDashing)
        {
            LookAtCurrentTarget();

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
    void LookAtCurrentTarget()
    {
        if (!currentTarget)
            return;

        transform.LookAt(currentTarget.transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
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
    public void RemoveInteractableObject(GameObject obj_)
    {
        myInteractableObjects.Remove(obj_);
        if (interactableObjectTarget = obj_)
        {
            interactableObjectTarget = null;
            interactableObjectLockOnObject.SetActive(false);
        }
    }
    protected virtual void CalculateStats()
    {
        maxHealth = (monsterStats.Vitality * 2) * (1 + (monsterStats.Level * LevelModifier));
        PhysicalAtk = (monsterStats.PhysicalProwess) * (1 + (monsterStats.Level * LevelModifier)) / 5;
        MysticalAtk = (monsterStats.MysticalProwess) * (1 + (monsterStats.Level * LevelModifier)) / 5;
        PhysicalDef = (monsterStats.PhysicalDefense) * (1 + (monsterStats.Level * LevelModifier)) / 5;
        MysticalDef = (monsterStats.MysticalDefense) * (1 + (monsterStats.Level * LevelModifier)) / 5;
        HealthRegenPercent = 0;
    }
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
