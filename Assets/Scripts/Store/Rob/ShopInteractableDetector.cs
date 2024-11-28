using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The collider that detects if any interactable objects are in range
/// </summary>
public class ShopInteractableDetector : MonoBehaviour
{
    StorePlayer Player;
    private void Start()
    {
        Player = GetComponentInParent<StorePlayer>();
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
