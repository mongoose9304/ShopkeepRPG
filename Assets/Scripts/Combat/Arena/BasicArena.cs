using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A version of a dungeon designed for wave based Combat of increasing difficulty 
/// </summary>
public class BasicArena : BasicDungeon
{
    public int currentWave;
    public bool onGoingWave;
    [SerializeField] private int spawnedEnemies;
    [Header("Variables for Difficulty")]
    [Tooltip("The amount and intensity of enemies")]
    public ArenaWaveList myWaveList;
    [Tooltip("Should the spawns happen in order or randomly")]
    public bool SpawnInOrder;
    private int maxEnemies;
    float currentSpawnDelay;
    [SerializeField] float spawnDelayMin;
    [SerializeField] float spawnDelayMax;
    [Header("References")]
    [Tooltip("REFERENCE to logic that counts how many enemies have died this wave to start the next")]
    [SerializeField] EnemyCounter myCounter;
    [Tooltip("REFERENCE to all the possible spawn points should be in this list")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [Tooltip("REFERENCE to all the possible spawn points for this team should be in this list")]
    //For team based combat its best to seperate the spawns into "sides" so the enemies group up with thier own team
    [SerializeField] private List<Transform> aTeamSpawns = new List<Transform>();
    [Tooltip("REFERENCE to all the possible spawn points should be in this list")]
    [SerializeField] private List<Transform> bTeamSpawns = new List<Transform>();
    [Tooltip("REFERENCE to all the possible spawn points should be in this list")]
    [SerializeField] private List<Transform> cTeamSpawns = new List<Transform>();
    [Tooltip("REFERENCE to all the possible spawn points should be in this list")]
    [SerializeField] private List<Transform> dTeamSpawns = new List<Transform>();
    [Tooltip("REFERENCE to all the doors that unlock when beating the level")]
    [SerializeField] private List<GameObject> lockObjects = new List<GameObject>();
    //to keep track of what the last guy we spawned was so we can ensure areound 50 50 for each team
    [SerializeField] private int lastTeamSpawned;
    
    
    private int currentSpawn=0;
    private float delayBeforeWave;
    private void OnEnable()
    {
        StartWave(0);
        LockArena(true);
    }
    /// <summary>
    /// Starst a wave and spawn some enemies
    /// </summary>
    public void StartWave(int wave_)
    {
        CombatPlayerManager.instance.ReturnFamiliars();
        myCounter.currentEnemies=maxEnemies= myWaveList.startingEnemies+myWaveList.extraEnemiesPerWave*wave_;
        enemyLevel = myWaveList.startingEnemyLevel + myWaveList.extraEnemyLevelPerWave * wave_;
        spawnedEnemies = 0;
        onGoingWave = true;
        delayBeforeWave = 5;
    }

    private void Update()
    {
        if (onGoingWave)
        {
            if(delayBeforeWave>0)
            {
                delayBeforeWave -= Time.deltaTime;
                return;
            }
            if (myCounter.currentEnemies <= 0)
            {
                onGoingWave = false;
                return;
            }
            currentSpawnDelay -= Time.deltaTime;
            if (currentSpawnDelay <= 0)
            {
                if (useRandomTeams)
                {
                    if (Random.Range(0.0f, 1.0f) < myWaveList.eliteChance)
                        SpawnBasicEnemyOnTeam(true);
                    else
                        SpawnBasicEnemyOnTeam();
                }
                else
                {
                    if(Random.Range(0.0f,1.0f)<myWaveList.eliteChance)
                        SpawnBasicEnemy(true);
                    else
                        SpawnBasicEnemy();
                }
                currentSpawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);
            }


        }
        else
        {
            currentWave += 1;
            if(currentWave>=myWaveList.maxWaves)
            {
                //Win
                LockArena(false);
            }
            else
            {
              StartWave(currentWave);
            }
        }
    }
    /// <summary>
    /// Spawn an enemy with no team
    /// </summary>
    private void SpawnBasicEnemy(bool isElite_=false)
    {
        if (spawnedEnemies >= maxEnemies)
            return;
        Debug.Log("Spawn");
        if(SpawnInOrder)
        {
            EnemyManager.instance.SpawnRandomEnemy(isElite_, spawnPoints[currentSpawn], myCounter, DungeonManager.instance.GetEnemyLevel());
            currentSpawn += 1;
            if (currentSpawn >= spawnPoints.Count)
                currentSpawn = 0;
        }
        else
        {
        EnemyManager.instance.SpawnRandomEnemy(isElite_, spawnPoints[Random.Range(0, spawnPoints.Count)], myCounter, enemyLevel);
        }
        spawnedEnemies += 1;
    }
    /// <summary>
    /// Spawn an enemy with a team
    /// </summary>
    private void SpawnBasicEnemyOnTeam(bool isElite_ = false)
    {
        if (spawnedEnemies >= maxEnemies)
            return;
        switch(lastTeamSpawned)
        {
            case 0:
                if (currentSpawn >= aTeamSpawns.Count)
                    currentSpawn = 0;
                EnemyManager.instance.SpawnRandomEnemy(isElite_, aTeamSpawns[currentSpawn], myCounter, DungeonManager.instance.GetEnemyLevel(),availableTeams[lastTeamSpawned]);
                break;
            case 1:
                if (currentSpawn >= bTeamSpawns.Count)
                    currentSpawn = 0;
                EnemyManager.instance.SpawnRandomEnemy(isElite_, bTeamSpawns[currentSpawn], myCounter, DungeonManager.instance.GetEnemyLevel(), availableTeams[lastTeamSpawned]);
                break;
            case 2:
                if (currentSpawn >= cTeamSpawns.Count)
                    currentSpawn = 0;
                EnemyManager.instance.SpawnRandomEnemy(isElite_, cTeamSpawns[currentSpawn], myCounter, DungeonManager.instance.GetEnemyLevel(), availableTeams[lastTeamSpawned]);
                break;
            case 3:
                if (currentSpawn >= dTeamSpawns.Count)
                    currentSpawn = 0;
                EnemyManager.instance.SpawnRandomEnemy(isElite_, dTeamSpawns[currentSpawn], myCounter, DungeonManager.instance.GetEnemyLevel(), availableTeams[lastTeamSpawned]);
                break;
        }
        lastTeamSpawned += 1;
        currentSpawn += 1;
        if (lastTeamSpawned >= availableTeams.Count)
            lastTeamSpawned = 0;
            
        
      
        spawnedEnemies += 1;
    }
    /// <summary>
    /// Lock/unlock the doors of an arena
    /// </summary>
    public void LockArena(bool lock_)
    {
        if(lock_)
        {
            foreach(GameObject obj in lockObjects)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject obj in lockObjects)
            {
                obj.SetActive(false);
            }
        }
    }
}
