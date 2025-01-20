using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private string id;
    private NPCNavMesh navMesh;
    private NPCBehavior behavior;

    void Start()
    {
        navMesh = gameObject.GetComponent<NPCNavMesh>();
        CheckSchedule();
    }
    public void CheckSchedule() 
    {
        behavior = TimeManager.instance.GetBehavior(id);
        gameObject.transform.position = behavior.patrolWaypoints[0];
        navMesh.waypoints = behavior.patrolWaypoints;
    }
}
