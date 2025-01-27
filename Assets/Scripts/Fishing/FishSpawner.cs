using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class FishSpawner : MonoBehaviour
{
    public GameObject fishPrefab;
    public float fishSpawnHeight = 0.0f;
    public int maxFishSpawned = 5;
    public float maxSpawnRadius = 60.0f;


    private List<GameObject> allFish;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        allFish = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (allFish.Count < maxFishSpawned)
        {
            GameObject newFish = Instantiate(fishPrefab);
            Vector2 xz = Random.insideUnitCircle * maxSpawnRadius;
            newFish.transform.position = new Vector3(xz.x, fishSpawnHeight, xz.y);
            allFish.Add(newFish);
        }

        for (int i = 0; i < allFish.Count; i++)
        {
            if (allFish[i] == null)
            {
                allFish.RemoveAt(i);
            }
        }
    }
}
