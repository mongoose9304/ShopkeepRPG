using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSection : MonoBehaviour
{
    public Tree myTree;
    private void OnTriggerEnter(Collider other)
    {
        if (!myTree.isFalling)
            return;
        if(other.tag=="TreeBase"&&other.gameObject!=myTree.gameObject)
        {
           if(other.gameObject.TryGetComponent<Tree>(out Tree tree_))
            {
                tree_.Fall();
                myTree.BreakTree();
                myTree.HitOtherTreeAudio();
                TreeManager.instance.AddToCombo();
            }
        }
        if (other.tag == "TreeTrunk" && !myTree.myTreeSections.Contains(other.gameObject))
        {
            if (other.gameObject.TryGetComponent<TreeSection>(out TreeSection tree_))
            {
                if (tree_.myTree.isFalling)
                    return;
                tree_.myTree.Fall();
                myTree.BreakTree();
                myTree.HitOtherTreeAudio();
                TreeManager.instance.AddToCombo();
            }
        }
      
        if (other.tag == "Ground")
        {
            myTree.GetComponent<Tree>().BreakTree();
            TreeManager.instance.ResetCombo();
        }
        if (other.tag == "Breakable")
        {
            if (other.gameObject.TryGetComponent<Breakable>(out Breakable breakObject_))
            {
                breakObject_.Break(this.gameObject);
                myTree.GetComponent<Tree>().BreakTree();
                TreeManager.instance.ResetCombo();
            }
        }
    }
}
