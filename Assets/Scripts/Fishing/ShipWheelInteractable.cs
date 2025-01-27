using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWheelInteractable : InteractableObject
{
    public override void Interact(GameObject interactingObject = null,InteractLockOnButton btn=null)
    {
        if (interactingObject == null)
            return;

        if (interactingObject.TryGetComponent<FishingPlayer>(out FishingPlayer playa))
        {
            playa.canMove = false;
        }
    }
}
