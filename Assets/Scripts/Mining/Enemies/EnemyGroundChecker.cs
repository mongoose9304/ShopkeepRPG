using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundChecker : MonoBehaviour
{
    [SerializeField] BasicMiningEnemy myEnemy;


    private void OnTriggerExit(Collider other)
    {
        if(other.tag=="Ground")
        myEnemy.DetectNoGround();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ground")
            myEnemy.DetectGround();
    }
}
