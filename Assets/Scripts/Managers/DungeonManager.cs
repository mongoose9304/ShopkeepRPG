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
    public List<BasicCurse> currentCurses;
    [SerializeField] List<BasicCurse> availableCurses;
    private void Awake()
    {
        instance = this;
        
    }
    private void Start()
    {
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
    }
    public void NextLevel(SinType sin_)
    {
        dungeonsCleared += 1;
        if(dungeonsCleared>dungeonList.Count)
        {
            WinLevel();
            return;
        }
        currentSin = sin_;
        Destroy(currentDungeon.gameObject);
      BasicDungeon d=  GameObject.Instantiate(dungeonList[dungeonsCleared].gameObject).GetComponent<BasicDungeon>();
        ChangeLevel(d);
        CombatPlayerManager.instance.MovePlayers(currentDungeon.playerStart);
        surface.BuildNavMesh();

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
        TextPopUpManager.instance.AddText(curse_.description, curse_.name);
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
    public void WinLevel(bool hasLost_=false)
    {
        CombatPlayerManager.instance.GetPlayer(0).gameObject.GetComponent<CombatPlayerMovement>().enabled = false;
        CombatPlayerManager.instance.GetPlayer(0).enabled = false;
        victoryLevel.SetActive(true);
        CombatPlayerManager.instance.GetPlayer(0).gameObject.transform.position = victoryPlayerPos.transform.position;
        CombatPlayerManager.instance.GetPlayer(0).gameObject.transform.rotation = victoryPlayerPos.transform.rotation;
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
