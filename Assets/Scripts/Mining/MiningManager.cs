using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine.SceneManagement;
/// <summary>
/// The singleton class that will handle loading the mining levels and managing thier order
/// </summary>

public class MiningManager : MonoBehaviour
{
    [Tooltip("The singleton instance")]
    public static MiningManager instance;
    [Tooltip("What level we are on")]
    public MiningLevel currentLevel;
    [Tooltip("How many Sections We have completed")]
    public int sectionsCompleted;
    [Tooltip("How many sections are there")]
    public int maxSections;
    [Tooltip("What is the state of the mine")]
    public float mineHealth;
    [Tooltip("A list of all the levels that can be spawned (prefabs)")]
    public List<MiningLevel> levelsReferences = new List<MiningLevel>();
    [Tooltip("A list of all the special levels that can be spawned (prefabs)")]
    public List<MiningLevel> specialLevelsReferences = new List<MiningLevel>();
    [Tooltip("A list of all the special levels that can be spawned (prefabs)")]
    public List<MiningLevel> treasureLevelsReferences = new List<MiningLevel>();
    [Tooltip("How many levels to spawn between checkpoints")]
    public int levelsPerCheckpoint;
    [Tooltip("A reference list of all the levels we have spawned in")]
    public List<MiningLevel> levels = new List<MiningLevel>();
    [Tooltip("Transforms we can spawn levels at")]
    public List<Transform> levelsLocations = new List<Transform>();
    public Transform specialLevelSpawn;
    public Transform treasureLevelSpawn;
    [Tooltip("Sprites for the resources we collect")]
    public List<Sprite> resourceSprites = new List<Sprite>();
    [Tooltip("REFERNCE to the pool of stone world objects")]
    [SerializeField] MMMiniObjectPooler stoneWorldObjectPooler;
    [Tooltip("REFERNCE to the pool of dead treasure rocks")]
    [SerializeField] MMMiniObjectPooler deadTreasureRocksObjectPooler;
    [Tooltip("REFERNCE to the decorative end level")]
    [SerializeField] GameObject victoryLevel;
    [Tooltip("REFERNCE to the checkpoint level where players can return or continue mining")]
    public GameObject checkPointLevel;
    [Tooltip("REFERNCE to the checkpoint level spawn")]
    public GameObject checkPointPlayerPos;
    [Tooltip("REFERNCE to the checkpoint continue object")]
    public GameObject checkPointContinueTunnel;
    [Tooltip("REFERNCE to the location to put the player when the game is over for the victroy screen")]
    [SerializeField] GameObject victoryPlayerPos;
    [Tooltip("REFERNCE to the miningPlayer")]
    [SerializeField] MiningPlayer[] players;
    [Tooltip("REFERENCE to BGMs available")]
    public List<AudioClip> BGMs = new List<AudioClip>();
    public AudioSource backgroundNoise;
    public AudioSource backgroundNoiseCheckpoint;
    [Tooltip("REFERENCE to MinefloorSpawner available")]
    public ObjectRepeater mineFloorSpawner;
    [Tooltip("REFERENCE to MinefloorSpawner available")]
    public GameObject mineFloor;
    [Tooltip("REFERENCE to CosmeticZones available")]
    public List<MiningCosmeticZone> cosmeticZones = new List<MiningCosmeticZone>();
    public CameraHelper myCamera;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitLevels();
        PlayRandomBGM();
        PlayNextLevel();
        SpawnFloor();
    }
    /// <summary>
    /// Set up all the levels and spawn them in
    /// </summary>
    public void InitLevels()
    {
        foreach(MiningLevel obj in levels)
        {
            Destroy(obj.gameObject);
        }
        levels.Clear();
        int randomIndex = 0;
        List<int> usedObjects = new List<int>();
        for (int i = 0; i < levelsPerCheckpoint; i++)
        {
            randomIndex = Random.Range(0, levelsReferences.Count);
            while (usedObjects.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, levelsReferences.Count);
                if (usedObjects.Count >= levelsReferences.Count)
                {
                    usedObjects.Clear();
                }
            }
            usedObjects.Add(randomIndex);
            GameObject obj = GameObject.Instantiate(levelsReferences[randomIndex].gameObject, levelsLocations[levels.Count].transform.position, levelsLocations[levels.Count].transform.rotation);
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
    public void WinLevel(bool loss_ = false)
    {
        victoryLevel.SetActive(true);
        float extraX=0; //Nudge the 2nd player to the side to prevent overlaps
        foreach (MiningPlayer playa in players)
        {
            playa.enabled = false;
            playa.gameObject.SetActive(true);
            playa.gameObject.transform.position = victoryPlayerPos.transform.position+new Vector3(extraX,0,0);
            playa.gameObject.transform.rotation = victoryPlayerPos.transform.rotation;
            extraX -= 1.5f;
        }
           
        LootDisplayManager.instance.AddItems(LootManager.instance.currentLootItems);
        List<int> x = new List<int>();
        //here is where you would load the inventorys count of how many resources *stone* you have.
        List<int> y = new List<int>();
        if (PlayerInventory.instance)
        {
            y.Add(PlayerInventory.instance.GetStone());
        }
        else
            y.Add(0);
        x.Add(LootManager.instance.currentResource);
        LootDisplayManager.instance.AddResources(x,y,resourceSprites);
        LootDisplayManager.instance.StartVictoryScreen(loss_);
        PlayerManager.instance.TemporaryDisablePlayer2();
        if (loss_)
            return;
        if (PlayerInventory.instance)
        {
            PlayerInventory.instance.AddStone(LootManager.instance.currentResource);
            PlayerInventory.instance.SaveAllResources();
        }
    }
    public void PlayNextLevel()
    {
        UpdateMineHealth(PlayerPrefs.GetFloat("MineHealth", 1));
    }
    public void EnterCheckpointLevel()
    {
        foreach(MiningPlayer playa in players)
        {
            playa.Respawn();
        }
    }
    public void HealPlayers(float amount_)
    {
        foreach(MiningPlayer playa in players )
        {
            playa.Heal(amount_);
        }
    }
    public void CheckIfBothPlayersDead(GameObject obj)
    {
        foreach (MiningPlayer playa in players)
        {
            if (!playa.gameObject.activeInHierarchy)
                continue;
            if (!playa.isDead)
            {
                obj.SetActive(false);
                return;
            }
        }
        WinLevel(false);
    }
    public void PlayRandomBGM()
    {
        MMSoundManager.Instance.StopTrack(MMSoundManager.MMSoundManagerTracks.Music);
        MMSoundManager.Instance.PlaySound(BGMs[Random.Range(0, BGMs.Count)], MMSoundManager.MMSoundManagerTracks.Music, Vector3.zero, true, 0.75f);
    }
    public void UpdateMineHealth(float health_)
    {
        mineHealth = health_;
        Mathf.Clamp(mineHealth, 0.5f, 1.5f); 
        foreach(MiningCosmeticZone zone in cosmeticZones)
        {
            zone.SetUpCosmetics(mineHealth);
        }
        PlayerPrefs.SetFloat("MineHealth", mineHealth);
    }
    public void DebugSetMineHealth(float newHP)
    {
        PlayerPrefs.SetFloat("MineHealth", newHP);
        mineHealth = newHP;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public GameObject GetDeadRock()
    {
        return deadTreasureRocksObjectPooler.GetPooledGameObject();
    }
    private void SpawnFloor()
    {
        mineFloorSpawner.objectToRepeat = mineFloor;
        mineFloor.GetComponent<MiningCosmeticZone>().SetUpCosmetics(mineHealth);
        mineFloorSpawner.SpawnObjects();
    }
    public List<GameObject> GetPlayerObjects()
    {
        List<GameObject> players_ = new List<GameObject>();
        foreach (MiningPlayer playa in players)
            players_.Add(playa.gameObject);

        return players_;
    }
    public void ContinueGameFromCheckPoint()
    {
        sectionsCompleted += 1;
        PlayBackgroundNoise();
        if(sectionsCompleted>=maxSections)
        {
            foreach (MiningLevel lev in levels)
            {
                Destroy(lev.gameObject);
            }
            levels.Clear();
            myCamera.gameObject.SetActive(false);
            myCamera.Teleport();
            int randomIndex = Random.Range(0, specialLevelsReferences.Count);
            GameObject obj = GameObject.Instantiate(specialLevelsReferences[randomIndex].gameObject, specialLevelSpawn.transform.position, specialLevelSpawn.transform.rotation);
            levels.Add(obj.GetComponent<MiningLevel>());
             randomIndex = Random.Range(0, treasureLevelsReferences.Count);
             obj = GameObject.Instantiate(treasureLevelsReferences[randomIndex].gameObject, treasureLevelSpawn.transform.position, treasureLevelSpawn.transform.rotation);
            levels.Add(obj.GetComponent<MiningLevel>());
            for (int i = 0; i < levels.Count - 1; i++)
            {
                levels[i].nextLocation = levels[i + 1];
            }
            currentLevel = levels[0];
            foreach(MiningPlayer playa in players)
                playa.transform.position = currentLevel.startLocation.position;
            currentLevel.gameObject.SetActive(true);
            currentLevel.StartLevel();
            myCamera.gameObject.SetActive(true);
            checkPointContinueTunnel.SetActive(false);
            return;
        }
        InitLevels();
        StartCameraTeleport();
        foreach (MiningPlayer playa in players)
            playa.transform.position = currentLevel.startLocation.position;
        currentLevel.gameObject.SetActive(true);
        currentLevel.StartLevel();
        EndCameraTeleport();
    }
    public void StartCameraTeleport()
    {
        foreach(PlayerController playa in PlayerManager.instance.GetPlayers())
        {
            playa.myCam.gameObject.SetActive(false);
        }
        //myCamera.Teleport();
    }
    public void EndCameraTeleport()
    {
        foreach (PlayerController playa in PlayerManager.instance.GetPlayers())
        {
            playa.myCam.gameObject.SetActive(true);
        }
    }
    public void PlayBackgroundNoise()
    {
        backgroundNoise.Play();
        backgroundNoiseCheckpoint.Stop();
    }
    public void PlayBackgroundNoiseCheckpoint()
    {
        backgroundNoise.Stop();
        backgroundNoiseCheckpoint.Play();
    }
}
