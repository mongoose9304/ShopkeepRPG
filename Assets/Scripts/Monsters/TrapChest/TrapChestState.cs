using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;


//this is the interface for every state for this enemys
public interface IState
{
    void Enter();
    void ExecuteState();
    void Exit();
}

//In here the logic of switching states will be handled
public class TrapChestStateMachine : MonoBehaviour {

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

    //Everything needed for the state machine
    TrapChestStateMachine trapChestStateMachine;
    public ChasePlayerState chasePlayerState;


    //Ray casting for the cone-like field of views
    Vector3 RayOrigin;
    Vector3 RayDirection;
    private float viewDistance = 5.0f;
    private float viewAngle = 90.0f;
    private int amountOfRays = 10;


    private float timer = 0.0f;
    private float rotationCD = 4.0f;
    private float currentAngle = 0.0f;
    private float targetAngle = 0.0f;
    private float rotationSpeed = 25.0f;//how fast rotation happpens
    private bool pauseRotation = false; 
    float startingAngle = 0.0f;//needed to take into the account initial on level rotation
    private float rotationTick = 0.0f; //max change in angle per deltaTime


    private float[] angles = { 0.0f, 45.0f, 90.0f, 135.0f, 180.0f };
    private int index = 0; //index of the angle 

    bool playerInViewRange = false;
    public void Enter() {
        Debug.Log("Entering Patrol State");
        RayOrigin = transform.position;
        RayDirection = new Vector3(10.0f, 0.0f, 0.0f);
        
        startingAngle = transform.rotation.eulerAngles.y; //using euler because i need degrees and not radians
        currentAngle = startingAngle;

        targetAngle = angles[1] + startingAngle;


        //for the state machine
        trapChestStateMachine = GetComponent<TrapChestStateMachine>();
        chasePlayerState = gameObject.AddComponent<ChasePlayerState>();

    }
    public void ExecuteState()
    {
        Rotate();
        CastRays();

        if (PlayerFound())
        {
            trapChestStateMachine.ChangeState(chasePlayerState);
            //trapChestStateMachine.ChangeState(GetComponent<ChasePlayerState>());
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
   

    void Start()
    {
        trapChestStateMachine = gameObject.AddComponent<TrapChestStateMachine>();
        patrolState = gameObject.AddComponent<PatrolRotateState>();
      
        trapChestStateMachine.ChangeState(patrolState);
    }

 
    void Update()
    {

        trapChestStateMachine.Update();
  
    }


}