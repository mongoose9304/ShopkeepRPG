using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.AI.Navigation;
using MoreMountains.Tools;

/// <summary>
/// The singleton class that will handle loading the dungeon levels, winning and losing the game and Player buffs/Debuffs
/// </summary>
public class DungeonManager : MonoBehaviour
{
    public bool in2PlayerMode;
    [Tooltip("The singleton instance")]
    public static DungeonManager instance;
    [Tooltip("The singleton instance")]
    public Transform levelSpawn;
    [Tooltip("The singleton instance")]
    public bool inArenaMode;
    [Tooltip("The navmesh must be stored like this so we can rebuild it when loading new levels")]
    public NavMeshSurface surface;
    [Tooltip("The dungeon that is currently being played")]
    public BasicDungeon currentDungeon;
    [Tooltip("How many dungeons you have cleared this session")]
    [SerializeField] int dungeonsCleared;
    [Tooltip("The dungeons to load in order ")]
    public List<BasicDungeon> dungeonList = new List<BasicDungeon>();
    [Tooltip("The dungeons to load in order ")]
    public BasicDungeon tutDungeon;
    [Tooltip("The current Sin, used for dynamically changing elements of the dungeon")]
    public SinType currentSin;
    [Tooltip("The current tier of Items to drop, used with lootmanager's currentItemDropList")]
    public int currentItemTier;
    [Tooltip("Sprites for collected resources ")]
    public List<Sprite> resourceSprites = new List<Sprite>();
    [Tooltip("Current Curses on Player, Reset when changing levels")]
    public List<BasicCurse> currentCurses;
    [Tooltip("Current Buffs on Player")]
    public List<BasicCurse> currentBlessings;
    [SerializeField] List<BasicCurse> availableCurses;
    [Tooltip("REFERENCE to BLESSINGS PROVIDED BY THE Sins UI")]
    [SerializeField] List<BasicCurse> availableSinBlessings;
    [Tooltip("REFERENCE to the location for displaying the win/loss cutscene")]
    public GameObject victoryLevel;
    [Tooltip("REFERENCE to slots for curses on the UI")]
    public Image[] curseImages;
    [Tooltip("REFERENCE to slots for blessings on the UI")]
    public Image[] blessingImages;
    [Tooltip("REFERENCE to sprites used to represent the sins")]
    public Sprite[] SinSprites;
    [Tooltip("REFERENCE to sprites used to represent the sins")]
    public ItemDropList[] sinItemDropLists;
    [Tooltip("REFERENCE to BGMs available")]
    public List<AudioClip> BGMs = new List<AudioClip>();
    public AudioClip tutBGM;
    private void Awake()
    {
        instance = this;
        
    }
    private void Start()
    {
        ClearBlessings();
        StartTutorial();
        //For Testing purchases, use responsibly 
        LootManager.instance.AddDemonMoney(1000);

    }
    public void StartTutorial()
    {
        MMSoundManager.Instance.StopTrack(MMSoundManager.MMSoundManagerTracks.Music);
        MMSoundManager.Instance.PlaySound(tutBGM, MMSoundManager.MMSoundManagerTracks.Music, Vector3.zero, true);
        if (currentDungeon)
            Destroy(currentDungeon.gameObject);
        StartCoroutine(WaitAFrameBeforeMovingTut());
        TutorialManager.instance_.StartTutorial();
    }

    public int GetEnemyLevel() { return currentDungeon.enemyLevel; }
    public int GetEliteEnemyLevel() { return currentDungeon.eliteEnemyLevel; }

