using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Week", menuName = "Calendar/Week")]
[Serializable]
public class WeekObject: ScriptableObject
{
    [Header("Days")]
    public DayObject[] Days;

}



