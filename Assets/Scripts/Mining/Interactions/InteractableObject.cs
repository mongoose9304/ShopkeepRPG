using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
   

   
    public virtual void Interact(GameObject interactingObject_=null)
    {
        Debug.Log("Interact");
    }
}
