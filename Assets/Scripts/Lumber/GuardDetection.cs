using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardDetection : MonoBehaviour
{
    public BasicGuard myGuard;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            myGuard.PlayerEnterVision(other.gameObject);
        }
    }
}
