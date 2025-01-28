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

    TrapChestStateMachine trapChestStateMachine;
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
    private float rotationSpeed = 25.0f;
    private bool pauseRotation = false;


    float startingAngle = 0.0f;
    private float rotationTick = 10.0f;


    private float[] angles = { 0.0f, 45.0f, 90.0f, 135.0f, 180.0f };
    private int index = 0;

    float viewDistance = 5.0f;
    private float viewAngle = 90.0f;
    private int amountOfRays = 10;
    bool playerInViewRange = false;
    public void Enter() {
        Debug.Log("Entering Patrol State");
        RayOrigin = transform.position;
        RayDirection = new Vector3(10.0f, 0.0f, 0.0f);
        
        startingAngle = transform.rotation.eulerAngles.y;
        currentAngle = startingAngle;

        targetAngle = angles[1] + startingAngle;
        Debug.Log(currentAngle);

    }
    public void ExecuteState()
    {
        Rotate();
        CastRays();

        if (PlayerFound())
        {
            trapChestStateMachine.ChangeState(GetComponent<ChasePlayerState>());
        }
    }
    public void Exit() {

        Debug.Log("Exiting Patrol State");
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

                targetAngle = angles[index]+ startingAngle;
            }
        }
        else
        {
            rotationTick = rotationSpeed * Time.deltaTime;

            
            float angle = Mathf.MoveTowards(currentAngle, targetAngle, rotationTick);

            currentAngle = angle;
  
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
          

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
           
            Debug.DrawRay(transform.position, RayDirection * viewDistance , Color.red);

            if (Physics.Raycast(RayOrigin, RayDirection, out RaycastHit hit, viewDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Player detected");
                    playerInViewRange = true;
                }

            }
        }
    }

    bool PlayerFound() { return playerInViewRange; }
}


public class ChasePlayerState : MonoBehaviour, IState {

    float chaseTime = 10.0f;
    float timer = 0.0f;
    float chaseSpeed = 5.0f;

    Transform playerTransform;
    public void Enter() {
        Debug.Log("Entering Chase State");
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerObject.transform;
    }

    public void Exit()
    {
        Debug.Log("Exiting Chase State");
    }

    public void ExecuteState() {
        StartChase();
    }


    public void StartChase() { 
        timer += Time.deltaTime;
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;
        Quaternion facePlayer = Quaternion.LookRotation(direction);
        transform.rotation = facePlayer;


        if (timer >= chaseTime) {
            Debug.Log("Chase Ended");
           //give up/fade mechanic;
        }
        
    }
}


public class TrapChestState : MonoBehaviour
{
    TrapChestStateMachine trapChestStateMachine;
    public PatrolRotateState patrolState;
    public ChasePlayerState chasePlayerState;

    void Start()
    {
        trapChestStateMachine = new TrapChestStateMachine();
        patrolState = gameObject.AddComponent<PatrolRotateState>();
        chasePlayerState = gameObject.AddComponent<ChasePlayerState>();
        trapChestStateMachine.ChangeState(patrolState);
    }

 
    void Update()
    {

        trapChestStateMachine.Update();
  
    }


}