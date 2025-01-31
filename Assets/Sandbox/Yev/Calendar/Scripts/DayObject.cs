using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Day", menuName = "Calendar/Day")]
[Serializable]
public class DayObject : ScriptableObject
{
    [Header("TimeBlocks")]
    public TimeBlockObject[] TimeBlocks;
}


public enum TimePeriod { Morning, Noon, Evening, Night, EndPeriod }

