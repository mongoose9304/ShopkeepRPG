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

    public Dictionary<CalendarEvent, Action> calendarEvents = new Dictionary<CalendarEvent, Action>();

    public Dictionary<CalendarEvent, Dictionary<string, NPCBehavior>> calendar = new Dictionary<CalendarEvent, Dictionary<string, NPCBehavior>>();

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

        /*
        To add a special event to the calendar do this:

        specialEvents[new SpecialEvent(TimePeriod.Noon, Day.Monday, Week.First, Season.Spring)] = () =>
        {
            Debug.Log("Test Event");
            ...
        }; 

        Alternatively:

        specialEvents[new TimeEventKey(TimePeriod.Noon, Day.Monday, Week.First, Season.Spring)] = YourFunctionHere;
         */

        calendarEvents[new CalendarEvent(TimePeriod.Noon, Day.Monday, Week.First, Season.Spring)] = () =>
        {
            Debug.Log("Event 1 Test");
        };

        calendarEvents[new CalendarEvent(TimePeriod.Evening, Day.Friday, Week.First, Season.Spring)] = TestEvent;
    }

    private void TestEvent()
    {
        Debug.Log("Event 2 Test");
    }

    private void CheckIfSpecialEvent()
    {
        CalendarEvent currentTime = new CalendarEvent(currentTimeBlock, currentDay, currentWeek, currentSeason);
        if (calendarEvents.TryGetValue(currentTime, out Action calendarEvent))
        {
            calendarEvent.Invoke();
        }
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

    //public functions

    public void PassTime() 
    {
        ProgressTimeByAmount(1);
        CheckIfSpecialEvent();
    }

    public NPCBehavior GetBehavior(string id) 
    {
        foreach (var calEvent in calendarConfig.events)
        {
            if (calEvent.timePeriod == currentTimeBlock && calEvent.day == currentDay && calEvent.week == currentWeek && calEvent.season == currentSeason)
            {
                foreach (var npc in calEvent.NPC)
                {
                    if (npc.ID == id)
                    {
                        return npc; 
                    }
                }
            }
        }
        return null;
    }

}
