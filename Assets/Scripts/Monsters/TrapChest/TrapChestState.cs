using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;


public interface IState
{
    void Enter();
    void ExecuteState();
    void Exit();
}

public class TrapChestStateMachine {

    IState currentState;
    public TrapChestStateMachine(IState initialState = null)
    {
        currentState = initialState;
    }

    public void ChangeState(IState newState_)
    {
        if (currentState != null) {
            currentState.Exit();
        }
       
        currentState = newState_;
        currentState.Enter();
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.ExecuteState();
        }
    }
}


public class PatrolRotateState : MonoBehaviour, IState {
    //Ray casting
    Vector3 RayOrigin;
    Vector3 RayDirection;
    Quaternion orignalRot;



    private float timer = 0.0f;
    private float rotationCD = 4.0f;
    private bool startCooldown = false;
    private float changeInAngle = 25.0f;
    private float currentAngle = 0.0f;
    private float targetAngle = 0.0f;
    private float rotationSpeed = 5.0f;
    private bool pauseRotation = false;

    private float rotationTick = 0.3f;


    private float[] angles = { 0.0f, 45.0f, 90.0f, 135.0f, 180.0f };
    private int index = 0;


    //information needed for the field of view 
    [SerializeField] float viewDistance = 5.8f;
    private float viewAngle = 90.0f;
    private int amountOfRays = 10;

    public void Enter() {
        Debug.Log("Entering Patrol State");
        RayOrigin = transform.position;
        RayDirection = new Vector3(10.0f, 0.0f, 0.0f);
        currentAngle = transform.rotation.eulerAngles.y;
        targetAngle = angles[0];
        Debug.Log(currentAngle);

    }
    public void ExecuteState() {
       Rotate();
       CastRays();
      // OnDrawGizmos();
    }
    public void Exit() {

        Debug.Log("Entering Patrol State");
    }


    void Rotate() {
        if (pauseRotation)
        {

            timer += Time.deltaTime;
            if (timer >= rotationCD)
            {
                pauseRotation = false;
                timer = 0.0f;

                //choose next angle depending on the index
                index = (index + 1) % angles.Length;

                targetAngle = angles[index];
            }
        }
        else
        {
            rotationTick = rotationSpeed * Time.deltaTime;

            Debug.Log(currentAngle);
            float angle = Mathf.MoveTowards(currentAngle, targetAngle, rotationTick);

            currentAngle = angle;
            //Debug.Log(String::Format()angle);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
            //transform.rotation =  new Vector3(0.0f, currentAngle, 0.0f);

            if (Mathf.Approximately(currentAngle, targetAngle))
            {
                pauseRotation = true;
            }
        }
    }
    void CastRays() {
        RayOrigin = transform.position;
        //this is to ganerate the cone- like field of view. 
        //Since for this mob we cant use simple colliders

        float startingPoint = -viewAngle / 2;
        float angleBetweenRays = viewAngle / amountOfRays;

        for (int i = 0; i < amountOfRays; i++)
        {
            float newAngle = startingPoint + angleBetweenRays * i;
            RayDirection = Quaternion.Euler(0, newAngle, 0) * transform.forward;
           
            Debug.DrawRay(transform.position, RayDirection, Color.red);

            if (Physics.Raycast(RayOrigin, RayDirection, out RaycastHit hit, viewDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Player detected");
                }

            }


        }
    }

    void OnDrawGizmos()
    {
        // Visualize the cone in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.rotation * Vector3.forward * viewDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.rotation * Vector3.forward * viewDistance;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}


public class TrapChestState : MonoBehaviour
{
    TrapChestStateMachine trapChestStateMachine;
    public PatrolRotateState patrolState;
    void Start()
    {
        trapChestStateMachine = new TrapChestStateMachine(patrolState);
        patrolState = gameObject.AddComponent<PatrolRotateState>();
        trapChestStateMachine.ChangeState(patrolState);
    }

 
    void Update()
    {

        trapChestStateMachine.Update();

    }


}