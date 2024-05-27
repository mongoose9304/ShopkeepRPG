using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform destination;



    public void Teleport(GameObject objectToMove)
    {
        objectToMove.transform.position = destination.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            Teleport(other.gameObject);
        }
    }

}
