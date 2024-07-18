using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunnel : InteractableObject
{
    [SerializeField] Transform teleportLocation;
    
    public void Teleport(GameObject obj_)
    {
        obj_.transform.position = teleportLocation.position;
    }
}