    /// <summary>
    /// Change the level and clean up old one
    /// </summary>
    public void ChangeLevel(BasicDungeon dungeon_)
    {
        currentDungeon = dungeon_;
        currentDungeon.SetUpEnemies();
        currentDungeon.ChangeSin(currentSin);
        if(CombatPickupManager.instance)
        {
            CombatPickupManager.instance.ClearPickups();
        }
        EnemyManager.instance.DisableAllEnemies();
        currentDungeon.SetEnemyManagerTeams();

    }
    /// <summary>
    /// Advance to the next level and apply its Sin changes
    /// </summary>
    public void NextLevel(SinType sin_)
    {
        if(TutorialManager.instance_.inTut)
        {
            EnemyManager.instance.HardClearEnemyList();
            TutorialManager.instance_.EndTutorial();
        }
        CoinSpawner.instance_.ClearAllCoins();
        LootManager.instance.ClearAllLootItems();
        dungeonsCleared += 1;
        if(dungeonsCleared>=dungeonList.Count)
        {
            WinLevel();
            return;
        }
        currentSin = sin_;
        PlayRandomBGM();
        SwitchSinDrops();
        if(currentDungeon)
        Destroy(currentDungeon.gameObject);
        StartCoroutine(WaitAFrameBeforeMoving());

    }
    /// <summary>
    /// Used to wait a few frame to ensure everything loads correctly 
    /// </summary>
    IEnumerator WaitAFrameBeforeMoving()
    {
        yield return new WaitForSeconds(0.001f);
        BasicDungeon d = GameObject.Instantiate(dungeonList[dungeonsCleared].gameObject, levelSpawn).GetComponent<BasicDungeon>();
        ChangeLevel(d);
        yield return new WaitForSeconds(0.201f);
        surface.BuildNavMesh();
        yield return new WaitForSeconds(0.01f);
        AddSinBlessing(currentSin);
        CombatPlayerManager.instance.MovePlayers(currentDungeon.playerStart);
        CombatPlayerManager.instance.ReturnFamiliars();
    }
    /// <summary>
    /// Used to wait a few frame to ensure everything loads correctly 
    /// </summary>
    IEnumerator WaitAFrameBeforeMovingTut()
    {
        yield return new WaitForSeconds(0.001f);
        BasicDungeon d = GameObject.Instantiate(tutDungeon.GetComponent<BasicDungeon>());
        currentDungeon = d;
        currentDungeon.SetUpEnemies();
        if (CombatPickupManager.instance)
        {
            CombatPickupManager.instance.ClearPickups();
        }
        EnemyManager.instance.DisableAllEnemies();
        currentDungeon.SetEnemyManagerTeams();
        yield return new WaitForSeconds(0.201f);
        surface.BuildNavMesh();
        yield return new WaitForSeconds(0.01f);
        CombatPlayerManager.instance.MovePlayers(currentDungeon.playerStart);
        CombatPlayerManager.instance.ReturnFamiliars();
    }
    /// <summary>
    /// Remove all curses from the player
    /// </summary>
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
    /// <summary>
    /// Remove all blessings from the player
    /// </summary>
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
    /// <summary>
    /// Add a random curse to the player. The curse will be of a level of severity supplied 
    /// </summary>
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
    /// <summary>
    /// Add a specific curse to the player 
    /// </summary>
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
    /// <summary>
    /// Add a specific blessing to the player 
    /// </summary>
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
    /// <summary>
    /// Add the blessing for entering a specific Sin domain
    /// </summary>
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
    /// <summary>
    /// Apply or remove a curses effect, also does blessings 
    /// </summary>
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
                case "Gluttony":
                    CombatExtrenalModManager.instance.AddModToAllPlayers("Gluttony");
                    break;
                case "Pride":
                    CombatExtrenalModManager.instance.AddModToAllPlayers("Pride");
                    break;
                case "Vainglory":
                    CombatExtrenalModManager.instance.AddModToAllPlayers("Vainglory");
                    break;
                case "Capriciousness":
                    CombatExtrenalModManager.instance.AddModToAllPlayers("Capriciousness");
                    break;
                case "Lust":
                    CombatExtrenalModManager.instance.AddModToAllPlayers("Lust");
                    break;
                case "Envy":
                    LootManager.instance.lootDropRateMultiplier += 0.2f;
                    break;
                case "Wrath":
                    //Recruit monsters easier 
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
                case "Gluttony":
                    CombatExtrenalModManager.instance.RemoveModFromAllPlayers("Gluttony");
                    break;
                case "Pride":
                    CombatExtrenalModManager.instance.RemoveModFromAllPlayers("Pride");
                    break;
                case "Vainglory":
                    CombatExtrenalModManager.instance.RemoveModFromAllPlayers("Vainglory");
                    break;
                case "Capriciousness":
                    CombatExtrenalModManager.instance.RemoveModFromAllPlayers("Capriciousness");
                    break;
                case "Lust":
                    CombatExtrenalModManager.instance.RemoveModFromAllPlayers("Lust");
                    break;
                case "Envy":
                    LootManager.instance.lootDropRateMultiplier -= 0.2f;
                    break;
                case "Wrath":
                    //recruit monsters easier 
                    break;

            }
        }
    }
    /// <summary>
    /// Stop playing combat, display your score then return to the overworld
    /// </summary>
    public void WinLevel(bool hasLost_=false)
    {
        CombatPlayerManager.instance.GetPlayer(0).gameObject.GetComponent<CombatPlayerMovement>().enabled = false;
        CombatPlayerManager.instance.GetPlayer(0).enabled = false;
        victoryLevel.SetActive(true);
        //CombatPlayerManager.instance.GetPlayer(0).gameObject.transform.position = victoryPlayerPos.transform.position;
        //CombatPlayerManager.instance.GetPlayer(0).gameObject.transform.rotation = victoryPlayerPos.transform.rotation;
        if(LootManager.instance.AquiredLootItems.Count>=1)
        LootDisplayManager.instance.AddItems(LootManager.instance.currentLootItems);
        List<int> x = new List<int>();
        //here is where you would load the inventorys count of how many resources *demon cash* you have.
        List<int> y = new List<int>();
        if (PlayerInventory.instance)
        {
            y.Add(PlayerInventory.instance.GetHellCash());
            PlayerInventory.instance.SaveFamiliarStats();
            PlayerInventory.instance.SavePlayerStats();
        }
        else
            y.Add(0);

        x.Add(LootManager.instance.demonCurrentCash);
        LootDisplayManager.instance.AddResources(x, y, resourceSprites);
        LootDisplayManager.instance.StartVictoryScreen(hasLost_);
        CombatPlayerManager.instance.EnableFamiliars(false);
        EnemyManager.instance.DisableAllEnemies();
        PlayerManager.instance.TemporaryDisablePlayer2();
        if (hasLost_)
            return;
        if (PlayerInventory.instance)
        {
            PlayerInventory.instance.AddHellCash(LootManager.instance.demonCurrentCash);
            PlayerInventory.instance.SaveAllResources();
            foreach (LootItem item_ in LootManager.instance.currentLootItems)
            {
                PlayerInventory.instance.AddItemToInventory(item_.name, item_.amount);
            }
            PlayerInventory.instance.SaveItems();
        }
    }
    public void SwitchSinDrops()
    {
        foreach(ItemDropList list_ in sinItemDropLists)
        {
            if(currentSin.ToString()==list_.itemListName)
            {
                LootManager.instance.SetItemDropList(list_);
                DemonShopManager.instance.SwitchSinSrops(list_.itemListName);
            }
        }
       
    }
    public void PlayRandomBGM()
    {
        MMSoundManager.Instance.StopTrack(MMSoundManager.MMSoundManagerTracks.Music);
        MMSoundManager.Instance.PlaySound(BGMs[Random.Range(0, BGMs.Count)], MMSoundManager.MMSoundManagerTracks.Music, Vector3.zero,true);
    }
}
