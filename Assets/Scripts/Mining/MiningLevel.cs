using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The logic for a single minig level, controls advancement to the next level through a tunnel
/// </summary>
public class MiningLevel : MonoBehaviour
{
    public bool isTutLevel;
    [Tooltip("Min amount of stone that can be dropped")]
    public int minStoneValue=1;
    [Tooltip("Max amount of stone that can be dropped")]
    public int maxStoneValue = 1;
    [Tooltip("Min amount of rocks")]
    public int minRockAmount = 1;
    [Tooltip("Max amount of rocks")]
    public int maxRockAmount= 1;
    [Tooltip("Min amount of treasure")]
    public int minTreasureAmount = 1;
    [Tooltip("Max amount of treasure")]
    public int maxTreasureAmount = 1;
    [Tooltip("Min amount of enemies")]
    public int minEnemiesAmount = 1;
    [Tooltip("Max amount of enemies")]
    public int maxEnemiesAmount = 1;
    [Tooltip("The location the player will start")]
    public Transform startLocation;
    [Tooltip("The next level a player will go to")]
    public MiningLevel nextLocation;
    [Tooltip("The rock that will hold the tunnel to the next level, it will be randomly selected on play from all the possible rocks in a level ")]
    public Rock tunnelHolder;
    [Tooltip("REFERENCE to the tunnel gameobject ")]
    public GameObject nextLevelTunnel;
    [Tooltip("REFERENCE all the rocks that are children of this object, auto collected on play")]
    public List<Rock> allRocks = new List<Rock>();
    [Tooltip("REFERENCE all the rocks that are children of this object, auto collected on play")]
    public List<TreasureRock> allTreasureRocks = new List<TreasureRock>();
    [Tooltip("REFERENCE all the tiles that are children of this object, auto collected on play")]
    public List<Tile> allTiles = new List<Tile>();
    [Tooltip("REFERENCE all the walls that are children of this object, auto collected on play")]
    public List<Wall> allWalls = new List<Wall>();
    [Tooltip("REFERENCE all the enemies that are children of this object, auto collected on play")]
    public List<BasicMiningEnemy> allEnemies = new List<BasicMiningEnemy>();
    [Tooltip("REFERENCE to what the walls should look like for this level")]
    public Material wallMat;
    [Tooltip("REFERENCE to what half the tiles should look like for this level")]
    public Material tileMatA;
    [Tooltip("REFERENCE to what half the tiles should look like for this level")]
    public Material tileMatB;
    private void OnEnable()
    {
        if (isTutLevel)
            SetUpMiningLevel(1);
    }
    public virtual void SetUpMiningLevel(float health_)
    {
        GetAllRocks();
        GetAllTreasureRocks();
        GetAllTiles();
        GetAllWalls();
        GetAllEnemies();
        SetMaterials();
        RandomizeAllObjects(health_);
        SetDeadTreasureRocks(health_);
        DecideTunnelHolder();
        //Invoke("DecideTunnelHolder", 1.1f);
    }
    /// <summary>
    /// Moves the tunnel to whatever rock was holding it when it is revealed 
    /// </summary>
    /// <param name="location_">where to spawn the object</param>
    public void CreateTunnel(Transform location_)
    {
        nextLevelTunnel.transform.position = location_.position - new Vector3(0, 0.5f, 0);
        nextLevelTunnel.SetActive(true);
        if (nextLocation)
        {
            nextLevelTunnel.GetComponent<Tunnel>().teleportLocation = nextLocation.startLocation;
            nextLevelTunnel.GetComponent<Tunnel>().objectToSetActive = nextLocation.gameObject;
        }
        else
        {
            nextLevelTunnel.GetComponent<Tunnel>().teleportLocation = MiningManager.instance.checkPointPlayerPos.transform;
            nextLevelTunnel.GetComponent<Tunnel>().objectToSetActive = MiningManager.instance.checkPointLevel;
        }
        if(!nextLevelTunnel.GetComponent<Tunnel>().objectToSetInactive)
            nextLevelTunnel.GetComponent<Tunnel>().objectToSetInactive = gameObject;
        tunnelHolder = null;
    }
    /// <summary>
    /// Randomly picks a rock to hold the tunnel to the next level
    /// </summary>
    public void DecideTunnelHolder()
    {
        if (allRocks.Count == 0)
            return;
        foreach(Rock rock in allRocks)
        {
            rock.SetTunnelHolder(false);
        }
      RemoveInactiveRocks();
      tunnelHolder= allRocks[Random.Range(0, allRocks.Count)];
      tunnelHolder.SetTunnelHolder(true,this);
    }
    /// <summary>
    /// Gets all the rock children at the start of the game
    /// </summary>
    protected void GetAllTreasureRocks()
    {
        allTreasureRocks.Clear();
        allTreasureRocks.AddRange(gameObject.GetComponentsInChildren<TreasureRock>());
    }
    /// <summary>
    /// Gets all the rock children at the start of the game
    /// </summary>
    protected void GetAllRocks()
    {
        allRocks.Clear();
        allRocks.AddRange(gameObject.GetComponentsInChildren<Rock>());
    }
    /// <summary>
    /// Gets all the tile children at the start of the game
    /// </summary>
    protected void GetAllTiles()
    {
        allTiles.Clear();
        allTiles.AddRange(gameObject.GetComponentsInChildren<Tile>());
    }
    /// <summary>
    /// Gets all the wall children at the start of the game
    /// </summary>
    protected void GetAllWalls()
    {
        allWalls.Clear();
        allWalls.AddRange(gameObject.GetComponentsInChildren<Wall>());
    }
    /// <summary>
    /// Gets all the rock children at the start of the game
    /// </summary>
    protected void GetAllEnemies()
    {
        allEnemies.Clear();
        allEnemies.AddRange(gameObject.GetComponentsInChildren<BasicMiningEnemy>());
    }
    /// <summary>
    /// Removes any inactive rocks
    /// </summary>
    protected void RemoveInactiveRocks()
    {
        for(int i=0;i<allRocks.Count;i++)
        {
            if (!allRocks[i].gameObject.activeSelf)
                allRocks[i] = null;
        }
        allRocks.RemoveAll(item => item == null);
    }
    /// <summary>
    /// Sets the wall and tile materials to the ones defined by the level
    /// </summary>
    public void SetMaterials()
    {
        for (int i = 0; i < allWalls.Count; i++)
        {
            allWalls[i].gameObject.GetComponent<MeshRenderer>().material = wallMat;
        }
        for (int i = 0; i < allTiles.Count; i++)
        {
            if(allTiles[i].typeBFloor)
                allTiles[i].gameObject.GetComponent<MeshRenderer>().material = tileMatB;
            else
                allTiles[i].gameObject.GetComponent<MeshRenderer>().material = tileMatA;
        }
    }
    /// <summary>
    /// Any functionality to be called when the level begins
    /// </summary>
    public void StartLevel()
    {
        MiningManager.instance.currentLevel = this;
        SetUpMiningLevel(MiningManager.instance.mineHealth);
    }
    protected void RandomizeAllObjects(float health_)
    {
        List<GameObject> tempObjs = new List<GameObject>();
        foreach(Rock rock_ in allRocks)
        {
            tempObjs.Add(rock_.gameObject);
        }
        RandomizeObjects(tempObjs, Mathf.RoundToInt(Random.Range(minRockAmount,maxRockAmount)*health_));
        tempObjs.Clear();
        foreach (TreasureRock rock_ in allTreasureRocks)
        {
            tempObjs.Add(rock_.gameObject);
        }
        RandomizeObjects(tempObjs, Mathf.RoundToInt(Random.Range(minTreasureAmount, maxTreasureAmount) * health_));
        tempObjs.Clear();
        if (MiningManager.instance.sectionsCompleted==0)
        {
            foreach (BasicMiningEnemy enemy_ in allEnemies)
            {
                enemy_.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (BasicMiningEnemy enemy_ in allEnemies)
            {
                tempObjs.Add(enemy_.gameObject);
            }
            RandomizeObjects(tempObjs, Mathf.RoundToInt(Random.Range(minEnemiesAmount, maxEnemiesAmount)));
        }
       
    }
    protected void RandomizeObjects(List<GameObject> objList,int amountToSetActive_)
    {
        int randomIndex = 0;
        List<int> usedObjects=new List<int>();
        foreach (GameObject obj in objList)
        {
            obj.SetActive(false);
        }
        for (int i = 0; i < amountToSetActive_; i++)
        {
            randomIndex = Random.Range(0, objList.Count);
            while (usedObjects.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, objList.Count);
                if (usedObjects.Count >= objList.Count)
                {
                    usedObjects.Clear();
                }
            }
            usedObjects.Add(randomIndex);
            objList[randomIndex].SetActive(true);
        }
    }
    protected void SetDeadTreasureRocks(float health_)
    {
        GetAllTreasureRocks();
        int amountToSetActive_ = 0;
        if (health_<=0.6f)
        {
            amountToSetActive_ = 2;
        }
        else if(health_ <= 0.8f)
        {
            amountToSetActive_ = 1;
        }
        int randomIndex = 0;
        List<int> usedObjects = new List<int>();
        for (int i = 0; i < amountToSetActive_; i++)
        {
            randomIndex = Random.Range(0, allTreasureRocks.Count);
            while (usedObjects.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, allTreasureRocks.Count);
                if (usedObjects.Count >= allTreasureRocks.Count)
                {
                    usedObjects.Clear();
                }
            }
            usedObjects.Add(randomIndex);
            GameObject obj = MiningManager.instance.GetDeadRock();
            obj.transform.position = allTreasureRocks[randomIndex].gameObject.transform.position;
            obj.SetActive(true);
            allTreasureRocks[randomIndex].gameObject.SetActive(false);
        }
    }
}
