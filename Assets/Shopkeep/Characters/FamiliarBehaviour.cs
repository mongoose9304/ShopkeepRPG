using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum FamiliarState { Idle, FollowPlayer, ChaseEnemy, Melee, CastSpell};

public class FamiliarBehaviour : MonoBehaviour {
    //actions:
    //Follow player
    //Chase enemy
    //Attack enemy

    [Header("Properties")]
    public float moveSpeed;
    /// <summary>
    /// How far the familiar can be from the player
    /// </summary>
    public float maxDistanceFromPlayer;
    /// <summary>
    /// How close the familiar can be from the player
    /// </summary>
    public float minDistanceFromPlayer;
    /// <summary>
    /// The detection radius to find and attack a target.
    /// Currently the familiar prioritizes the closes enemy for the familiar
    /// </summary>
    public float targetRadius;
    float meleeRange;
    /// <summary>
    /// The range of the melee range
    /// </summary>

    NavMeshAgent agent;
    GameObject target;
    FamiliarState currentState = FamiliarState.Idle;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        FindTarget();
    }

    void FindTarget() {
        target = null;
        //Sphere cast
        RaycastHit[] hits =  Physics.SphereCastAll(transform.position, targetRadius, Vector3.up);
        foreach (RaycastHit hit in hits) {
            //If it is an enemy, set it as the target
            Enemy e = hit.collider.gameObject.GetComponent<Enemy>();
            if(e != null) {
                target = hit.collider.gameObject;
                break;
            }
        }
    }

    void EvaluateState() {
        //Set to idle by default
        currentState = FamiliarState.Idle;

        //Killing enemies is higher in the priority list
        if (target != null) {
            currentState = FamiliarState.ChaseEnemy;
            float distanceToTarget = (target.transform.position - transform.position).magnitude;

            //If the distance is within melee range, then melee
            if (distanceToTarget <= meleeRange) {
                currentState = FamiliarState.Melee;
            }
        }

        //Check if the 

    }
}
