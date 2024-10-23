using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomizer : MonoBehaviour
{
    public List<GameObject> objectOptions;
    private void OnEnable()
    {
        foreach(GameObject obj in objectOptions)
        {
            obj.SetActive(false);
        }
        objectOptions[Random.Range(0, objectOptions.Count)].SetActive(true);
    }
}
