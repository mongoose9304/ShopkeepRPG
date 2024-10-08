using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberLevel : MonoBehaviour
{
    public GameObject[] smallPuzzles;
    public GameObject[] smallDeadTreePatches;
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
    public GameObject[] lootDirtPiles;
    public GameObject[] lootBushes;
    public GameObject[] lootHoldBushes;
    public int averagelootDirtPileAmount;
    public int averagelootBushAmount;
    public int averagelootHoldBushAmount;
    //Decorations
    public GameObject maxDecorations;
    public GameObject highDecorations;
    public GameObject lowDecorations;
    public GameObject minDecorations;


    public void SpawnAllPuzzles(float forestHP)
    {
        if(forestHP>0.8f)
        {
            SpawnPuzzles(smallPuzzleLocations, smallPuzzles);
        }
        else if (forestHP > 0.6f)
        {
            SpawnPuzzles(smallPuzzleLocations, smallPuzzles,1, smallDeadTreePatches);
        }
        else
        {
            SpawnPuzzles(smallPuzzleLocations, smallPuzzles, 2,smallDeadTreePatches);
        }
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
    private void SpawnPuzzles(Transform[] spawns_,GameObject[] spawnableObjects,int deadSpawns=0, GameObject[] spawnableDeadObjects=null)
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
            if(deadSpawns>0)
            {
                deadSpawns -= 1;
                GameObject.Instantiate(spawnableDeadObjects[Random.Range(0,spawnableDeadObjects.Length)], spawns_[i].position, spawns_[i].rotation);
            }
            else
            GameObject.Instantiate(spawnableObjects[randomIndex], spawns_[i].position, spawns_[i].rotation);
        }
    }
    public void SpawnLootables(float forestHP)
    {
        List<int> usedObjects = new List<int>();
        int randomIndex = 0;
        int dirtSpawnCount = Mathf.RoundToInt(averagelootDirtPileAmount * forestHP);
        int lootBushCount = Mathf.RoundToInt(averagelootBushAmount * forestHP);
        int holdLootBushCount = Mathf.RoundToInt(averagelootHoldBushAmount * forestHP);
        foreach(GameObject obj in lootDirtPiles)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in lootBushes)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in lootHoldBushes)
        {
            obj.SetActive(false);
        }
        for (int i = 0; i < dirtSpawnCount; i++)
        {
            randomIndex = Random.Range(0, lootDirtPiles.Length);
            while (usedObjects.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, lootDirtPiles.Length);
                if (usedObjects.Count >= lootDirtPiles.Length)
                {
                    usedObjects.Clear();
                }
            }
            usedObjects.Add(randomIndex);
            lootDirtPiles[randomIndex].SetActive(true);
        }
        usedObjects.Clear();
        for (int i = 0; i < lootBushCount; i++)
        {
            randomIndex = Random.Range(0, lootBushes.Length);
            while (usedObjects.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, lootBushes.Length);
                if (usedObjects.Count >= lootBushes.Length)
                {
                    usedObjects.Clear();
                }
            }
            usedObjects.Add(randomIndex);
            lootBushes[randomIndex].SetActive(true);
        }
        usedObjects.Clear();
        for (int i = 0; i < holdLootBushCount; i++)
        {
            randomIndex = Random.Range(0, lootHoldBushes.Length);
            while (usedObjects.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, lootHoldBushes.Length);
                if (usedObjects.Count >= lootHoldBushes.Length)
                {
                    usedObjects.Clear();
                }
            }
            usedObjects.Add(randomIndex);
            lootHoldBushes[randomIndex].SetActive(true);

        }
    }
    public void SetUpDecorations(float forestHP)
    {
        minDecorations.SetActive(false);
        maxDecorations.SetActive(false);
        lowDecorations.SetActive(false);
        maxDecorations.SetActive(false);
        if(forestHP<0.6f)
        {
            //Min
            minDecorations.SetActive(true);
        }
        else if (forestHP < 0.8f)
        {
            //Low
            lowDecorations.SetActive(true);
        }
        else if (forestHP < 1.2f)
        {
            //Medium
            lowDecorations.SetActive(true);
            highDecorations.SetActive(true);
        }
        else if (forestHP < 1.4)
        {
            //High
            lowDecorations.SetActive(true);
            highDecorations.SetActive(true);
        }
        else
        {
            lowDecorations.SetActive(true);
            highDecorations.SetActive(true);
            maxDecorations.SetActive(true);
            //Max
        }
    }
}
