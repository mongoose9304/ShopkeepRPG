using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHaggleInteract : InteractableObject
{
    public Customer customer;
    public override void Interact(GameObject interactingObject_ = null)
    {
        if (interactingObject_.TryGetComponent<StorePlayer>(out StorePlayer playa))
        {
        customer.BeginHaggle(playa.isPlayer2);
        }
    }
}
