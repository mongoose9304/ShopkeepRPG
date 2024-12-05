using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberPuzzle : MonoBehaviour
{
    public int currentComboCount;
    public List<GameObject> myTrees = new List<GameObject>();
    public GameObject resetParticleEffect;
    private float timeToResetTreeChopDirectoions;
    private void Update()
    {
        if(timeToResetTreeChopDirectoions<3)
        {
            timeToResetTreeChopDirectoions += Time.deltaTime;
        }
        else if(timeToResetTreeChopDirectoions >= 3)
        {
            resetParticleEffect.SetActive(false);
        }
        if(timeToResetTreeChopDirectoions <= 0)
        {
            ResetTreeChopDirections();
        }
    }
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
        SetTreeOwners();
        ResetCombo();
    }
    public void ResetTreeChopDirections()
    {
        foreach(GameObject obj in myTrees)
        {
            obj.GetComponent<Tree>().ResetFallIndicator();
        }
        timeToResetTreeChopDirectoions = 3;
    }
    public void HoldResetTreeChopDirections()
    {
        resetParticleEffect.SetActive(true);
        timeToResetTreeChopDirectoions -= Time.deltaTime*2;
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
    private void SetTreeOwners()
    {
        foreach(GameObject obj in myTrees)
        {
            obj.GetComponent<Tree>().myPuzzle = this;
        }
    }
    public void AddToCombo()
    {
        currentComboCount += 1;
        TreeManager.instance.SetCombo(currentComboCount);
    }
    public void ResetCombo()
    {
        currentComboCount = 0;
        TreeManager.instance.ResetCombo();
    }

}
