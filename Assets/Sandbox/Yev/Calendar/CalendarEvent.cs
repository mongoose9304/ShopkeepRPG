using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CalendarEvent
{
    public TimePeriod timePeriod;
    public Day day;
    public Week week;
    public Season season;

    public CalendarEvent(TimePeriod timePeriod, Day day, Week week, Season season)
    {
        this.timePeriod = timePeriod;
        this.day = day;
        this.week = week;
        this.season = season;
    }

    // Override GetHashCode and Equals to use as dictionary keys.
    public override bool Equals(object obj)
    {
        if (obj is CalendarEvent other)
        {
            return timePeriod == other.timePeriod &&
                   day == other.day &&
                   week == other.week &&
                   season == other.season;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(timePeriod, day, week, season);
    }
}
