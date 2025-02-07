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
    public bool isActive = true;

    public List<FishType> spawnTypes = new List<FishType>();
    public List<float> weights = new List<float>();
    private float totalWeight;

    private List<GameObject> allFish;

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
        if (isActive == false)
        {
            // Need to be able to enable or disable these for
            // rare fish management.
            return;
        }

        if (allFish.Count < maxFishSpawned)
        {
            GameObject newFish = Instantiate(fishPrefab);
            Vector2 xz = Random.insideUnitCircle * maxSpawnRadius;
            newFish.transform.position = transform.position + new Vector3(xz.x, fishSpawnHeight - 3.0f, xz.y);
            FishInWaterBehaviour fwb = newFish.GetComponent<FishInWaterBehaviour>();
            fwb.targetY = fishSpawnHeight;
            FishType species = GetRandomFish();
            fwb.fish = new Fish(species, GetSize(species));
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

    // Returns a reasonable size for a fish of the given species.
    float GetSize(FishType species)
    {

        // I know the breaks are unreachable but they should still be there. Supressing.
#pragma warning disable CS0162 // Unreachable code detected
        switch (species)
        {
            case FishType.Trout:
                return Random.Range(12.0f, 31.0f);
                break;
            case FishType.Pike:
                return Random.Range(16.0f, 24.0f);
                break;
            case FishType.Bass:
                return Random.Range(12.0f, 22.0f);
                break;
            case FishType.Carp:
                return Random.Range(7.5f, 15.5f);
                break;
            default:
                Debug.LogWarning("Getting size of an invalid species. Returning 1.0f.");
                return 1.0f;
                break;
        }
#pragma warning restore CS0162 // Unreachable code detected
    }
}
