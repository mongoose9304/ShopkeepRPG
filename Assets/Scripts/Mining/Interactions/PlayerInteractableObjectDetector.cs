using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractableObjectDetector : MonoBehaviour
{
    MiningPlayer miningPlayer;
    private void Start()
    {
        miningPlayer = GetComponentInParent<MiningPlayer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Interactable")
        {

            if (!miningPlayer.myInteractableObjects.Contains(other.gameObject))
                miningPlayer.myInteractableObjects.Add(other.gameObject);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interactable")
        {
            Debug.Log("lost Object");
            if (miningPlayer.myInteractableObjects.Contains(other.gameObject))
                miningPlayer.myInteractableObjects.Remove(other.gameObject);

        }
    }
}
