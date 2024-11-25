using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayerInteractableObjectDetector : MonoBehaviour
{
    CombatPlayerMovement combatPlayer;
    private void Start()
    {
        combatPlayer = GetComponentInParent<CombatPlayerMovement>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Interactable")
        {

            if (!combatPlayer.myInteractableObjects.Contains(other.gameObject))
                combatPlayer.myInteractableObjects.Add(other.gameObject);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interactable")
        {
            Debug.Log("lost Object");
            if (combatPlayer.myInteractableObjects.Contains(other.gameObject))
                combatPlayer.myInteractableObjects.Remove(other.gameObject);

        }
    }
}
