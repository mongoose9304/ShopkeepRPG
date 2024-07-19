using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMineableObjectDetector : MonoBehaviour
{
    MiningPlayer miningPlayer;
    private void Start()
    {
        miningPlayer = GetComponentInParent<MiningPlayer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Mineable")
        {
            
            if(!miningPlayer.myMineableObjects.Contains(other.gameObject))
            miningPlayer.myMineableObjects.Add(other.gameObject);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        
        {
            Debug.Log("lost Object");
            if (miningPlayer.myMineableObjects.Contains(other.gameObject))
                miningPlayer.RemoveObjectFromMineableObjects(other.gameObject);

        }
    }
}
