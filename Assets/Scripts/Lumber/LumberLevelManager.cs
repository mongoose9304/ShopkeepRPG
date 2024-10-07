using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class LumberLevelManager : MonoBehaviour
{
    public static LumberLevelManager instance;
    public LumberLevel currentLevel;
    public LumberLevel nextLevel;
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
        currentLevel.SpawnAllPuzzles();
    }
}
