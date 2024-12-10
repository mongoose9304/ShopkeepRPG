using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownInteractableObjectDetector : MonoBehaviour
{
    TownPlayer Player;
    private void Start()
    {
        Player = GetComponentInParent<TownPlayer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Interactable")
        {

            if (!Player.myInteractableObjects.Contains(other.gameObject))
                Player.myInteractableObjects.Add(other.gameObject);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interactable")
        {
            if (Player.myInteractableObjects.Contains(other.gameObject))
                Player.myInteractableObjects.Remove(other.gameObject);

        }
    }
}
