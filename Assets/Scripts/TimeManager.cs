using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using static TimeManager;
 
public struct SpecialEvent
{
    private TimeManager.TimePeriod TimePeriod;
    private TimeManager.Day Day;
    private TimeManager.Week Week;
    private TimeManager.Season Season;

    public SpecialEvent(TimeManager.TimePeriod TimePeriod_, TimeManager.Day Day_, TimeManager.Week Week_, TimeManager.Season Season_)
    {
        TimePeriod = TimePeriod_;
        Day = Day_;
        Week = Week_;
        Season = Season_;
    }

    public override bool Equals(object obj)
    {
        if (obj is SpecialEvent other)
        {
            return TimePeriod == other.TimePeriod &&
                   Day == other.Day &&
                   Week == other.Week &&
                   Season == other.Season;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(TimePeriod, Day, Week, Season);
    }
}

public class TimeManager : MonoBehaviour
{
    //Morning, Noon, Evening and Night are 4 times of day with EndPeriod being a more restricted time.
    public enum TimePeriod{ Morning, Noon, Evening, Night, EndPeriod };
    int numTimePeriods = 5;

    public enum Day{ Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday};
    int numDays = 7;

    public enum Week { First, Second, Third, Fourth };
    int numWeeks = 4;

    public enum Season { Spring, Summer, Autumn, Winter };
    int numSeasons = 4;

    public int totalDays = 1;

    public TimePeriod currentTimeBlock = TimePeriod.Morning;
    public Day currentDay = Day.Monday;
    public Week currentWeek = Week.First;
    public Season currentSeason = Season.Spring;

    public Dictionary<SpecialEvent, Action> specialEvents = new Dictionary<SpecialEvent, Action>();

    public Dictionary<SpecialEvent, Dictionary<string, NPCBehavior>> calendar = new Dictionary<SpecialEvent, Dictionary<string, NPCBehavior>>();

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

        specialEvents[new SpecialEvent(TimePeriod.Noon, Day.Monday, Week.First, Season.Spring)] = () =>
        {
            Debug.Log("Event 1 Test");

            Dictionary<string, NPCBehavior> event1 = new Dictionary<string, NPCBehavior>();

            NPCBehavior test;
            List<Vector3> testList = new List<Vector3>();
            testList.Add(new Vector3(-13.53f, 0, -23.65f));
            testList.Add(new Vector3(0, 0, -22.55f));
            testList.Add(new Vector3(-5.69f, 0, -36.71f));
            test.patrolWaypoints = testList;

            event1.Add("linfordTest", test);

            calendar[new SpecialEvent(TimePeriod.Noon, Day.Monday, Week.First, Season.Spring)] = event1;
        };

        specialEvents[new SpecialEvent(TimePeriod.Evening, Day.Friday, Week.First, Season.Spring)] = TestEvent;
    }

    private void Start() 
    {
        

    }
    private void TestEvent()
    {
        Debug.Log("Event 2 Test");
    }

    private void CheckIfSpecialEvent()
    {
        SpecialEvent currentTime = new SpecialEvent(currentTimeBlock, currentDay, currentWeek, currentSeason);
        if (specialEvents.TryGetValue(currentTime, out Action specialEvent))
        {
            specialEvent.Invoke();
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
        SpecialEvent currentTime = new SpecialEvent(currentTimeBlock, currentDay, currentWeek, currentSeason);
        Dictionary<string, NPCBehavior> NPCBehaviors;
        calendar.TryGetValue(currentTime, out NPCBehaviors);

        NPCBehavior behavior;
        NPCBehaviors.TryGetValue(id, out behavior);

        return behavior;
    }

}
