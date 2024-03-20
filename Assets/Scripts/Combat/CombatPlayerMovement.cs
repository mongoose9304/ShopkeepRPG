using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayerMovement : MonoBehaviour
{
    public float maxdashCoolDown;
    public float moveSpeed;
    public float moveSpeedModifier;
    public float dampModifier;
    public float dashDistance;
    public float dashCoolDown;
    public float dashTime;
    public bool isDashing;
    Vector3 moveInput;
    Vector3 newInput;
    Rigidbody rb;
    private Vector3 velocity = Vector3.zero;
   [SerializeField] LayerMask wallMask;
    GameObject lockOnTarget;
    [SerializeField] GameObject dashEffect;

    //targeting and lock on
    [SerializeField] GameObject currentTarget;
    [SerializeField] bool hardLockOn;
    [SerializeField] GameObject lockOnIcon;
    [SerializeField] Transform lockOnCheckPosition;
    [SerializeField] List<GameObject> currentEnemiesList=new List<GameObject>();
    
    [SerializeField] string enemyTag;
    [SerializeField] float minDistanceBetweenRetargets;
    [SerializeField] float MaxLockOnDistance;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame

    void Update()
    {
        GetInput();
     moveInput=PreventGoingThroughWalls(moveInput);
        CheckForSoftLockOn();
        if (!isDashing)
        {


            if (dashCoolDown > 0)
                dashCoolDown -= Time.deltaTime;


            //rb.MovePosition(rb.position + PreventFalling() * moveSpeed * Time.fixedDeltaTime * moveSpeedModifier);
            // transform.position= (transform.position + PreventFalling() * moveSpeed * Time.fixedDeltaTime * moveSpeedModifier);

            transform.position = Vector3.SmoothDamp(transform.position, transform.position + PreventFalling() * moveSpeed * Time.fixedDeltaTime * moveSpeedModifier, ref velocity, dampModifier);
            if (moveInput != Vector3.zero)
                transform.forward = moveInput;
            LookAtCurrentTarget();
        }
        else
        {
            if (dashTime > 0)
            {
                dashTime -= Time.deltaTime;
                if (dashTime <= 0)
                {
                    isDashing = false;
                    GroundCheck();
                }
            }
            // rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.fixedDeltaTime * dashDistance);
            // transform.position += (transform.forward * moveSpeed * Time.fixedDeltaTime * dashDistance);
            Vector3 temp = transform.position + (transform.forward * moveSpeed * Time.fixedDeltaTime * dashDistance);
            transform.position = Vector3.SmoothDamp(transform.position,PreventGoingThroughWalls(temp), ref velocity, dampModifier);
         
            //rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.fixedDeltaTime * dashDistance);
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
       if(Input.GetButtonDown("Fire2"))
        {
            OnDash();
        }
        
    }
    private void DashAction()
    {
        if (moveInput != Vector3.zero)
            transform.forward = moveInput;
        isDashing = true;
        dashTime = 0.2f;
        Instantiate(dashEffect, transform.position, transform.rotation);
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

        if (Physics.Raycast(transform.position + new Vector3(0f, 5.0f, -1), dir, 15,wallMask))
            if (newInput.z < 0)
                newInput.z = 0;
        // Down
        if (Physics.Raycast(transform.position + new Vector3(0f, 5.0f, 1), dir, 15, wallMask))
            if (newInput.z > 0)
                newInput.z = 0;
        //Left
        if (Physics.Raycast(transform.position + new Vector3(1, 5.0f, 0f), dir, 15, wallMask))
            if (newInput.x > 0)
                newInput.x = 0;
        //Right
        if (Physics.Raycast(transform.position + new Vector3(-1, 5.0f, 0f), dir, 15, wallMask))
            if (newInput.x < 0)
                newInput.x = 0;
        return newInput;


    }
    private void GroundCheck()
    {
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 10))
        {
            transform.position = new Vector3(0, 0.66f, 0);
        }
    }

    void CheckForSoftLockOn()
    {
        if (hardLockOn||currentEnemiesList.Count==0)
            return;

        if (!currentTarget)
            currentTarget = currentEnemiesList[0];

       foreach(GameObject obj in currentEnemiesList)
        {
            if (!obj.activeInHierarchy)
                continue;
            if (Vector3.Distance(lockOnCheckPosition.position, obj.transform.position) < Vector3.Distance(transform.position, currentTarget.transform.position)-minDistanceBetweenRetargets)
                currentTarget = obj;
        }
        if (Vector3.Distance(lockOnCheckPosition.position, currentTarget.transform.position) > MaxLockOnDistance)
            currentTarget = null;

        if(currentTarget)
        {
            lockOnIcon.transform.position = currentTarget.transform.position;
            lockOnIcon.SetActive(true);
            if (!currentTarget.activeInHierarchy)
                currentTarget = null;
        }
        else
        {

            lockOnIcon.SetActive(false);
        }
       

    }
    void LookAtCurrentTarget()
    {
        if (!currentTarget)
            return;
       
        transform.LookAt(currentTarget.transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    public void SetCurrentEnemyList(List<GameObject> objectsToSet)
    {
        currentEnemiesList.Clear();
        foreach(GameObject obj in objectsToSet)
        {
            currentEnemiesList.Add(obj);
        }
    }
    public void AddEnemy(GameObject obj_)
    {

        currentEnemiesList.Add(obj_);
    }
    public List<GameObject> GetCurrentEnemyList()
    {
        return currentEnemiesList;
    }
    private void CleanCurrentEnemyList()
    {
       
       currentEnemiesList.RemoveAll(null);
       
    }
    public GameObject GetCurrentTarget()
    {
        return currentTarget;
    }
        
}
