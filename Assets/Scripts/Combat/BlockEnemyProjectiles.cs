using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEnemyProjectiles : MonoBehaviour
{
    public string blockTag;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag==blockTag)
        {
            other.gameObject.SetActive(false);
        }
    }
}
