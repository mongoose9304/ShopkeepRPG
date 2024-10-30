using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AvoidTargetNavAgent : MonoBehaviour
{
    public NavMeshAgent myAgent;
    public GameObject target;
    public float minDistanceFromTarget;

    private void Update()
    {
        if(Vector3.Distance(transform.position,target.transform.position)<minDistanceFromTarget)
        {

        }
    }
}
