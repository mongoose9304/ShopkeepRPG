using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interact with a thief and catch them
/// </summary>
public class ThiefInteraction : InteractableObject
{
    public Thief myThief;

    public override void Interact(GameObject interactingObject_ = null)
    {
        myThief.Caught();
        ShopManager.instance.RemoveInteractableObject(gameObject);
    }
}
