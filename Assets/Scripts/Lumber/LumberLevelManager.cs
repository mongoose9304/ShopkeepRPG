using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class LumberLevelManager : MonoBehaviour
{
    public static LumberLevelManager instance;
    public LumberLevel currentLevel;
    public LumberLevel nextLevel;
    //ranges from 0.5 to 1.5, the rough amount of how much of each reasource your should be aquiring. ALso affects visuals 
    public float forestHealth;
    public float currentForestHealthChange;
    [Tooltip("REFERNCE to the pool of wood world objects")]
    [SerializeField] MMMiniObjectPooler woodWorldObjectPooler;
    [Tooltip("REFERNCE to the pool of multiwood world objects")]
    [SerializeField] MMMiniObjectPooler multiwoodWorldObjectPooler;
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Using pools spawn a lumber object at target location with a set value 
    /// </summary>
    public void SpawnLumber(Transform location_, int value_ = 1, bool useMultiWood = false)
    {
        GameObject obj = null;
        if (useMultiWood)
        {
             obj = multiwoodWorldObjectPooler.GetPooledGameObject();
        }
        else
        {
             obj = woodWorldObjectPooler.GetPooledGameObject();
        }
       
        obj.GetComponent<LumberPickUp>().lumberAmount = value_;
        
        obj.transform.position = location_.position;
        obj.transform.rotation = location_.rotation;
        obj.SetActive(true);
    }
    private void Start()
    {
        
        SetUpCurrentLevel();
        currentForestHealthChange = 0;
    }
    private void SetUpCurrentLevel()
    {
        currentLevel.SpawnLootables(forestHealth);
        currentLevel.SpawnAllPuzzles(forestHealth);
        currentLevel.SetUpDecorations(forestHealth);

    }
    public void TreeFall()
    {
        currentForestHealthChange += 0.01f;
    }
    public void TreeReplanted()
    {
        currentForestHealthChange -= 0.01f;
    }
    public void EndLevel()
    {
        if (currentForestHealthChange < 0)
            currentForestHealthChange = 0;
        if(currentForestHealthChange>10)
            currentForestHealthChange = 10;
        forestHealth -= currentForestHealthChange;
    }
}
