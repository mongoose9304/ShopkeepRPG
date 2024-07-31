using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObstacleDetector : MonoBehaviour
{
    [SerializeField] BasicMiningEnemy myEnemy;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag!="Player")
        myEnemy.DetectObstacle();
    }
}