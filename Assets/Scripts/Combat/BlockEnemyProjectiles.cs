using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEnemyProjectiles : MonoBehaviour
{
    public string blockTag;
    public int maxBlocks;
    private int currentBlocks;
    private void OnTriggerEnter(Collider other)
    {
       
        if(other.tag==blockTag)
        {
            other.gameObject.SetActive(false);
            currentBlocks += 1;
            if (currentBlocks > maxBlocks)
                gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        currentBlocks = 0;
    }
}
