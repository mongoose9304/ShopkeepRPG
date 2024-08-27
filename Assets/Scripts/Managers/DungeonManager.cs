using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.AI.Navigation;

public class DungeonManager : MonoBehaviour
{
    public NavMeshSurface surface;
    public BasicDungeon currentDungeon;
    [SerializeField] int dungeonsCleared;
    public List<BasicDungeon> dungeonList = new List<BasicDungeon>();
    public SinType currentSin;
    public GameObject victoryLevel;
    public GameObject victoryPlayerPos;
    [Tooltip("Sprites for collected resources ")]
    public List<Sprite> resourceSprites = new List<Sprite>();
    public static DungeonManager instance;
    public Image[] curseImages;
    public Image[] blessingImages;
    public Sprite[] SinSprites;
    public List<BasicCurse> currentCurses;
    public List<BasicCurse> currentBlessings;
    [SerializeField] List<BasicCurse> availableCurses;
    [SerializeField] List<BasicCurse> availableSinBlessings;
    private void Awake()
    {
        instance = this;
        
    }
    private void Start()
    {
        ClearBlessings();
        NextLevel(SinType.Greed);
    }

    public int GetEnemyLevel() { return currentDungeon.enemyLevel; }
    public int GetEliteEnemyLevel() { return currentDungeon.eliteEnemyLevel; }


    public void ChangeLevel(BasicDungeon dungeon_)
    {
        currentDungeon = dungeon_;
        currentDungeon.SetUpEnemies();
        currentDungeon.ChangeSin(currentSin);
        ClearCurses();
        if(CombatPickupManager.instance)
        {
            CombatPickupManager.instance.ClearPickups();
        }
        EnemyManager.instance.DisableAllEnemies();


    }
    public void NextLevel(SinType sin_)
    {
        dungeonsCleared += 1;
        if(dungeonsCleared>=dungeonList.Count)
        {
            WinLevel();
            return;
        }
        currentSin = sin_;
        if(currentDungeon)
        Destroy(currentDungeon.gameObject);
        StartCoroutine(WaitAFrameBeforeMoving());

    }
    IEnumerator WaitAFrameBeforeMoving()
    {
        yield return new WaitForSeconds(0.001f);
        BasicDungeon d = GameObject.Instantiate(dungeonList[dungeonsCleared].gameObject).GetComponent<BasicDungeon>();
        ChangeLevel(d);
        yield return new WaitForSeconds(0.201f);
        surface.BuildNavMesh();
        yield return new WaitForSeconds(0.01f);
        AddSinBlessing(currentSin);
        CombatPlayerManager.instance.MovePlayers(currentDungeon.playerStart);
        CombatPlayerManager.instance.ReturnFamiliars();
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
    public void ClearBlessings()
    {
        for (int i = 0; i < blessingImages.Length; i++)
        {
            blessingImages[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < currentCurses.Count; i++)
        {
            CurseEffect(blessingImages[i].name, true);
        }
        currentBlessings.Clear();
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
        foreach (BasicCurse c_ in currentCurses)
        {
            if (c_.name == curse_.name)
                return;
        }
        currentCurses.Add(curse_);
        curseImages[currentCurses.Count - 1].sprite = currentCurses[currentCurses.Count - 1].icon;
        curseImages[currentCurses.Count - 1].gameObject.SetActive(true);
        CurseEffect(curse_.name);
        TextPopUpManager.instance.AddText(curse_.description, curse_.name);
    }

    public void AddBlessing(BasicCurse curse_)
    {
        foreach(BasicCurse c_ in currentBlessings)
        {
            if (c_.name == curse_.name)
                return;
        }
        currentBlessings.Add(curse_);
        blessingImages[currentBlessings.Count - 1].sprite = currentBlessings[currentBlessings.Count - 1].icon;
        blessingImages[currentBlessings.Count - 1].gameObject.SetActive(true);
        CurseEffect(curse_.name);
        TextPopUpManager.instance.AddText(curse_.description, curse_.name);
    }
    public void AddSinBlessing(SinType sin_)
    {
        foreach(BasicCurse c_ in availableSinBlessings)
        {
            if(c_.name==sin_.ToString())
            {
                AddBlessing(c_);
            }
        }
    }

    private void CurseEffect(string name_,bool removeCurse=false)
    {
        if (!removeCurse)
        {
            switch (name_)
            {
                //curses
                case "Poverty":
                    LootManager.instance.AddToCashMultiplier(-0.2f);
                    break;
                case "Slow":
                    GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayerMovement>().moveSpeedModifier -= 0.2f;
                    break;

                    //blessings
                case "Greed":
                    LootManager.instance.AddToCashMultiplier(0.25f);
                    break;
                case "Sloth":
                    GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayerMovement>().moveSpeedModifier += 0.25f;
                    break;

            }
        }
        else
        {
            switch (name_)
            {
                //curses
                case "Poverty":
                    LootManager.instance.AddToCashMultiplier(0.2f);
                    break;
                case "Slow":
                    GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayerMovement>().moveSpeedModifier += 0.2f;
                    break;
                    //blessings  
                case "Greed":
                    LootManager.instance.AddToCashMultiplier(-0.25f);
                    break;
                case "Sloth":
                    GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayerMovement>().moveSpeedModifier -= 0.25f;
                    break;

            }
        }
    }
    public void WinLevel(bool hasLost_=false)
    {
        CombatPlayerManager.instance.GetPlayer(0).gameObject.GetComponent<CombatPlayerMovement>().enabled = false;
        CombatPlayerManager.instance.GetPlayer(0).enabled = false;
        victoryLevel.SetActive(true);
        //CombatPlayerManager.instance.GetPlayer(0).gameObject.transform.position = victoryPlayerPos.transform.position;
        //CombatPlayerManager.instance.GetPlayer(0).gameObject.transform.rotation = victoryPlayerPos.transform.rotation;
        if(LootManager.instance.AquiredLootItems.Count>=1)
        LootDisplayManager.instance.AddItems(LootManager.instance.AquiredLootItems);
        List<int> x = new List<int>();
        //here is where you would load the inventorys count of how many resources *demon cash* you have.
        List<int> y = new List<int>();
        y.Add(1000);
        x.Add(LootManager.instance.demonCurrentCash);
        LootDisplayManager.instance.AddResources(x, y, resourceSprites);
        LootDisplayManager.instance.StartVictoryScreen(hasLost_);
        CombatPlayerManager.instance.EnableFamiliars(false);

    }
}
