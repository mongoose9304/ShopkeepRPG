using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicArena : BasicDungeon
{
    public ArenaWaveList myWaveList;
    public int currentWave;
    public bool onGoingWave;
    [SerializeField] EnemyCounter myCounter;
    private int spawnedEnemies;
    private int maxEnemies;
    [SerializeField] float currentSpawnDelay;
    [SerializeField] float spawnDelayMin;
    [SerializeField] float spawnDelayMax;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    public bool SpawnInOrder;
    private int currentSpawn=0;
    private float delayBeforeWave;
    private void OnEnable()
    {
        StartWave(0);
    }
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
                SpawnBasicEnemy();
                currentSpawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);
            }


        }
        else
        {
            currentWave += 1;
            if(currentWave>=myWaveList.maxWaves)
            {
                //Win
            }
            else
            {
              StartWave(currentWave);
            }
        }
    }
    private void SpawnBasicEnemy()
    {
        if (spawnedEnemies >= maxEnemies)
            return;
        Debug.Log("Spawn");
        if(SpawnInOrder)
        {
            EnemyManager.instance.SpawnRandomEnemy(false, spawnPoints[currentSpawn], myCounter, DungeonManager.instance.GetEnemyLevel());
            currentSpawn += 1;
            if (currentSpawn >= spawnPoints.Count)
                currentSpawn = 0;
        }
        else
        {
        EnemyManager.instance.SpawnRandomEnemy(false, spawnPoints[Random.Range(0, spawnPoints.Count)], myCounter, enemyLevel);
        }
        spawnedEnemies += 1;
    }
}
