using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class MiningPlayer : MonoBehaviour
{
    //movement
    public float maxdashCoolDown;
    public float moveSpeed;
    public float moveSpeedModifier;
    public float dampModifier;
    public float dashDistance;
    public float dashCoolDown;
    public float dashTime;
    public bool isDashing;
    //bombs
    public int bombCountMax;
    [SerializeField] private GameObject bombObject;
    public List<GameObject> activeBombs = new List<GameObject>();
    [SerializeField] protected MMMiniObjectPooler bombPool;
    //pickaxe
    [SerializeField] GameObject myPickaxe;
    public List<GameObject> myMineableObjects=new List<GameObject>();
    bool isSwinging;
    [SerializeField] float maxSwingtime;
    float currentSwingTime;
    [SerializeField] Vector3 startRotation;
    [SerializeField] float swingSpeed;
    [SerializeField] GameObject pickaxeLockOnObject;
    [SerializeField] GameObject pickaxeLockOnTarget;
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
    [SerializeField] GameObject dashEffect;
    [SerializeField] LayerMask tileLayer;

    [SerializeField] string enemyTag;

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
                    transform.position = Vector3.SmoothDamp(transform.position, transform.position + PreventFalling() * moveSpeed * Time.deltaTime * moveSpeedModifier, ref velocity, dampModifier);
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
                transform.position = Vector3.SmoothDamp(transform.position, PreventGoingThroughWalls(temp), ref velocity, dampModifier);
               
                
            }




        }
    }
    private void OnDash()
    {
        
        if (dashCoolDown <= 0&&!isSwinging)
        {
            dashCoolDown = maxdashCoolDown;
            DashAction();
        }
    }
   
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
                tile.SetBomb(obj.GetComponent<Bomb>());
            }
        }
    }
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
    public void RemoveObjectFromMineableObjects(GameObject obj_)
    {
        myMineableObjects.Remove(obj_);
        if (pickaxeLockOnTarget = obj_)
            pickaxeLockOnTarget = null;
        pickaxeLockOnObject.SetActive(false);
    }
    private void PickaxeUpdate()
    {
        if (currentSwingTime <= 0)
            return;
          myPickaxe.transform.Rotate(swingSpeed * Time.deltaTime, 0.0f, 0.0f, Space.Self);
      

   
        currentSwingTime -= Time.deltaTime;
        if (currentSwingTime <= 0)
            EndPickaxeAttack();
    }
    private void EndPickaxeAttack()
    {
        myPickaxe.SetActive(false);
        isSwinging = false;
    }
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
    public void Death()
    {

    }
   
   
  
   
}
