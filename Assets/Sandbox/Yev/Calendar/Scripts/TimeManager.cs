using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private CalendarConfig calendarConfig;

    int numTimePeriods = 5;
    int numDays = 7;
    int numWeeks = 4;
    int numSeasons = 4;

    public int totalDays = 1;

    public TimePeriod currentTimeBlock = TimePeriod.Morning;
    public Day currentDay = Day.Monday;
    public Week currentWeek = Week.First;
    public Season currentSeason = Season.Spring;

    public static TimeManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void TestEvent()
    {
        Debug.Log("Event 2 Test");
    }

    private void ProgressTimeByAmount(int amount){
        if(currentTimeBlock == TimePeriod.EndPeriod) 
        {
            ProgressDay(1);
            return;
        }
        int newTimeBlock = (int)currentTimeBlock + amount;
        newTimeBlock = Mathf.Clamp(newTimeBlock, 0, numTimePeriods - 1);
        currentTimeBlock = (TimePeriod)newTimeBlock;      
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
            int newDay = (int)currentDay + 1;
            currentDay = (Day)newDay;
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
            return;
        }
        int newWeek = (int)currentWeek + 1;
        currentWeek = (Week)newWeek;
        currentDay = Day.Monday;
        currentTimeBlock = TimePeriod.Morning;
    }

    private void ProgressSeason()
    {
        if (currentSeason == Season.Winter)
        {
            currentSeason = Season.Spring;
        }
        else 
        {
            int newSeason = (int)currentSeason + 1;
            currentSeason = (Season)newSeason;
        }
        currentWeek = Week.First;
        currentDay = Day.Monday;
        currentTimeBlock = TimePeriod.Morning;
    }

    public void PassTime() 
    {
        ProgressTimeByAmount(1);
    }

    public NPCBehavior GetBehavior(string id) 
    {
        foreach(var specialEvent in calendarConfig.SpecialEvents) 
        {
            if(specialEvent.timePeriod == currentTimeBlock && specialEvent.day == currentDay && specialEvent.week == currentWeek && specialEvent.season == currentSeason) 
            {
                foreach (var npc in specialEvent.NPC)
                {
                    if (npc.ID == id)
                    {
                        return npc;
                    }
                }
            }
        }

        foreach (var npc in calendarConfig.Seasons[(int)currentSeason].Days[(int)currentWeek].TimeBlocks[(int)currentDay].NPC)
        {
            if(npc.ID == id) 
            {
                return npc;
            }
        }
        return null;
    }

}
