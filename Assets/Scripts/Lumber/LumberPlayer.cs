using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberPlayer : MonoBehaviour
{
    //movement
    public int axePower;
    public int shovelPower;
    public float maxdashCoolDown;
    public float moveSpeed;
    public float moveSpeedModifier;
    public float dashDistance;
    public float dashCoolDown;
    public float dashTime;
    public bool isDashing;
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
    [SerializeField] GameObject dashEffect;
    [SerializeField] LayerMask tileLayer;

    [SerializeField] string enemyTag;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
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
    private void OnDash()
    {

        if (dashCoolDown <= 0 && !isSwinging)
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
            AxeAction();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            OnDash();
        }
        if (Input.GetButton("Fire4"))
        {
            InteractAction();
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



    public void Death()
    {

    }




}