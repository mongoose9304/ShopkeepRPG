using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberLevel : MonoBehaviour
{
    public GameObject[] smallPuzzles;
    public GameObject[] mediumPuzzles;
    public GameObject[] largePuzzles;
    public Transform[] smallPuzzleLocations;
    public Transform[] mediumPuzzleLocations;
    public Transform[] largePuzzleLocations;             
    public LootTableItem[] tier1Items;
    public LootTableItem[] tier2Items;
    public LootTableItem[] tier3Items;
    public LootTableItem[] tier4Items;
    public LootTableItem[] tier5Items;

    public LootTableItem[] tier1DigItems;
    public LootTableItem[] tier2DigItems;
    public LootTableItem[] tier3DigItems;
    public LootTableItem[] tier4DigItems;
    public LootTableItem[] tier5DigItems;


    public void SpawnAllPuzzles()
    {
        SpawnPuzzles(smallPuzzleLocations, smallPuzzles);
        SpawnPuzzles(mediumPuzzleLocations, mediumPuzzles);
        SpawnPuzzles(largePuzzleLocations, largePuzzles);
    }
    public LootTableItem[] GetItemTier(int tier_)
    {
        switch (tier_)
        {
            case 1:
                return tier1Items;
                break;
            case 2:
                return tier2Items;
                break;
            case 3:
                return tier3Items;
                break;
            case 4:
                return tier4Items;
                break;
            case 5:
                return tier5Items;
                break;
            default:
                return tier1Items;
                break;

        }
    }
    public LootTableItem[] GetDigItemTier(int tier_)
    {
        switch (tier_)
        {
            case 1:
                return tier1DigItems;
                break;
            case 2:
                return tier2DigItems;
                break;
            case 3:
                return tier3DigItems;
                break;
            case 4:
                return tier4DigItems;
                break;
            case 5:
                return tier5DigItems;
                break;
            default:
                return tier1DigItems;
                break;

        }
    }
    private void SpawnPuzzles(Transform[] spawns_,GameObject[] spawnableObjects)
    {
        List<int> usedObjects = new List<int>();
        for(int i=0;i<spawns_.Length;i++)
        {
            int randomIndex = Random.Range(0, spawnableObjects.Length);
            while(usedObjects.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, spawnableObjects.Length);
                if(usedObjects.Count>=spawnableObjects.Length)
                {
                    usedObjects.Clear();
                }
            }
            usedObjects.Add(randomIndex);
            GameObject.Instantiate(spawnableObjects[randomIndex], spawns_[i].position, spawns_[i].rotation);
        }
    }
}
