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

    public List<FishType> spawnTypes = new List<FishType>();
    public List<float> weights = new List<float>();
    private float totalWeight;

    private List<GameObject> allFish;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        allFish = new List<GameObject>();
        totalWeight = 0.0f;
        for (int i = 0; i < weights.Count; ++i)
        {
            totalWeight += weights.ElementAt(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (allFish.Count < maxFishSpawned)
        {
            GameObject newFish = Instantiate(fishPrefab);
            Vector2 xz = Random.insideUnitCircle * maxSpawnRadius;
            newFish.transform.position = transform.position + new Vector3(xz.x, fishSpawnHeight - 3.0f, xz.y);
            FishInWaterBehaviour fwb = newFish.GetComponent<FishInWaterBehaviour>();
            fwb.targetY = fishSpawnHeight;
            fwb.type = GetRandomFish();
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

    FishType GetRandomFish()
    {
        // Generates a random fish with their given weights,
        // so for example if you had pike with a weight of 2 and bass with a weight of 8 you
        // would have a 20% chance of getting a pike, and 80% chance of getting a bass.
        float rand = Random.Range(0.0f, totalWeight);
        for (int i = 0; i < weights.Count; ++i)
        {
            if (rand < weights.ElementAt(i))
            {
                return spawnTypes.ElementAt(i);
            }
            rand -= weights.ElementAt(i);
        }

        Debug.LogWarning("Should not reach here.");
        return FishType.Bass;
    }
}
