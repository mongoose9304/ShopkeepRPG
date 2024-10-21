using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRepeater : MonoBehaviour
{
    public GameObject objectToRepeat;
    public Transform[] objectSpawns;

    public void SpawnObjects()
    {
        foreach(Transform transform_ in objectSpawns)
        {
            GameObject.Instantiate(objectToRepeat, transform_.position, transform_.rotation);
        }
    }
}
