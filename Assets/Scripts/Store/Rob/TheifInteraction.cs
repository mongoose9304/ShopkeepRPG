using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheifInteraction : InteractableObject
{
    public Theif myTheif;

    public override void Interact(GameObject interactingObject_ = null)
    {
        myTheif.Caught();
        ShopManager.instance.RemoveInteractableObject(gameObject);
    }
}
