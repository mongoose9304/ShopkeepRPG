using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 lastPos;
    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Player")
        {
            //other.GetComponent<CombatPlayerMovement>()
        }
    }
}
