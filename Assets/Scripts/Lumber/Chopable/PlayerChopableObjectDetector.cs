using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChopableObjectDetector : MonoBehaviour
{
    LumberPlayer lumberPlayer;
    private void Start()
    {
        lumberPlayer = GetComponentInParent <LumberPlayer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Lumber")
        {

            if (!lumberPlayer.myChopableObjects.Contains(other.gameObject))
                lumberPlayer.myChopableObjects.Add(other.gameObject);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Lumber")
        {
            Debug.Log("lost Object");
            if (lumberPlayer.myChopableObjects.Contains(other.gameObject))
                lumberPlayer.RemoveObjectFromChopableObjects(other.gameObject);

        }
    }
}
