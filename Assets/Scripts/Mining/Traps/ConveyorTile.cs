using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorTile : MonoBehaviour
{
    public float moveSpeed;
    public float dampModifier;
    Vector3 velocity = Vector3.zero;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Explosion")
            return;
        other.transform.position += transform.forward * moveSpeed * Time.deltaTime;
        //other.transform.position = Vector3.SmoothDamp(other.transform.position, other.transform.position + (transform.forward*moveSpeed * Time.fixedDeltaTime), ref velocity, dampModifier);
    }
}
