using Blobcreate.ProjectileToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrower : MonoBehaviour
{
    public List<GameObject> objectsToThrow = new List<GameObject>();
    public List<Transform> objectTargets = new List<Transform>();
    public List<Transform> objectStartPositions = new List<Transform>();

    public void ThrowAllObjects()
    {
        for (int i = 0; i < objectsToThrow.Count; i++)
        {
            objectsToThrow[i].transform.position = objectStartPositions[i].position;
            objectsToThrow[i].SetActive(true);
            objectsToThrow[i].GetComponent<Rigidbody>().AddForce(Projectile.VelocityByA(objectsToThrow[i].transform.position, objectTargets[i].transform.position, -0.1f), ForceMode.VelocityChange);
        }
    }
}
