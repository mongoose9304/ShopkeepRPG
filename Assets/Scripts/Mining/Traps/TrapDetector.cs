using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDetector : MonoBehaviour
{
    public BasicMiningTrap myTrap;
    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Player")
        {
            myTrap.PlayerInRange();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            myTrap.PlayerLeavesRange();
        }
    }
}
