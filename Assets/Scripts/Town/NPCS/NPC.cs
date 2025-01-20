using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private string id;
    private NPCNavMesh navMesh;
    private NPCBehavior behavior;
    private DialogueSystemTrigger trigger;

    void Start()
    {
        navMesh = gameObject.GetComponent<NPCNavMesh>();
        trigger = gameObject.GetComponent<DialogueSystemTrigger>();
        CheckSchedule();
    }
    public void CheckSchedule() 
    {
        behavior = TimeManager.instance.GetBehavior(id);
        gameObject.transform.position = behavior.patrolWaypoints[0];
        trigger.conversation = behavior.conversationName;
        navMesh.waypoints = behavior.patrolWaypoints;
    }
}
