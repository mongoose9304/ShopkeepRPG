using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    public BasicDungeon currentDungeon;

    public static DungeonManager instance;
    public Image[] curseImages;
    public List<BasicCurse> currentCurses;
    [SerializeField] List<BasicCurse> availableCurses;
    private void Awake()
    {
        instance = this;
        
    }
    private void Start()
    {
        ChangeLevel(currentDungeon);
    }

    public int GetEnemyLevel() { return currentDungeon.enemyLevel; }
    public int GetEliteEnemyLevel() { return currentDungeon.eliteEnemyLevel; }


    public void ChangeLevel(BasicDungeon dungeon_)
    {
        currentDungeon = dungeon_;
        currentDungeon.SetUpEnemies();
        ClearCurses();
    }
    public void ClearCurses()
    {
        for(int i=0;i <curseImages.Length;i++)
        {
            curseImages[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < currentCurses.Count; i++)
        {
            CurseEffect(currentCurses[i].name,true);
        }
        currentCurses.Clear();
    }
    public void AddRandomCurse(int severity_)
    {
        if (currentCurses.Count == availableCurses.Count)
            return;
        int sev = 99;
        int x = Random.Range(0, availableCurses.Count);
       while (sev>severity_)
        {
            while (currentCurses.Contains(availableCurses[x]))
            {
                x = Random.Range(0, availableCurses.Count);
            }
            sev = availableCurses[x].severity;
        }
        AddCurse(availableCurses[x]);


    }
    public void AddCurse(BasicCurse curse_)
    {
        currentCurses.Add(curse_);
        curseImages[currentCurses.Count - 1].sprite = currentCurses[currentCurses.Count - 1].icon;
        curseImages[currentCurses.Count - 1].gameObject.SetActive(true);
        CurseEffect(curse_.name);
    }


    private void CurseEffect(string name_,bool removeCurse=false)
    {
        if (removeCurse)
        {
            switch (name)
            {
                case "Poverty":
                    LootManager.instance.AddToCashMultiplier(-0.2f);
                    break;
                case "Slow":
                    GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayerMovement>().moveSpeedModifier -= 0.2f;
                    break;

            }
        }
        else
        {
            switch (name)
            {
                case "Poverty":
                    LootManager.instance.AddToCashMultiplier(0.2f);
                    break;
                case "Slow":
                    GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayerMovement>().moveSpeedModifier += 0.2f;
                    break;

            }
        }
    }
}
