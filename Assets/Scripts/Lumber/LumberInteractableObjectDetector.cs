using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberInteractableObjectDetector : MonoBehaviour
{
    LumberPlayer lumberPlayer;
    private void Start()
    {
        lumberPlayer = GetComponentInParent<LumberPlayer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Interactable")
        {

            if (!lumberPlayer.myInteractableObjects.Contains(other.gameObject))
                lumberPlayer.myInteractableObjects.Add(other.gameObject);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interactable")
        {
            Debug.Log("lost Object");
            if (lumberPlayer.myInteractableObjects.Contains(other.gameObject))
                lumberPlayer.myInteractableObjects.Remove(other.gameObject);

        }
    }
}
