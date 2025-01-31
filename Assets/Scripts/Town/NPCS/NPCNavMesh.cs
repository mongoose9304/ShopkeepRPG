using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class NPCNavMesh : MonoBehaviour
{
    private NavMeshAgent agent;

    public List<Vector3> waypoints = new List<Vector3>();

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private Transform NPCtarget;

    [SerializeField]
    private float waitTime = 2f;

    private bool isWaiting = false;

    public int indexPath = 1;

    private void Start()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }

    public void SetupNPC() 
    {
        isWaiting = false;
        NPCtarget.position = waypoints[indexPath];
        indexPath = 1;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!isWaiting) 
        {
            agent.destination = NPCtarget.position;
            bool isMoving = agent.remainingDistance > agent.stoppingDistance;
            anim.SetBool("isWalking", isMoving);

            if (!isMoving)
            {
                if (waypoints.Count > 0)
                {
                    StartCoroutine(Move());
                }
            }
        } 
    }

    private IEnumerator Move()
    {
        isWaiting = true;
   
        yield return new WaitForSeconds(waitTime);
        
        NPCtarget.position = waypoints[indexPath];
        agent.SetDestination(NPCtarget.position);
        indexPath += 1;
        if (indexPath >= waypoints.Count)
        {
            indexPath = 0;
        }

        isWaiting = false;
    }

}