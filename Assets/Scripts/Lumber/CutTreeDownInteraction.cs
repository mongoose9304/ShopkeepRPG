using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTreeDownInteraction : InteractableObject
{
    [SerializeField] Tree myTree;
    public override void Interact(GameObject interactingObject_ = null, InteractLockOnButton btn = null)
    {
        if (interactingObject_.TryGetComponent<LumberPlayer>(out LumberPlayer playa))
        {
            if(myTree.myPuzzle)
            myTree.myPuzzle.LastHitByPlayerTwo = playa.isPlayer2;
            myTree.Fall();
        }
    }
}
