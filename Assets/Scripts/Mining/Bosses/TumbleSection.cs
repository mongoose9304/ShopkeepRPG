using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleSection : MonoBehaviour
{
   public List<GameObject> myObjects = new List<GameObject>();

    public void CheckObjects()
    {
        foreach(GameObject obj in myObjects)
        {
            if (obj.activeInHierarchy)
                return;
        }
        gameObject.SetActive(false);
    }
    public void ChangeColliders(bool enableCollider)
    {
        foreach(GameObject obj in myObjects)
        {
            obj.GetComponent<Collider>().enabled = enableCollider;
        }
    }
   
}
