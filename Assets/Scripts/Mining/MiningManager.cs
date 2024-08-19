using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
/// <summary>
/// The singleton class that will handle loading the mining levels and managing thier order
/// </summary>

public class MiningManager : MonoBehaviour
{
    [Tooltip("The singleton instance")]
    public static MiningManager instance;
    [Tooltip("What level we are on")]
    public MiningLevel currentLevel;
    [Tooltip("A list of all the levels that will be spawned (prefabs)")]
    public List<MiningLevel> levelsReferences = new List<MiningLevel>();
    [Tooltip("A reference list of all the levels we have spawned in")]
    public List<MiningLevel> levels = new List<MiningLevel>();
    [Tooltip("Transforms we can spawn levels at")]
    public List<Transform> levelsLocations = new List<Transform>();
    [Tooltip("Sprites for the resources we collect")]
    public List<Sprite> resourceSprites = new List<Sprite>();
    [Tooltip("REFERNCE to the pool of stone world objects")]
    [SerializeField] MMMiniObjectPooler stoneWorldObjectPooler;
    [Tooltip("REFERNCE to the decorative end level")]
    [SerializeField] GameObject victoryLevel;
    [Tooltip("REFERNCE to the checkpoint level where players can return or continue mining")]
    public GameObject checkPointLevel;
    [Tooltip("REFERNCE to the checkpoint level spawn")]
    public GameObject checkPointPlayerPos;
    [Tooltip("REFERNCE to the location to put the player when the game is over for the victroy screen")]
    [SerializeField] GameObject victoryPlayerPos;
    [Tooltip("REFERNCE to the miningPlayer")]
    [SerializeField] MiningPlayer player;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitLevels();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MiningPlayer>();
    }
    /// <summary>
    /// Set up all the levels and spawn them in
    /// </summary>
    public void InitLevels()
    {
        levels.Clear();
        for(int i=0;i< levelsReferences.Count;i++)
        {
           GameObject obj= GameObject.Instantiate(levelsReferences[i].gameObject,levelsLocations[i].transform.position, levelsLocations[i].transform.rotation);
            levels.Add(obj.GetComponent<MiningLevel>());
        }
        for (int i = 0; i < levels.Count-1; i++)
        {
            levels[i].nextLocation = levels[i + 1];
        }
        currentLevel = levels[0];
    }
    /// <summary>
    /// Using pools spawn a stone object at target location with a set value or the current levels default value
    /// </summary>
    public void SpawnStone(Transform location_,int value_=1,bool useMiningLevelStoneAmount=true)
    {
        GameObject obj = stoneWorldObjectPooler.GetPooledGameObject();
        if (useMiningLevelStoneAmount)
        {
            obj.GetComponent<StonePickUp>().stoneAmount = Random.Range(currentLevel.minStoneValue, currentLevel.maxStoneValue);
        }
        else
        {
            obj.GetComponent<StonePickUp>().stoneAmount = value_;
        }
        obj.transform.position = location_.position;
        obj.transform.rotation = location_.rotation;
        obj.SetActive(true);
    }
    public void WinLevel()
    {
        player.enabled = false;
        victoryLevel.SetActive(true);
        player.gameObject.transform.position = victoryPlayerPos.transform.position;
        player.gameObject.transform.rotation = victoryPlayerPos.transform.rotation;
        LootDisplayManager.instance.AddItems(LootManager.instance.AquiredLootItems);
        List<int> x = new List<int>();
        //here is where you would load the inventorys count of how many resources *stone* you have.
        List<int> y = new List<int>();
        y.Add(1000);
        x.Add(LootManager.instance.currentResource);
        LootDisplayManager.instance.AddResources(x,y,resourceSprites);
        LootDisplayManager.instance.StartVictoryScreen();

    }
}
