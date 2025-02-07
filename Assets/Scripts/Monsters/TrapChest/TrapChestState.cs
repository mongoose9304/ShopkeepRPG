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
    private float viewDistance = 10.0f;
    private float viewAngle = 90.0f;
    private int amountOfRays = 15;


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


        startingAngle = transform.rotation.eulerAngles.y; //using euler because i need degrees and not radians
        currentAngle = startingAngle;

        targetAngle = angles[1] + startingAngle;
        RayOrigin = transform.position;
        RayDirection = Quaternion.Euler(0, currentAngle, 0) * transform.forward;

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


    void CastRays()
    {
        RayOrigin = transform.position;
        //this is to ganerate the cone- like field of view. 
        //Since for this mob we cant use simple colliders

        float startingPoint = -viewAngle / 2;
        float angleBetweenRays = viewAngle / amountOfRays;

        for (int i = 0; i < amountOfRays; i++)
        {
            float newAngle = startingPoint + angleBetweenRays * i;
            RayDirection = Quaternion.Euler(0, newAngle, 0) * transform.forward;

            Debug.DrawRay(RayOrigin, RayDirection * viewDistance, Color.red);

            RaycastHit[] hit = Physics.RaycastAll(RayOrigin, RayDirection * viewDistance, viewDistance);
            for (int j = 0; j < hit.Length; j++)
            {
                if (hit[j].collider.CompareTag("Player"))
                {
                    Debug.Log("Player Found!");
                    playerInViewRange = true;
                    break;
                }
            }
        }
    }


    bool PlayerFound() { return playerInViewRange; }
}


public class ChasePlayerState : MonoBehaviour, IState {

    LootManager playerLoot;
    float chaseTime = 5.0f;
    float timer = 0.0f;
    float chaseSpeed = 1.0f;

    float runAway = 3.0f;
    float minCloseDistance = 1.0f; 
    bool playerCaught = false; 

    Transform playerTransform;
    public void Enter() {
        Debug.Log("Entering Chase State");
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        playerTransform = playerObject.transform;
        playerLoot = LootManager.instance;
        if (playerLoot == null)
        {
            Debug.LogError("LootManager component not found on the player object!");
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Chase State");
    }

    public void ExecuteState()
    {
        if (timer <= chaseTime && playerCaught == false)
        {
            timer += Time.deltaTime;
            //Debug.Log(timer);
            if (Vector3.Distance(transform.position, playerTransform.position) < minCloseDistance)
            {
                Debug.Log("Player Caught");
           
             
                LootItem stolenItem = playerLoot.StealAnItem();
                if (stolenItem != null)
                {
                    Debug.Log($"Stole {stolenItem.amount} {stolenItem.name}!");
                }
                playerCaught = true;
            }
            StartChase();
        }
        else {
            if (timer >= chaseTime) {
                if (timer <= chaseTime + 5.0f) {
                    RunAway();
                }
              
            }
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 3.0f);

        }
    }

    public void StartChase() { 
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;
        Quaternion facePlayer = Quaternion.LookRotation(direction);
        transform.rotation = facePlayer;
    }

    public void RunAway() {
        Vector3 direction = (transform.position- playerTransform.position).normalized;
        transform.position += direction * chaseSpeed* 1.5f * Time.deltaTime;
        Quaternion faceAway = Quaternion.LookRotation(-direction);
        transform.rotation = faceAway;
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