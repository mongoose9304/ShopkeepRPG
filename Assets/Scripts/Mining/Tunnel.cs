using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunnel : InteractableObject
{
    public Transform teleportLocation;
    public GameObject objectToSetActive;
    
    public void Teleport(GameObject obj_)
    {
        obj_.transform.position = teleportLocation.position;
    }
    public override void Interact()
    {
        Teleport(GameObject.FindGameObjectWithTag("Player"));
        if (objectToSetActive)
            objectToSetActive.SetActive(true);
        Debug.Log("Interact");
    }
}
