using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
/// <summary>
/// The singleton class that will handle loading the mining levels and managing thier order
/// </summary>

public class MiningManager : MonoBehaviour
{
    [Tooltip("The singleton instance")]
    public static MiningManager instance;
    [Tooltip("What level we are on")]
    public MiningLevel currentLevel;
    [Tooltip("A list of all the levels that will be spawned (prefabs)")]
    public List<MiningLevel> levelsReferences = new List<MiningLevel>();
    [Tooltip("A reference list of all the levels we have spawned in")]
    public List<MiningLevel> levels = new List<MiningLevel>();
    [Tooltip("Transforms we can spawn levels at")]
    public List<Transform> levelsLocations = new List<Transform>();
    [Tooltip("REFERNCE to the pool of stone world objects")]
    [SerializeField] MMMiniObjectPooler stoneWorldObjectPooler;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitLevels();
    }
    /// <summary>
    /// Set up all the levels and spawn them in
    /// </summary>
    public void InitLevels()
    {
        levels.Clear();
        for(int i=0;i< levelsReferences.Count;i++)
        {
           GameObject obj= GameObject.Instantiate(levelsReferences[i].gameObject,levelsLocations[i].transform.position, levelsLocations[i].transform.rotation);
            levels.Add(obj.GetComponent<MiningLevel>());
        }
        for (int i = 0; i < levels.Count-1; i++)
        {
            levels[i].nextLocation = levels[i + 1];
        }
        currentLevel = levels[0];
    }
    /// <summary>
    /// Using pools spawn a stone object at target location with a set value or the current levels default value
    /// </summary>
    public void SpawnStone(Transform location_,int value_=1,bool useMiningLevelStoneAmount=true)
    {
        GameObject obj = stoneWorldObjectPooler.GetPooledGameObject();
        if (useMiningLevelStoneAmount)
        {
            obj.GetComponent<StonePickUp>().stoneAmount = Random.Range(currentLevel.minStoneValue, currentLevel.maxStoneValue);
        }
        else
        {
            obj.GetComponent<StonePickUp>().stoneAmount = value_;
        }
        obj.transform.position = location_.position;
        obj.transform.rotation = location_.rotation;
        obj.SetActive(true);
    }
}
