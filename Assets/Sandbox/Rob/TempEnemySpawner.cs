using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class TempEnemySpawner : MonoBehaviour
{
    public float startSpawns;
    public float maxSpawnDelay;
    public float minSpawnDelay;
    private float currentSpawnDelay;
    public MMMiniObjectPooler pooler;
    GameObject obj;
    public Transform[] spawns;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<startSpawns;i++)
        {
          obj=  pooler.GetPooledGameObject();
          obj.transform.position = spawns[Random.Range(0, spawns.Length)].position;
            obj.SetActive(true);
            if(!GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayerMovement>().GetCurrentEnemyList().Contains(obj))
             GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayerMovement>().AddEnemy(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentSpawnDelay -= Time.deltaTime;
        if(currentSpawnDelay<=0)
        {
            currentSpawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            obj = pooler.GetPooledGameObject();
            obj.transform.position = spawns[Random.Range(0, spawns.Length)].position;
            obj.SetActive(true);
            if (!GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayerMovement>().GetCurrentEnemyList().Contains(obj))
                GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayerMovement>().AddEnemy(obj);
        }
    }
}
