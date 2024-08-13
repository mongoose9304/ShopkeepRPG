using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : BasicRoom
{
    bool isLocked;

    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private int maxEnemies;
    [SerializeField] private int instantEnemies;
    [SerializeField] EnemyCounter myCounter;
    [SerializeField] private float spawnDelayMin;
    [SerializeField] private float spawnDelayMax;
    private float currentSpawnDelay;
    private int spawnedEnemies;

    public override void StartRoomActivity()
    {
        CombatPlayerManager.instance.ReturnFamiliars();
        LockRoom(true);
        myCounter.currentEnemies = maxEnemies;
        spawnedEnemies = 0;
        for(int i=0;i<instantEnemies;i++)
        {
          //  SpawnBasicEnemy();
        }
    }
    private void LockRoom(bool lock_)
    {
        foreach (GameObject obj in lockObjects)
        {
            obj.SetActive(lock_);
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
                SpawnBasicEnemy();
                currentSpawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);
            }

           
        }
    }

    private void SpawnBasicEnemy()
    {
        if (spawnedEnemies >= maxEnemies)
            return;

        EnemyManager.instance.SpawnEnemy(myDungeon.regularEnemies[Random.Range(0, myDungeon.regularEnemies.Count)].myBaseData.originalName, spawnPoints[Random.Range(0, spawnPoints.Count)],myCounter);
        spawnedEnemies += 1;
    }
}
