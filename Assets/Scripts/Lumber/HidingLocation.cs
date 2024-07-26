using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingLocation : InteractableObject
{
    float timeBeingUsed;
    private void Update()
    {
        if(timeBeingUsed>0)
        timeBeingUsed -= Time.deltaTime;
    }
    public override void Interact(GameObject interactingObject_ = null)
    {
    if( interactingObject_.TryGetComponent<LumberPlayer>(out LumberPlayer lumberPlayer_))
        {
            if (timeBeingUsed > 0)
            {
                timeBeingUsed += Time.deltaTime;
                return;
            }
            timeBeingUsed += Time.deltaTime*4;
            Debug.Log("TryHide");
            if (!lumberPlayer_.isHiding)
                lumberPlayer_.Hide(this.transform, true);
            else
                lumberPlayer_.Hide(this.transform, false);
        }
    }
}
