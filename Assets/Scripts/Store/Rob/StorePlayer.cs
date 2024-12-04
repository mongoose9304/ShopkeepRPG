using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// The player class while they are in the store
/// </summary>
public class StorePlayer : MonoBehaviour
{
    public bool isPlayer2;
    public bool inHell;
    [Header("Movement")]
    [Tooltip("The time before a player can dash again")]
    public float maxdashCoolDown;
    [Tooltip("The base speed a player can move")]
    public float moveSpeed;
    [Tooltip("A temproary modifier to increase or decrease a players speed 1=100%")]
    public float moveSpeedModifier;
    [Tooltip("The distance a player can dash")]
    public float dashDistance;
    [Tooltip("If the Player is currently dashing")]
    public bool isDashing;
    [Tooltip("Timebefore Player can warp to other store again")]
    public float teleportCooldownMax;
    private float teleportCooldown;
    float dashTime;
    float dashCoolDown;



    [Header("Interactions")]
    [Tooltip("All the objects the player is currently in range to interact with")]
    public List<GameObject> myInteractableObjects = new List<GameObject>();
    [Tooltip("The object the player is currently locked onto")]
    [SerializeField] GameObject interactableObjectTarget;
    [Tooltip("REFERENCE to gameobject used to show what you are locked onto")]
    [SerializeField] GameObject interactableObjectLockOnObject;

    [Header("Moveable")]
    [Tooltip("All the objects the player is currently in range to interact with")]
    public List<MoveableObjectSlot> myMoveableObjectSlots = new List<MoveableObjectSlot>();
    [Tooltip("The object the player is currently locked onto")]
    [SerializeField] MoveableObjectSlot moveableObjectSlotTarget;
    [Tooltip("REFERENCE to gameobject used to show what you are locked onto")]
    [SerializeField] GameObject moveableObjectSlotLockOnObject;
    [Tooltip("The MoveableObject we are currently holding or have picked up")]
    [SerializeField] MoveableObject heldObject;
    [Tooltip("Are we currently able to move objects")]
    public bool isInMovingMode;
    [Tooltip("REFERENCE to location to spawn held objects, currently over the player's head")]
    public GameObject heldObjectSpawn;
    [Tooltip("The visual for the object we are holding")]
    public GameObject heldObjectVisual;
    [Tooltip("REFERENCE to object that detects if any moveable objects around us")]
    public GameObject moveDetector;
    [Tooltip("REFERENCE to the UI for move mode")]
    public GameObject moveModeUIObject;

    [Header("REFERNCES and Inputs")]
    //used for movement calculations
    Vector3 moveInput;
    Vector3 newInput;
    Vector3 dashStartPos;
    Rigidbody rb;
    //slight delay before player regains control after falling off the map
    float timeBeforePlayerCanMoveAfterFallingOffPlatform;
    [Tooltip("The layermask for the walls")]
    [SerializeField] LayerMask wallMask;
    [Tooltip("REFERENCE to gameobject used for dashing")]
    [SerializeField] GameObject dashEffect;
    [Tooltip("The layermask for tiles and the floor")]
    [SerializeField] LayerMask tileLayer;
    [SerializeField] AudioClip dashAudio;
    [SerializeField] bool isDead;
    public ParticleSystem tpEffect;
    [Header("Inputs")]
    public InputActionMap playerActionMap;
    public InputAction movement;
    private bool InteractHeld;

    public void SetUpControls(PlayerInput myInput)
    {
        playerActionMap = myInput.actions.FindActionMap("Player");
        movement = playerActionMap.FindAction("Movement");
        playerActionMap.FindAction("Dash").performed += OnDash;
        playerActionMap.FindAction("YAction").performed += OnInteract;
        playerActionMap.FindAction("YAction").canceled += OnInteractReleased;
        playerActionMap.FindAction("XAction").performed += OnMoveAction;
        playerActionMap.FindAction("LTAction").performed += OnWarp;
        if(!isPlayer2)
        playerActionMap.FindAction("RBAction").performed += OnOpenMoveableInventory;
        playerActionMap.FindAction("StartAction").performed += OnPause;
        playerActionMap.Enable();
    }
    private void OnEnable()
    {
        playerActionMap.Enable();
    }
    private void OnDisable()
    {
        playerActionMap.Disable();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        dashStartPos = transform.position;
    }
    // Update is called once per frame

