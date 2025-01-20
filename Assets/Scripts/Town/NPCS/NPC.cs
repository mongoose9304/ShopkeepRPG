using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct NPCBehavior 
{
    public List<GameObject> patrolWaypoints;
}

public class NPC : MonoBehaviour
{
    [SerializeField]
    private string id;
    private NPCNavMesh navMesh;
    private NPCBehavior behavior;

    void Start()
    {
        navMesh = gameObject.GetComponent<NPCNavMesh>();

        //gameObject.transform.position = behavior.patrolWaypoints[0].transform.position;       
    }

    void Update()
    {
        
    }
}
