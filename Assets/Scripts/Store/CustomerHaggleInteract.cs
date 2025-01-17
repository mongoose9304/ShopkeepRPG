using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Interact with acustomer and start haggling
/// </summary>
public class CustomerHaggleInteract : InteractableObject
{
    public Customer customer;
    public override void Interact(GameObject interactingObject_ = null, InteractLockOnButton btn = null)
    {
        if (interactingObject_.TryGetComponent<StorePlayer>(out StorePlayer playa))
        {
        customer.BeginHaggle(playa.isPlayer2);
        }
    }
}
