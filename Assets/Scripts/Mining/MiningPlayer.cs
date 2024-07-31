using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

/// <summary>
/// The controls and behavior for the player during the mining activity
/// </summary>
public class MiningPlayer : MonoBehaviour
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

    [Header("Bombs")]
    [Tooltip("How many bombs the player can place at once")]
    public int bombCountMax;
    [Tooltip("How many squares in any direction a bomb will reach")]
    public int bombRange;
    [Tooltip("REFERENCE to the bomb gameobject")]
    [SerializeField] private GameObject bombObject;
    [Tooltip("REFERENCE to the pool of bombs the player has")]
    [SerializeField] protected MMMiniObjectPooler bombPool;

    [Header("Pickaxe")]
    [Tooltip("All the objects the player is currently in range to mine")]
    public List<GameObject> myMineableObjects=new List<GameObject>();
    bool isSwinging;
    [Tooltip("The length of time a pickaxe will swing for")]
    [SerializeField] float maxSwingtime;
    float currentSwingTime;
    [Tooltip("The rotation the pickaxe will start swinging from")]
    [SerializeField] Vector3 startRotation;
    [Tooltip("The speed the player will swing their axe")]
    [SerializeField] float swingSpeed;
    [Tooltip("The object the player is currently locked onto")]
    [SerializeField] GameObject pickaxeLockOnTarget;
    [Tooltip("REFERENCE to gameobject used to show what you are locked onto")]
    [SerializeField] GameObject pickaxeLockOnObject;
    [Tooltip("REFERENCE to the pickaxe the player swings")]
    [SerializeField] GameObject myPickaxe;

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

    //put stats in a script where all player stats can be held later. Only here for temp testing
    public float maxHealth;
    float currentHealth;
    [Header("UI")]
    public MMProgressBar healthBar;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
    }
    // Update is called once per frame

    void Update()
    {
       
        
        GetInput();
        moveInput = PreventGoingThroughWalls(moveInput);
        GetClosestMineableObject();
        GetClosestInteractableObject();


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
                transform.position =  PreventGoingThroughWalls(temp);


            }




        }
    }
    /// <summary>
    /// The actions taken when the player presses the dash button
    /// </summary>
    private void OnDash()
    {
        
        if (dashCoolDown <= 0&&!isSwinging)
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
            PickaxeAction();
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
            BombAction();
        }

    }
    /// <summary>
    /// The funcionality of a players dash. The player snaps to the nearest 90 degree and moves forwards constantly unless there is a wall
    /// </summary>
    private void DashAction()
    {
       
        if (Physics.Raycast(transform.position - new Vector3(0f, 0f, -1), transform.TransformDirection(Vector3.down), 10))
            dashStartPos = transform.position;
        SnapRotationToGrid(transform);
        if (moveInput != Vector3.zero)
            transform.forward = moveInput;
        isDashing = true;
        dashTime = 0.2f;
        Instantiate(dashEffect, transform.position, transform.rotation);

    }
    /// <summary>
    /// The actions taken when the player presses the bomb button
    /// </summary>
    private void BombAction()
    {
       GameObject obj= bombPool.GetPooledGameObject();
       Tile tile= GetCurrentTile();
        if(obj&&tile)
        {
           if(tile.CanPlaceBomb())
            {
                obj.SetActive(true);
                obj.transform.position = tile.transform.position + new Vector3(0, 1, 0);
                obj.GetComponent<Bomb>().range = bombRange;

                tile.SetBomb(obj.GetComponent<Bomb>());
            }
        }
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
    /// The actions taken when the player presses the pickaxe button
    /// </summary>
    private void PickaxeAction()
    {
        if (currentSwingTime > 0||isDashing)
            return;
        if(pickaxeLockOnTarget)
        {
            transform.LookAt(pickaxeLockOnTarget.transform);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            moveInput = Vector3.zero;
            currentSwingTime = maxSwingtime;
            myPickaxe.SetActive(true);
            myPickaxe.transform.localEulerAngles = startRotation;
            isSwinging = true;
            if(pickaxeLockOnTarget.TryGetComponent<MineableObject>(out MineableObject obj))
            {
                obj.MineInteraction();
            }
            return;
        }
        moveInput = Vector3.zero;
        currentSwingTime = maxSwingtime;
        myPickaxe.SetActive(true);
        myPickaxe.transform.localEulerAngles = startRotation;
        isSwinging = true;
    }
    /// <summary>
    /// Calculates the nearest mining object and sets that as the minable target that will be used for lock ons
    /// </summary>
    private void GetClosestMineableObject()
    {
        if(myMineableObjects.Count==0)
        {
            pickaxeLockOnTarget = null;
            pickaxeLockOnObject.SetActive(false);
            return;
        }
        
        for (int i=0;i<myMineableObjects.Count;i++)
        {
            if(!myMineableObjects[i].activeInHierarchy)
            {
                if (pickaxeLockOnTarget == myMineableObjects[i])
                    pickaxeLockOnTarget = null;
                myMineableObjects.RemoveAt(i);
                continue;
            }
            if(!pickaxeLockOnTarget)
            {
                pickaxeLockOnTarget = myMineableObjects[i];
            }
            if (Vector3.Distance(transform.position, myMineableObjects[i].transform.position) < Vector3.Distance(transform.position, pickaxeLockOnTarget.transform.position))
                pickaxeLockOnTarget = myMineableObjects[i];
            pickaxeLockOnObject.SetActive(true);
            pickaxeLockOnObject.transform.position = pickaxeLockOnTarget.transform.position;
        }
        if (!pickaxeLockOnTarget)
            return;
        foreach(GameObject obj in myMineableObjects)
        {
            if (Vector3.Distance(transform.position, obj.transform.position) < Vector3.Distance(transform.position, pickaxeLockOnTarget.transform.position))
                pickaxeLockOnTarget = obj;
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
    /// Removes an object from the mineable object list and resets the lockon target
    /// </summary>
    /// <param name="obj_">Object to remove</param>
    public void RemoveObjectFromMineableObjects(GameObject obj_)
    {
        myMineableObjects.Remove(obj_);
        if (pickaxeLockOnTarget = obj_)
            pickaxeLockOnTarget = null;
        pickaxeLockOnObject.SetActive(false);
    }
    /// <summary>
    /// The swinging of the pickaxe that happens during the update
    /// </summary>
    
    private void PickaxeUpdate()
    {
        if (currentSwingTime <= 0)
            return;
          myPickaxe.transform.Rotate(swingSpeed * Time.deltaTime, 0.0f, 0.0f, Space.Self);
      

   
        currentSwingTime -= Time.deltaTime;
        if (currentSwingTime <= 0)
            EndPickaxeAttack();
    }
    /// <summary>
    /// Set the pickaxe inactive once done
    /// </summary>
    private void EndPickaxeAttack()
    {
        myPickaxe.SetActive(false);
        isSwinging = false;
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
    /// Returns the tile you are standing on if any
    /// </summary>
    /// <returns></returns>
    private Tile GetCurrentTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down),out hit, 10,tileLayer))
        {
           
            if (hit.collider.gameObject.TryGetComponent<Tile>(out Tile tile))
            {
                return tile;
            }
        }
        return null;
    }





    /// <summary>
    /// The player will take damage, and if they run out of health they will die
    /// </summary>
    /// <param name="damage_">the damage to take</param>
    public void TakeDamage(float damage_)
    {
        currentHealth -= damage_;
        if (currentHealth <= 0)
        {
            Death();
            return;
        }
        healthBar.UpdateBar01(currentHealth / maxHealth);

    }
    /// <summary>
    /// Functionality for running out of life
    /// </summary>
    public void Death()
    {

    }
   
   
  
   
}
