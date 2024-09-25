using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A room that will spawn a set number of enemies over time, can be locked until they are defeated
/// </summary>
public class CombatRoom : BasicRoom
{   [Header("Variables to change")]
    bool isLocked;
    [Tooltip("The maximum amount of enemies to spawn")]
    [SerializeField] private int maxEnemies;
    [Tooltip("Should the enemies be elite")]
    [SerializeField] private bool useEliteEnemies;
    [SerializeField] private float spawnDelayMin;
    [SerializeField] private float spawnDelayMax;
    private float currentSpawnDelay;
    private int spawnedEnemies;
    [Header("References")]
    [Tooltip("REFERNCE to the object that counts how many enemies you have killed so we know when to unlock the room")]
    [SerializeField] EnemyCounter myCounter;
    [Tooltip("REFERNCE to the places we can put enemies")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    public override void StartRoomActivity()
    {
        CombatPlayerManager.instance.ReturnFamiliars();
        LockRoom(true);
        myCounter.currentEnemies = maxEnemies;
        spawnedEnemies = 0;
    }
    private void LockRoom(bool lock_)
    {
        if (willLockOnEnter)
        {
            foreach (GameObject obj in lockObjects)
            {
                obj.SetActive(lock_);
            }
        }
        isLocked = lock_;
    }
    private void Update()
    {
        if(isLocked)
        {
            if (myCounter.currentEnemies <= 0)
            {
                LockRoom(false);
                Debug.Log("UNLOCKEDROB");
                return;
            }
            currentSpawnDelay -= Time.deltaTime;
            if (currentSpawnDelay <= 0)
            {
                if (!useEliteEnemies)
                    SpawnBasicEnemy();
                else
                    SpawnEliteEnemy();
                currentSpawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);
            }

           
        }
    }

    private void SpawnBasicEnemy()
    {
        if (spawnedEnemies >= maxEnemies)
            return;
        Debug.Log("Spawn");
        EnemyManager.instance.SpawnRandomEnemy(false, spawnPoints[Random.Range(0, spawnPoints.Count)], myCounter, DungeonManager.instance.GetEnemyLevel());
        spawnedEnemies += 1;
    }
    private void SpawnEliteEnemy()
    {
        if (spawnedEnemies >= maxEnemies)
            return;

        EnemyManager.instance.SpawnRandomEnemy(true, spawnPoints[Random.Range(0, spawnPoints.Count)], myCounter, DungeonManager.instance.GetEnemyLevel()); spawnedEnemies += 1;
    }
}
