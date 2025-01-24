using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Week", menuName = "ScriptableObjects/Week")]
[Serializable]
public class WeekObject: ScriptableObject
{
    [Header("Days")]
    public DayObject Monday;
    public DayObject Tuesday;
    public DayObject Wednesday;
    public DayObject Thursday;
    public DayObject Friday;
    public DayObject Saturday;
    public DayObject Sunday;
}



