using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberPuzzle : MonoBehaviour
{
    public List<GameObject> myTrees = new List<GameObject>();
    private void AddDescendantsWithTag(Transform parent, string tag, List<GameObject> list)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.tag == tag)
            {
                list.Add(child.gameObject);
            }
            AddDescendantsWithTag(child, tag, list);
        }
    }
    void Start()
    {
        AddDescendantsWithTag(transform, "Tree", myTrees);
    }
    public void ResetTreeChopDirections()
    {
        foreach(GameObject obj in myTrees)
        {
            obj.GetComponent<Tree>().ResetFallIndicator();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<LumberPlayer>().EnterPuzzle(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<LumberPlayer>().ExitPuzzle();
        }
    }

}
