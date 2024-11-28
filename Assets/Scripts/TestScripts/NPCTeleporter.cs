using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCTeleporter : MonoBehaviour
{
    public GameObject location;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        // check if the collision is activated by a gameobject tagged with NPC

        if (other.gameObject.CompareTag("NPC")) {
            // use navmesh warp fuction on the npc to warp them to the location position
            other.gameObject.GetComponent<NavMeshAgent>().Warp(location.transform.position);
        }
    }
}
