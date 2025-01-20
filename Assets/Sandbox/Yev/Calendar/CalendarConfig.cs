using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CalendarConfig", menuName = "ScriptableObjects/CalendarConfig")]
public class CalendarConfig : ScriptableObject
{
    [Header("Calendar Events")]
    public List<EventConfig> events = new List<EventConfig>(); 
}

[Serializable]
public class EventConfig
{
    public string ID;

    [Header("Time Slot")]
    public TimePeriod timePeriod;   
    public Day day;                 
    public Week week;               
    public Season season;           

    [Header("NPC Behaviors")]
    public List<NPCBehavior> NPC = new List<NPCBehavior>(); 
}

[Serializable]
public class NPCBehavior
{
    public string ID; 

    [Header("Behavior")]
    public List<Vector3> patrolWaypoints = new List<Vector3>(); 
}

public enum TimePeriod{Morning, Noon, Evening, Night, EndPeriod}

public enum Day { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday };

public enum Week { First, Second, Third, Fourth };

public enum Season { Spring, Summer, Autumn, Winter };
