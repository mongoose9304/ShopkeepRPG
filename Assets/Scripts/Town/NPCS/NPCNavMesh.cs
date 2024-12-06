using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCNavMesh : MonoBehaviour
{
    private NavMeshAgent agent;

    // testing for now, this will be every location in the game
    // need to make a schedule for the npcs
    [SerializeField]
    private List<GameObject> waypoints = new List<GameObject>();

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private Transform NPCtarget;

    private void Start() {
        if (anim == null) {
            anim = GetComponent<Animator>();
        }
    }

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        // just for testing
        agent.destination = NPCtarget.position;
        bool isMoving = agent.remainingDistance > agent.stoppingDistance;
        
        if (!isMoving){
            //check if waypoints aren't empty, choose random waypoint
            if (waypoints.Count > 0) {
                int randomIndex = Random.Range(0, waypoints.Count);
                NPCtarget.position = waypoints[randomIndex].transform.position;
            }
        }

        anim.SetBool("isWalking", isMoving);

    }

}
