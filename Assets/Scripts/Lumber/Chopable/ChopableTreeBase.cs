using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopableTreeBase : ChopableObject
{
    [SerializeField] Tree myTree;
    [SerializeField] int myDirection;
    private void Start()
    {
        myTree = GetComponentInParent<Tree>();
    }
    public override void ChopInteraction(int damage_)
    {
        myTree.HitTree(myDirection,damage_);
    }
}
