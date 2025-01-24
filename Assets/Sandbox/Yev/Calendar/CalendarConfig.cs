using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CalendarConfig", menuName = "ScriptableObjects/CalendarConfig")]
public class CalendarConfig : ScriptableObject
{
    [Header("Weeks")]
    public WeekObject Spring;
    public WeekObject Summer;
    public WeekObject Autumn;
    public WeekObject Winter;
}

public enum Day { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday };

public enum Week { First, Second, Third, Fourth };

public enum Season { Spring, Summer, Autumn, Winter };
