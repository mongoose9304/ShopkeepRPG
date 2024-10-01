using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTreeDownInteraction : InteractableObject
{
    [SerializeField] Tree myTree;
    public override void Interact(GameObject interactingObject_ = null)
    {
        myTree.Fall();
    }
}
