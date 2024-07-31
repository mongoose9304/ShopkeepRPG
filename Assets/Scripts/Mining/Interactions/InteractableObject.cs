using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An base class for all interactable objects to inherit from
/// </summary>
public class InteractableObject : MonoBehaviour
{



    /// <summary>
    /// The virtual function all interactbale objects will override to set thier specific functionality
    /// </summary>
    public virtual void Interact(GameObject interactingObject_=null)
    {
        Debug.Log("Interact");
    }
}
