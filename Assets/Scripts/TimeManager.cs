using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeManager;

public class TimeManager : MonoBehaviour
{
    public enum TimePeriod{ Morning, Noon, Evening, Night, EndPeriod };
    int numTimePeriods = 5;

    public enum Day{ Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday};
    int numDays = 7;

    public enum Week { First, Second, Third, Fourth };
    int numWeeks = 4;

    public enum Season { Spring, Summer, Autumn, Winter };
    int numSeason = 4;

    public int totalDays = 0;

    public TimePeriod currentTimeBlock = TimePeriod.Morning;
    public Day currentDay = Day.Sunday;
    public Week currentWeek = Week.First;
    public Season currentSeason = Season.Spring;


    private void ProgressTime(int amount){
        if(currentTimeBlock == TimePeriod.EndPeriod) 
        {
            ProgressDay(1);
            return;
        }
        int newProgress = (int)currentTimeBlock + amount;
        newProgress = Mathf.Clamp(newProgress, 0, numTimePeriods - 1);
        currentTimeBlock = (TimePeriod)newProgress;      
    }

    private void ProgressDay(int days) 
    {
        totalDays += days;
        int allocatedDays = 0;
        while(allocatedDays < days) 
        {
            if (currentDay == Day.Sunday)
            {
                currentDay = Day.Monday;
                ProgressWeek();
                allocatedDays++;
                continue;
            }
            int newProgress = (int)currentDay + 1;
            currentDay = (Day)newProgress;
            allocatedDays++;
        }
        currentTimeBlock = TimePeriod.Morning;
    }

    private void ProgressWeek()
    {
        if(currentWeek == Week.Fourth) 
        {
            currentWeek = Week.First;
            ProgressSeason();
        }

        int newProgress = (int)currentWeek + 1;
        int newProgressClamp = Mathf.Clamp(newProgress, 0, numWeeks - 1);

        currentWeek = (Week)newProgress;
        currentDay = Day.Monday;
        currentTimeBlock = TimePeriod.Morning;
    }

    private void ProgressSeason()
    {
        if (currentSeason == Season.Winter)
        {
            currentSeason = Season.Spring;
            return;
        }

        int newProgress = (int)currentSeason + 1;

        currentSeason = (Season)newProgress;
        currentWeek = Week.First;
        currentDay = Day.Monday;
        currentTimeBlock = TimePeriod.Morning;
    }

    public void PassTime() 
    {
        ProgressTime(1);
    }
}
