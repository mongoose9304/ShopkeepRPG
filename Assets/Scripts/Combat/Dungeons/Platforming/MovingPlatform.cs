using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 lastPlayerPos;
    private float PlayerInRangeCountdownFailsafe;
    GameObject playerToMove;
    private void Update()
    {
        if(playerToMove)
        {
            PlayerInRangeCountdownFailsafe -= Time.deltaTime;
            if(PlayerInRangeCountdownFailsafe<=0)
            {
                playerToMove = null;
                return;
            }
            playerToMove.GetComponent<CombatPlayerMovement>().ExternalMoveForce((transform.position - lastPlayerPos));
            lastPlayerPos = transform.position;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Player")
        {
            PlayerInRangeCountdownFailsafe = 1;
           
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            lastPlayerPos = transform.position;
            playerToMove = other.gameObject;
            PlayerInRangeCountdownFailsafe = 1;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerToMove = null;
        }
    }
}
