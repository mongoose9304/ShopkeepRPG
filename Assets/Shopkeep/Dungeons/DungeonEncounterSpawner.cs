using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonEncounterSpawner : MonoBehaviour
{
    [SerializeField]
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
        Debug.Log("Spawning");
        GameObject obj = Instantiate(enemyPrefab);
        obj.transform.position = transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        if (obj.GetComponent<DungeonDestructible>() != null)
        {

        }
    }
}
