using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEncounterSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        GameObject obj = Instantiate(enemyPrefab);
        if (obj.GetComponent<DungeonDestructible>() != null)
        {

        }
        
    }
}
