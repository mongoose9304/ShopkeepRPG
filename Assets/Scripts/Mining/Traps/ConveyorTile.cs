using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorTile : MonoBehaviour
{
    public float moveSpeed;
    public float dampModifier;
    Vector3 velocity = Vector3.zero;
    public List<GameObject> objectsInZone = new List<GameObject>();
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Explosion")
            return;
        if(!objectsInZone.Contains(other.gameObject))
        objectsInZone.Add(other.gameObject);
        
        //other.transform.position = Vector3.SmoothDamp(other.transform.position, other.transform.position + (transform.forward*moveSpeed * Time.fixedDeltaTime), ref velocity, dampModifier);
    }
 
    private void Update()
    {
        foreach(GameObject obj in objectsInZone)
        {
            obj.transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        objectsInZone.Clear();
    }

}
