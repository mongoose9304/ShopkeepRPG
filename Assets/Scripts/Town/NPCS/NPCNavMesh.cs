using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCNavMesh : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField]
    private Transform NPCtarget;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        // just for testing
        agent.destination = NPCtarget.position;
    }

}
