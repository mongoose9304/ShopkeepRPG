using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialEvent", menuName = "Calendar/SpecialEvent")]
public class SpecialEvent : ScriptableObject
{
    public TimePeriod timePeriod;
    public Day day;
    public Week week;
    public Season season;
    public NPCBehavior[] NPC;   
}

