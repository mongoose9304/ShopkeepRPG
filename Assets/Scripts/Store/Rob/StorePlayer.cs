using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePlayer : MonoBehaviour
{
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
    float dashTime;
    float dashCoolDown;



    [Header("Interactions")]
    [Tooltip("All the objects the player is currently in range to interact with")]
    public List<GameObject> myInteractableObjects = new List<GameObject>();
    [Tooltip("The object the player is currently locked onto")]
    [SerializeField] GameObject interactableObjectTarget;
    [Tooltip("REFERENCE to gameobject used to show what you are locked onto")]
    [SerializeField] GameObject interactableObjectLockOnObject;

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
        if (ShopManager.instance.inMenu)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                ShopManager.instance.MenuBackButton();
            }
            return;
        }

           
        
        GetInput();
        moveInput = PreventGoingThroughWalls(moveInput);
        GetClosestInteractableObject();


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
               // transform.position = Vector3.SmoothDamp(transform.position, PreventGoingThroughWalls(temp), ref velocity, dampModifier);
                transform.position =  PreventGoingThroughWalls(temp);
                if(dashTime<=0.1f)
                DashEdgeCheck();

            }




        }
    }
    /// <summary>
    /// The actions taken when the player presses the dash button
    /// </summary>
    private void OnDash()
    {
        
        if (dashCoolDown <= 0)
        {
            dashCoolDown = maxdashCoolDown;
            DashAction();
        }
    }

    /// <summary>
    /// Gets all the butons or movement inputs the player makes each frame
    /// </summary>
    void GetInput()
    {
       
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Fire3"))
        {
            
        }
        if (Input.GetButtonDown("Fire2"))
        {
            OnDash();
        }
        if (Input.GetButton("Fire4"))
        {
            InteractAction();
        }
        if (Input.GetButtonDown("Fire1"))
        {
        
        }

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
    /// The actions taken when the player presses the interact button
    /// </summary>
    private void InteractAction()
    {
        if(interactableObjectTarget)
        {
           if(interactableObjectTarget.TryGetComponent<InteractableObject>(out InteractableObject obj ))
            {
                obj.Interact();
            }
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
        /* for directions that will not change when player moves
        var dir = transform.TransformDirection(Vector3.down);
        // Up

        if (Physics.Raycast(transform.position + new Vector3(0f, 5.0f, -0.5f), dir, 15, wallMask))
            return true;
        // Down
        if (Physics.Raycast(transform.position + new Vector3(0f, 5.0f, 0.5f), dir, 15, wallMask))
            return true;
        //Left
        if (Physics.Raycast(transform.position + new Vector3(0.5f, 5.0f, 0f), dir, 15, wallMask))
            return true;
        //Right
        if (Physics.Raycast(transform.position + new Vector3(-0.5f, 5.0f, 0f), dir, 15, wallMask))
            return true;

        return false;


        */
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
    /// <summary>
    /// Stop the player from going over the edge near the end of their dash to make the dash feel smoother.
    /// </summary>
    private void DashEdgeCheck()
    {
        if (!Physics.Raycast(transform.position+transform.forward, transform.TransformDirection(Vector3.down), 10))
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 10))
            {
                dashTime = 0;
                isDashing = false;
            }
        }
    }





    /// <summary>
    /// Functionality for running out of life
    /// </summary>
    public void Death()
    {
        if (isDead)
            return;
        isDead = true;
    }
   
  
   
}