    void Update()
    {
        if (TempPause.instance.isPaused)
            return;
        if (isDead)
            return;
        //close most menus by pressing the B or back button
        if (!isPlayer2)
        {
            if (ShopManager.instance.inMenu&&!ShopManager.instance.player2InMenu)
            {
                return;
            }
            if(ShopManager.instance.inHaggle)
                return;
        }
        else
        {
            if (ShopManager.instance.inMenu && ShopManager.instance.player2InMenu)
            {
                return;
            }
            if (ShopManager.instance.player2InHaggle)
                return;
        }

           
        
        GetInput();
        moveInput = PreventGoingThroughWalls(moveInput);
        GetClosestInteractableObject();
        GetClosestMoveableObject();


        if (!isDashing)
        {


            if (dashCoolDown > 0)
                dashCoolDown -= Time.deltaTime;


                if (timeBeforePlayerCanMoveAfterFallingOffPlatform <= 0)
                {
                    //transform.position = Vector3.SmoothDamp(transform.position, transform.position + PreventFalling() * moveSpeed * Time.deltaTime * moveSpeedModifier, ref velocity, dampModifier);
                    transform.position = transform.position + PreventFalling() * moveSpeed * moveSpeedModifier * Time.deltaTime;
                }
                else
                    timeBeforePlayerCanMoveAfterFallingOffPlatform -= Time.deltaTime;
                if (moveInput != Vector3.zero)
                    transform.forward = moveInput;
          
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
                transform.position =  PreventGoingThroughWalls(temp);

            }




        }
    }
    /// <summary>
    /// The actions taken when the player presses the dash button
    /// </summary>
    private void OnDash(InputAction.CallbackContext obj)
    {
        if (TempPause.instance.isPaused)
            return;
        if (!isPlayer2)
        {
            if (ShopManager.instance.inMenu&& !ShopManager.instance.player2InMenu)
            {

                ShopManager.instance.MenuBackButton();
                if (ShopManager.instance)
                {
                    ShopManager.instance.PlayUIAudio("Close");
                }
                return;
            }
            if (ShopManager.instance.inHaggle)
            {
                {
                    ShopManager.instance.MenuBackButton();
                    if (ShopManager.instance)
                    {
                        ShopManager.instance.PlayUIAudio("Close");
                    }
                    return;
                }
            }
        }
        else
        {
            if (ShopManager.instance.inMenu &&ShopManager.instance.player2InMenu)
            {

                ShopManager.instance.MenuBackButton(true);
                if (ShopManager.instance)
                {
                    ShopManager.instance.PlayUIAudio("Close");
                }
                return;
            }
            if (ShopManager.instance.player2InHaggle)
            {
                {
                    ShopManager.instance.MenuBackButton(true);
                    if (ShopManager.instance)
                    {
                        ShopManager.instance.PlayUIAudio("Close");
                    }
                    return;
                }
            }
        }
        
        if (dashCoolDown <= 0)
        {
            dashCoolDown = maxdashCoolDown;
            DashAction();
        }
    }
    /// <summary>
    /// The actions taken when the player presses the dash button
    /// </summary>
    private void OnWarp(InputAction.CallbackContext obj)
    {

        if (!ShopTutorialManager.instance.inTut)
            WarpToOtherShop();
    }
    /// <summary>
    /// The actions taken when the player presses the dash button
    /// </summary>
    private void OnInteract(InputAction.CallbackContext obj)
    {
        InteractHeld = true;
    }
    /// <summary>
    /// The actions taken when the player presses the dash button
    /// </summary>
    private void OnInteractReleased(InputAction.CallbackContext obj)
    {
        InteractHeld = false;
    }
    /// <summary>
    /// The actions taken when the player presses the dash button
    /// </summary>
    private void OnMoveAction(InputAction.CallbackContext obj)
    {
        if (ShopManager.instance.inMenu)
        {
            return;
        }
        if (isInMovingMode)
            MoveItemAction();
    }
    private void OnPause(InputAction.CallbackContext obj)
    {
        if (TempPause.instance)
        {
            TempPause.instance.TogglePause();
        }
    }
    /// <summary>
    /// The actions taken when the player presses the dash button
    /// </summary>
    private void OnOpenMoveableInventory(InputAction.CallbackContext obj)
    {
        if (ShopManager.instance.inMenu)
        {
            return;
        }
        if (isInMovingMode)
            ShopManager.instance.OpenMoveableObjectScreen();
    }

    /// <summary>
    /// Gets all the butons or movement inputs the player makes each frame
    /// </summary>
    void GetInput()
    {
       
        if(teleportCooldown>0)
        teleportCooldown -= Time.deltaTime;
        if (InteractHeld)
            InteractAction();
        moveInput = new Vector3(movement.ReadValue<Vector2>().x, 0, movement.ReadValue<Vector2>().y);
    }
    /// <summary>
    /// The funcionality of a players dash. The player snaps to the nearest 90 degree and moves forwards constantly unless there is a wall
    /// </summary>
    private void DashAction()
    {

        dashStartPos = transform.position;
        SnapRotationToGrid(transform);
        if (moveInput != Vector3.zero)
            transform.forward = moveInput;
        isDashing = true;
        dashTime = 0.2f;
        Instantiate(dashEffect, transform.position, transform.rotation);
        MMSoundManager.Instance.PlaySound(dashAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
     false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
     1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);

    }
    /// <summary>
    /// The actions taken when the player presses the interact button, the functionality comes from the object being used
    /// </summary>
    private void InteractAction()
    {
        if(interactableObjectTarget)
        {
           if(interactableObjectTarget.TryGetComponent<InteractableObject>(out InteractableObject obj ))
            {
                obj.Interact(gameObject);
            }
        }
    }
    /// <summary>
    /// The actions taken when the player presses the move button (pickup or place down)
    /// </summary>
    private void MoveItemAction()
    {
        if (moveableObjectSlotTarget)
        {
            if (!heldObject)
            {
                if (moveableObjectSlotTarget.CheckForObject())
                {
                    SetHeldObject(moveableObjectSlotTarget.placedObject);
                    moveableObjectSlotTarget.PickUpObject();
                }
            }
            else
            {
                if (moveableObjectSlotTarget.CheckForObject())
                {

                }
                else
                {
                    moveableObjectSlotTarget.PlaceObject(heldObject);
                    ClearHeldObject();
                }
            }
        }
    }
    /// <summary>
    /// Set the object we are holding and create a visual object above the players head
    /// </summary>
    public void SetHeldObject(MoveableObject obj_)
    {
        if(obj_ == null)
        {
            heldObject = obj_;
            if (heldObjectVisual)
            {
                Destroy(heldObjectVisual);
            }
            return;
        }
        heldObject = obj_;
        if(heldObjectVisual)
        {
            Destroy(heldObjectVisual);
        }
      heldObjectVisual=  GameObject.Instantiate(heldObject.myHeldVisualPrefab, heldObjectSpawn.transform);
        heldObjectVisual.transform.position = heldObjectSpawn.transform.position;

    }
    public MoveableObject GetHeldObject()
    {
        return heldObject;
    }
    private void ClearHeldObject()
    {
        heldObject = null;
        if (heldObjectVisual)
        {
            Destroy(heldObjectVisual);
        }
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
    /// Calculates the nearest moveable object and sets that as the moveable target that will be used for lock ons
    /// </summary>
    private void GetClosestMoveableObject()
    {
        if (myMoveableObjectSlots.Count == 0)
        {
            moveableObjectSlotTarget = null;
            moveableObjectSlotLockOnObject.SetActive(false);
            return;
        }
        for (int i = 0; i < myMoveableObjectSlots.Count; i++)
        {
            if (!myMoveableObjectSlots[i].gameObject.activeInHierarchy)
            {
                if (moveableObjectSlotTarget == myMoveableObjectSlots[i])
                    moveableObjectSlotTarget = null;
                myMoveableObjectSlots.RemoveAt(i);
                continue;
            }
            if (!moveableObjectSlotTarget)
            {
                moveableObjectSlotTarget = myMoveableObjectSlots[i];
            }
            if (Vector3.Distance(transform.position, myMoveableObjectSlots[i].transform.position) < Vector3.Distance(transform.position, moveableObjectSlotTarget.transform.position))
                moveableObjectSlotTarget = myMoveableObjectSlots[i];
            moveableObjectSlotLockOnObject.SetActive(true);
            moveableObjectSlotLockOnObject.transform.position = moveableObjectSlotTarget.transform.position;
        }
        foreach (MoveableObjectSlot obj in myMoveableObjectSlots)
        {
            if (Vector3.Distance(transform.position, obj.transform.position) < Vector3.Distance(transform.position, moveableObjectSlotTarget.transform.position))
                moveableObjectSlotTarget = obj;
        }
    }
    /// <summary>
    /// Snaps the player to the nearest 90 degree angle
    /// </summary>
    /// <param name="transform_">Transform to snap</param>
    private void SnapRotationToGrid(Transform transform_)
    {
      
      if(transform_.localEulerAngles.y<45|| transform_.localEulerAngles.y> 315)
        {
            //lock to 0
           // transform_.localEulerAngles.Set(0,0,0);
            moveInput.Set(0, 0, 1);

        }
      else  if (transform_.localEulerAngles.y < 135 && transform_.localEulerAngles.y > 45)
        {
            //lock to 90
           // transform_.localEulerAngles.Set(0, 90, 0);
            moveInput.Set(1, 0, 0);
        }
      else if (transform_.localEulerAngles.y < 225 && transform_.localEulerAngles.y >135)
        {
            //lock to 180
            //transform_.localEulerAngles.Set(0, 180, 0);
            moveInput.Set(0, 0, -1);
        }
      else  if (transform_.localEulerAngles.y < 315 && transform_.localEulerAngles.y > 225)
        {
            //lock to 270
            //transform_.localEulerAngles.Set(0, 270, 0);
            moveInput.Set(-1, 0, 0);
        }
    }
    /// <summary>
    /// Adjusts the players movement to stop them from walikng off ledges
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
    /// Adjusts the players movement to stop them from walikng into walls
    /// </summary>
    /// <param name="temp_">current movement input</param>
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
    /// Check if the player is hitting a wall
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// Check if the player is grounded and if not put them back on ground
    /// </summary>
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
    /// Teleport to the other shop
    /// </summary>
    public void WarpToOtherShop()
    {
        if (teleportCooldown > 0)
            return;
        teleportCooldown = teleportCooldownMax;
        ShopManager.instance.WarpPlayerToOtherStore(gameObject);
    }
    public void PlayTeleportEffects()
    {
        tpEffect.Play();
    }
    /// <summary>
    /// Toggle the ability to move objects, this will also save their layout 
    /// </summary>
    public void ToggleMoveMode()
    {
        if(ShopTutorialManager.instance.inTut)
        {
            ShopManager.instance.MenuBackButton();
            ShopManager.instance.ChangeCameraForUI(PlayerManager.instance.GetPlayers()[0]);
            if (heldObject)
            {
                ShopManager.instance.moveableObjectScreen.StoreItem(heldObject);
                ClearHeldObject();
            }
            if (isInMovingMode)
            {
                moveDetector.SetActive(false);
                moveableObjectSlotLockOnObject.SetActive(false);
                myMoveableObjectSlots.Clear();
                isInMovingMode = false;
                if (moveModeUIObject)
                    moveModeUIObject.SetActive(false);
            }
            else
            {
                moveDetector.SetActive(true);
                isInMovingMode = true;
                moveModeUIObject.SetActive(true);
            }
            return;
        }
        ShopManager.instance.MenuBackButton();
        ShopManager.instance.ChangeCameraForUI(PlayerManager.instance.GetPlayers()[0]);
        if (heldObject)
        {
            ShopManager.instance.moveableObjectScreen.StoreItem(heldObject);
            ClearHeldObject();
        }
        if (isInMovingMode)
        {
            moveDetector.SetActive(false);
            moveableObjectSlotLockOnObject.SetActive(false);
            myMoveableObjectSlots.Clear();
            isInMovingMode = false;
            MoveableObjectManager.instance.SaveAllSlots();
            ShopManager.instance.SetPedestalList();
            ShopManager.instance.SetBarginBinList();
            ShopManager.instance.RedoNavMesh();
            if (moveModeUIObject)
                moveModeUIObject.SetActive(false);
            ShopManager.instance.DebugSaveItems();
        }
        else
        {
            moveDetector.SetActive(true);
            isInMovingMode = true;
            moveModeUIObject.SetActive(true);
        }
        
    }
  
   
}
