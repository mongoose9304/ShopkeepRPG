using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeBlock", menuName = "ScriptableObjects/TimeBlock")]
[Serializable]
public class TimeBlockObject : ScriptableObject
{
    [Header("NPC Behaviors")]
    public List<NPCBehavior> NPC = new List<NPCBehavior>();
}

[Serializable]
public class NPCBehavior
{
    public string ID;

    [Header("Behavior")]
    public string conversationName;
    public List<Vector3> patrolWaypoints = new List<Vector3>();
}
