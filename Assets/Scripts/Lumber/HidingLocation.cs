using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class HidingLocation : InteractableObject
{
    float timeBeingUsed;
    public MMF_Player[] interactFeedbacks;
    private void Update()
    {
        if(timeBeingUsed>0)
        timeBeingUsed -= Time.deltaTime;
    }
    public override void Interact(GameObject interactingObject_ = null, InteractLockOnButton btn = null)
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
            foreach(MMF_Player player_ in interactFeedbacks)
            {
                player_.PlayFeedbacks();
            }
            if (!lumberPlayer_.isHiding)
                lumberPlayer_.Hide(this.transform, true);
            else
                lumberPlayer_.Hide(this.transform, false);
        }
    }
}
