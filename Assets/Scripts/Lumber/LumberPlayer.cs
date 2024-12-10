using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine.InputSystem;
using Cinemachine;

public class LumberPlayer : MonoBehaviour
{
    //movement
    public bool isPlayer2;
    public int axePower;
    public int shovelPower;
    public float maxdashCoolDown;
    public float moveSpeed;
    public float moveSpeedModifier;
    public float dashDistance;
    public float dashCoolDown;
    public float dashTime;
    public bool isDashing;
    //Camera
    public CinemachineVirtualCamera cam;
    public float camChangeSpeed;
    private float camChange;
    public float MinDist;
    public float MaxDist;
    public float currentDist;
    CinemachineFramingTransposer transposerCam;
    [SerializeField] GameObject cameraObject;
    [SerializeField] Vector3 cameraRotationRegular;
    [SerializeField] Vector3 cameraRotationPuzzle;
    public LumberPuzzle myPuzzle;
    //Stealth
    public bool isHiding;
    public Transform hideLocation;
    //Axe
    [SerializeField] GameObject myAxe;
    public List<GameObject> myChopableObjects = new List<GameObject>();
    bool isSwinging;
    [SerializeField] float maxSwingtime;
    float currentSwingTime;
    [SerializeField] Vector3 startRotation;
    [SerializeField] float swingSpeed;
    [SerializeField] GameObject axeLockOnObject;
    [SerializeField] GameObject axeLockOnTarget;
    //references and inputs
    [SerializeField] GameObject interactableObjectTarget;
    [SerializeField] GameObject interactableObjectLockOnObject;
    public List<GameObject> myInteractableObjects = new List<GameObject>();
    Vector3 moveInput;
    Vector3 newInput;
    Vector3 dashStartPos;
    Rigidbody rb;
    float timeBeforePlayerCanMoveAfterFallingOffPlatform;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] LayerMask wallMask;
    [SerializeField] LayerMask groundMask;
    [SerializeField] GameObject dashEffect;
    [SerializeField] LayerMask tileLayer;

    [SerializeField] string enemyTag;
    public AudioClip dashAudio;

    [Header("Inputs")]
    public InputActionMap playerActionMap;
    private InputAction movement;
    private InputAction cameraMovement;
    private bool InteractHeld;
    private bool ResetHeld;
    public void SetUpControls(PlayerInput myInput)
    {
        playerActionMap = myInput.actions.FindActionMap("Player");
        movement = playerActionMap.FindAction("Movement");
        cameraMovement = playerActionMap.FindAction("CameraMovement");
        playerActionMap.FindAction("Dash").performed += OnDash;
        playerActionMap.FindAction("YAction").performed += OnInteract;
        playerActionMap.FindAction("YAction").canceled += OnInteractReleased;
        playerActionMap.FindAction("XAction").performed += OnAxeAction;
        playerActionMap.FindAction("LTAction").performed += OnPuzzleReset;
        playerActionMap.FindAction("LTAction").canceled += OnPuzzleResetReleased;
        playerActionMap.FindAction("StartAction").performed += OnPause;
        playerActionMap.Enable();
    }
    private void OnDisable()
    {
        playerActionMap.FindAction("Dash").performed -= OnDash;
        playerActionMap.FindAction("YAction").performed -= OnInteract;
        playerActionMap.FindAction("YAction").canceled -= OnInteractReleased;
        playerActionMap.FindAction("XAction").performed -= OnAxeAction;
        playerActionMap.FindAction("LTAction").performed -= OnPuzzleReset;
        playerActionMap.FindAction("LTAction").canceled -= OnPuzzleResetReleased;
        playerActionMap.FindAction("StartAction").performed -= OnPause;

    }


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        transposerCam = cam.GetCinemachineComponent<CinemachineFramingTransposer>();


    }
    // Update is called once per frame

    void Update()
    {
        if (TempPause.instance.isPaused)
            return;

        GetInput();
        moveInput = PreventGoingThroughWalls(moveInput);
        GetClosestChopableObject();
        GetClosestInteractableObject();
        if(isHiding)
        {
            if(hideLocation)
            transform.position = hideLocation.position;
            return;
        }

        if (!isDashing)
        {


            if (dashCoolDown > 0)
                dashCoolDown -= Time.deltaTime;

            if (!isSwinging)
            {
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
            PickaxeUpdate();

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
    /// The actions while the player holds the puzzle reset button
    /// </summary>
    private void OnPuzzleReset(InputAction.CallbackContext obj)
    {
        ResetHeld = true;
    }
    /// <summary>
    ///  The actions while the player releases the puzzle reset button
    /// </summary>
    private void OnPuzzleResetReleased(InputAction.CallbackContext obj)
    {
        ResetHeld = false;
    }
    /// <summary>
    /// The actions taken when the player presses the dash button
    /// </summary>
    private void OnAxeAction(InputAction.CallbackContext obj)
    {
        if (TempPause.instance.isPaused)
            return;
        AxeAction();
    }
    private void OnPause(InputAction.CallbackContext obj)
    {
        if (TempPause.instance)
        {
            TempPause.instance.TogglePause();
        }
    }
    private void OnDash(InputAction.CallbackContext obj)
    {
        if (TempPause.instance.isPaused)
            return;
        if (dashCoolDown <= 0 && !isSwinging)
        {
            dashCoolDown = maxdashCoolDown;
            DashAction();
        }
    }

    void GetInput()
    {

        moveInput = new Vector3(movement.ReadValue<Vector2>().x, 0, movement.ReadValue<Vector2>().y);
        if (InteractHeld)
            InteractAction();
        if (ResetHeld)
            ResetCurrentPuzzle();

        camChange = cameraMovement.ReadValue<Vector2>().y;
        if(camChange!=0)
        {
            AdjustCameraDistance(camChange);
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
   
    private void InteractAction()
    {
        if (interactableObjectTarget)
        {
            if (interactableObjectTarget.TryGetComponent<InteractableObject>(out InteractableObject obj))
            {
                obj.Interact(gameObject);
            }
        }
    }
    private void AxeAction()
    {
        if (currentSwingTime > 0 || isDashing)
            return;
        if (axeLockOnTarget)
        {
            transform.LookAt(axeLockOnTarget.transform);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            moveInput = Vector3.zero;
            currentSwingTime = maxSwingtime;
            myAxe.SetActive(true);
            myAxe.transform.localEulerAngles = startRotation;
            isSwinging = true;
            if (axeLockOnTarget.TryGetComponent<ChopableObject>(out ChopableObject obj))
            {
                obj.ChopInteraction(axePower);
            }
            return;
        }
        moveInput = Vector3.zero;
        currentSwingTime = maxSwingtime;
        myAxe.SetActive(true);
        myAxe.transform.localEulerAngles = startRotation;
        isSwinging = true;
    }
    private void GetClosestChopableObject()
    {
        if (myChopableObjects.Count == 0)
        {
            axeLockOnTarget = null;
            axeLockOnObject.SetActive(false);
            return;
        }

        for (int i = 0; i < myChopableObjects.Count; i++)
        {
            if (!myChopableObjects[i].activeInHierarchy)
            {
                if (axeLockOnTarget == myChopableObjects[i])
                    axeLockOnTarget = null;
                myChopableObjects.RemoveAt(i);
                continue;
            }
            if (!axeLockOnTarget)
            {
                axeLockOnTarget = myChopableObjects[i];
            }
            if (Vector3.Distance(transform.position, myChopableObjects[i].transform.position) < Vector3.Distance(transform.position, axeLockOnTarget.transform.position))
                axeLockOnTarget = myChopableObjects[i];
            axeLockOnObject.SetActive(true);
            axeLockOnObject.transform.position = axeLockOnTarget.transform.position;
        }
        if (!axeLockOnTarget)
            return;
        foreach (GameObject obj in myChopableObjects)
        {
            if (Vector3.Distance(transform.position, obj.transform.position) < Vector3.Distance(transform.position, axeLockOnTarget.transform.position))
                axeLockOnTarget = obj;
        }
    }
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
        if (myInteractableObjects.Count <= 1)
            return;
        foreach (GameObject obj in myInteractableObjects)
        {
            if (Vector3.Distance(transform.position, obj.transform.position) 
                < Vector3.Distance(transform.position, interactableObjectTarget.transform.position))
                interactableObjectTarget = obj;
        }
    }
    public void RemoveObjectFromChopableObjects(GameObject obj_)
    {
        myChopableObjects.Remove(obj_);
        if (axeLockOnTarget = obj_)
            axeLockOnTarget = null;
        axeLockOnObject.SetActive(false);
    }
    private void PickaxeUpdate()
    {
        if (currentSwingTime <= 0)
            return;
        myAxe.transform.Rotate(swingSpeed * Time.deltaTime, 0.0f, 0.0f, Space.Self);



        currentSwingTime -= Time.deltaTime;
        if (currentSwingTime <= 0)
            EndPickaxeAttack();
    }
    private void EndPickaxeAttack()
    {
        myAxe.SetActive(false);
        isSwinging = false;
    }
    private void AdjustCameraDistance(float dist_)
    {
        currentDist -= dist_*camChangeSpeed*Time.deltaTime;
        if (currentDist > MaxDist)
            currentDist = MaxDist;
        if (currentDist < MinDist)
            currentDist = MinDist;
        transposerCam.m_CameraDistance = currentDist;
    }
    private void SnapRotationToGrid(Transform transform_)
    {

        if (transform_.localEulerAngles.y < 45 || transform_.localEulerAngles.y > 315)
        {
            //lock to 0
            // transform_.localEulerAngles.Set(0,0,0);
            moveInput.Set(0, 0, 1);

        }
        else if (transform_.localEulerAngles.y < 135 && transform_.localEulerAngles.y > 45)
        {
            //lock to 90
            // transform_.localEulerAngles.Set(0, 90, 0);
            moveInput.Set(1, 0, 0);
        }
        else if (transform_.localEulerAngles.y < 225 && transform_.localEulerAngles.y > 135)
        {
            //lock to 180
            //transform_.localEulerAngles.Set(0, 180, 0);
            moveInput.Set(0, 0, -1);
        }
        else if (transform_.localEulerAngles.y < 315 && transform_.localEulerAngles.y > 225)
        {
            //lock to 270
            //transform_.localEulerAngles.Set(0, 270, 0);
            moveInput.Set(-1, 0, 0);
        }
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

        var dir = -transform.up;
        newInput = temp_;
        // Up
        if (Physics.Raycast(transform.position + new Vector3(0f, 10.0f, -0.5f), dir, 15, wallMask))
            if (newInput.z < 0)
                newInput.z = 0;

        // Down
        //Debug.DrawLine(transform.position + new Vector3(0f, 5.0f, 0.5f), dir * 2);
        if (Physics.Raycast(transform.position + new Vector3(0f, 10.0f, .5f), dir, 15, wallMask))
            if (newInput.z > 0)
                newInput.z = 0;
        //Left
        //Debug.DrawLine(transform.position + new Vector3(0.5f, 5.0f, 0f), dir * 2);
        if (Physics.Raycast(transform.position + new Vector3(0.5f, 10.0f, 0f), dir, 15, wallMask))
            if (newInput.x > 0)
                newInput.x = 0;
        //Right
        //Debug.DrawLine(transform.position + new Vector3(-0.5f, 5.0f, 0f), dir * 2);
        if (Physics.Raycast(transform.position + new Vector3(-0.5f, 10.0f, 0f), dir, 15, wallMask))
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
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 10, groundMask))
        {
            // transform.position = new Vector3.(0, 0.66f, 0);
            transform.position = dashStartPos;
            moveInput = Vector3.zero;
            timeBeforePlayerCanMoveAfterFallingOffPlatform = 0.1f;
        }
    }

    public void Hide(Transform hideLocation_,bool isHiding_ = true)
    {
        isHiding = isHiding_;
        hideLocation = hideLocation_;
        if(isHiding)
        {
            GuardManager.instance.PlayerAttemptingHide(gameObject);
        }
    }

    /// <summary>
    /// Removes an object from the mineable object list and resets the lockon target
    /// </summary>
    /// <param name="obj_">Object to remove</param>
    public void RemoveObjectFromInteractableObjects(GameObject obj_)
    {
        myInteractableObjects.Remove(obj_);
        if (interactableObjectTarget = obj_)
            interactableObjectTarget = null;
        interactableObjectLockOnObject.SetActive(false);
    }
    public void ResetCurrentPuzzle()
    {
        if(myPuzzle)
        myPuzzle.HoldResetTreeChopDirections();
    }


    public void Death()
    {

    }
    public void EnterPuzzle(LumberPuzzle puzzle_=null)
    {
        if(puzzle_)
        myPuzzle = puzzle_;
        cameraObject.transform.DORotate(cameraRotationPuzzle,1,RotateMode.Fast);
    }
    public void ExitPuzzle()
    {
        cameraObject.transform.DORotate(cameraRotationRegular, 1, RotateMode.Fast);
    }



}