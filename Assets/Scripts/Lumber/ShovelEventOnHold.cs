using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelEventOnHold : InteractableEventOnHold
{
    public override void Interact(GameObject interactingObject_ = null)
    {

        if (interactingObject_.TryGetComponent<LumberPlayer>(out LumberPlayer p_))
        {
            currentHoldDuration += Time.deltaTime * p_.shovelPower*2;
        }
        else
            currentHoldDuration += Time.deltaTime*2;
    }
}
