using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWheelInteractable : InteractableObject
{
    public virtual void Interact(GameObject interactingObject_ = null)
    {
        Debug.Log("Interacting!");

        if (interactingObject_ == null)
            return;

        if (interactingObject_.TryGetComponent<FishingPlayer>(out FishingPlayer playa))
        {
            playa.canMove = false;
        }
    }
}
