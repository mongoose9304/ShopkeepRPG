using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class MiningPlayer : MonoBehaviour
{
    public float maxdashCoolDown;
    public float moveSpeed;
    public float moveSpeedModifier;
    public float dampModifier;
    public float dashDistance;
    public float dashCoolDown;
    public float dashTime;
    public bool isDashing;
    public int bombCountMax;
    [SerializeField] private GameObject bombObject;
    public List<GameObject> activeBombs = new List<GameObject>();
    [SerializeField] protected MMMiniObjectPooler bombPool;
    Vector3 moveInput;
    Vector3 newInput;
    Vector3 dashStartPos;
    Rigidbody rb;
    float timeBeforePlayerCanMoveAfterFallingOffPlatform;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] LayerMask wallMask;
    [SerializeField] GameObject dashEffect;


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
      
        if (!isDashing)
        {


            if (dashCoolDown > 0)
                dashCoolDown -= Time.deltaTime;


            if (timeBeforePlayerCanMoveAfterFallingOffPlatform <= 0)
                transform.position = Vector3.SmoothDamp(transform.position, transform.position + PreventFalling() * moveSpeed * Time.fixedDeltaTime * moveSpeedModifier, ref velocity, dampModifier);
            else
                timeBeforePlayerCanMoveAfterFallingOffPlatform -= Time.deltaTime;
            if (moveInput != Vector3.zero)
                transform.forward = moveInput;
          
        }
        else
        {
            if (dashTime > 0)
            {
                Vector3 temp = transform.position + (transform.forward * moveSpeed * Time.fixedDeltaTime * dashDistance);
                transform.position = Vector3.SmoothDamp(transform.position, PreventGoingThroughWalls(temp), ref velocity, dampModifier);
                dashTime -= Time.deltaTime;
                if (CheckForWallHit())
                {
                    dashTime = 0;
                }
                if (dashTime <= 0)
                {
                    isDashing = false;
                    GroundCheck();
                }
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
      
        if (Input.GetButtonDown("Fire2"))
        {
            OnDash();
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
    private void SnapRotationToGrid(Transform transform_)
    {
        Debug.Log("Local angle " + transform_.localEulerAngles.y);
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
        if (Physics.Raycast(transform.position, dir, 2.5f, wallMask))
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
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down),out hit, 10))
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