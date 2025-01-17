using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishingPlayer : MonoBehaviour
{
    //movement
    public bool isPlayer2;
    public float maxdashCoolDown;
    public float moveSpeed;
    public float moveSpeedModifier;
    public float dashDistance;
    public float dashCoolDown;
    public float dashTime;
    public bool isDashing;

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

    [SerializeField] string enemyTag;
    public AudioClip dashAudio;

    [Header("Inputs")]
    public InputActionMap playerActionMap;
    private InputAction movement;
    private bool InteractHeld;
    private bool castHeld;

    public GameObject bobberPrefab;
    private GameObject currentBobber;

    public float castPower = 0.0f;
    public float maxCast = 10.0f;
    public float minCast = 1.0f;
    public float castSpeed = 4.0f;
    private int castMultiplier = 1;
    private Vector3 castDirection;

    public bool canMove = true;
    FishingMinigame menu = null;

    public void SetUpControls(PlayerInput myInput)
    {
        playerActionMap = myInput.actions.FindActionMap("Player");
        movement = playerActionMap.FindAction("Movement");
        playerActionMap.FindAction("Dash").performed += OnDash;
        playerActionMap.FindAction("XAction").performed += OnCast;
        playerActionMap.FindAction("XAction").canceled += OnCastReleased;
        playerActionMap.FindAction("YAction").performed += OnInteract;
        playerActionMap.FindAction("YAction").canceled += OnInteractReleased;
        playerActionMap.FindAction("StartAction").performed += OnPause;
        playerActionMap.Enable();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (TempPause.instance.isPaused)
            return;

        if (castHeld == true)
        {
            canMove = false;
            castPower += castSpeed * Time.deltaTime * castMultiplier;
            if (castPower >= maxCast || castPower <= 0.0f)
            {
                castMultiplier *= -1;
            }

            // Set bobber trial position
            currentBobber.transform.position = rb.position + castDirection * castPower;
            // Zero out the y so that it lies flat on the water surface
            currentBobber.transform.position = new Vector3(currentBobber.transform.position.x, 0.0f, currentBobber.transform.position.z);

            // Face in the direction of the input
            GetInput();
            // A deadzone for changing your rotation
            if (Vector3.Magnitude(moveInput) > 0.5f)
            {
                castDirection = Vector3.Normalize(moveInput);
            }
            transform.LookAt(rb.position + castDirection);
        }

        if (canMove == true)
        {
            GetInput();
            moveInput = PreventGoingThroughWalls(moveInput);
            GetClosestInteractableObject();
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
                    transform.position = PreventGoingThroughWalls(temp);
                }
            }
        }
        else
        {
            if (menu != null)
            {
                menu.HandleInputs(movement.ReadValue<Vector2>());
            }
        }
    }
    /// <summary>
    /// The actions taken when the player presses the dash button
    /// </summary>
    private void OnCast(InputAction.CallbackContext obj)
    {
        castHeld = true;
        if (currentBobber == null)
        {
            currentBobber = Instantiate(bobberPrefab);
            //Color currentColour = currentBobber.GetComponent<Renderer>().material.color;
            //currentBobber.GetComponent<Renderer>().material.color = new Color(currentColour.r, currentColour.g, currentColour.b, 0.5f);
        }
        currentBobber.GetComponent<BobberLogic>().isActive = false;
    }
    /// <summary>
    /// The actions taken when the player presses the dash button
    /// </summary>
    private void OnCastReleased(InputAction.CallbackContext obj)
    {
        castHeld = false;

        // Potentially cast fishing line
        if (castPower > minCast)
        {
            // Bobber becomes solid
            Color currentColour = currentBobber.GetComponent<Renderer>().material.color;
            currentBobber.GetComponent<Renderer>().material.color = new Color(currentColour.r, currentColour.g, currentColour.b, 1.0f);

            // Set bobber final position
            currentBobber.transform.position = rb.position + castDirection * castPower;
            currentBobber.transform.position = new Vector3(currentBobber.transform.position.x, 0.0f, currentBobber.transform.position.z);

            currentBobber.GetComponent<BobberLogic>().isActive = true;
        }
        else
        {
            canMove = true;
            Destroy(currentBobber);
        }
        castPower = 0.0f;
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        InteractHeld = true;
    }
    private void OnInteractReleased(InputAction.CallbackContext obj)
    {
        InteractHeld = false;
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
        if (dashCoolDown <= 0)
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

    public void InitiateMinigame()
    {
        if (menu == null)
        {
            menu = GameObject.Find("MinigameUI").GetComponent<FishingMinigame>();
        }
        menu.Activate();
    }
}
