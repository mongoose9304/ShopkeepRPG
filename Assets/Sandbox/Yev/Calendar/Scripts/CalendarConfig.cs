using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CalendarConfig", menuName = "Calendar/CalendarConfig")]
public class CalendarConfig : ScriptableObject
{
    public WeekObject[] Seasons;
    public SpecialEvent[] SpecialEvents;
}

public enum Day { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday };

public enum Week { First, Second, Third, Fourth };

public enum Season { Spring, Summer, Autumn, Winter };
