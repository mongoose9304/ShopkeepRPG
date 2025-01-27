using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Day", menuName = "ScriptableObjects/Day")]
[Serializable]
public class DayObject : ScriptableObject
{
    [Header("TimeBlocks")]
    public TimeBlockObject Morning;
    public TimeBlockObject Noon;
    public TimeBlockObject Evening;
    public TimeBlockObject Night;
}


public enum TimePeriod { Morning, Noon, Evening, Night, EndPeriod }

