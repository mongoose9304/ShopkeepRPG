using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomizer : MonoBehaviour
{
    public List<GameObject> objectOptions;
    public bool useMultipleRandomObjects;
    public float randomChance;
    private void OnEnable()
    {
       
        foreach(GameObject obj in objectOptions)
        {
            obj.SetActive(false);
        }
        if (!useMultipleRandomObjects)
        {
            objectOptions[Random.Range(0, objectOptions.Count)].SetActive(true);
        }
        else
        {
            foreach (GameObject obj in objectOptions)
            {
                if(Random.Range(0.0f,1.0f)<randomChance)
                {
                    obj.SetActive(true);
                }
            }
        }
    }
}
