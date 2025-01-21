using System.Collections.Generic;
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

    private void Start()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // just for testing
        agent.destination = NPCtarget.position;
        bool isMoving = agent.remainingDistance > agent.stoppingDistance;

        if (!isMoving)
        {
            //check if waypoints aren't empty, choose random waypoint
            if (waypoints.Count > 0)
            {
                int randomIndex = Random.Range(0, waypoints.Count);
                NPCtarget.position = waypoints[randomIndex];
            }
        }

        anim.SetBool("isWalking", isMoving);

    }

}