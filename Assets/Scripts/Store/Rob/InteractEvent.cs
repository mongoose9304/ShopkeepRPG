using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractEvent : InteractableObject
{
    public UnityEvent myEvent;
    /// <summary>
    /// The virtual function all interactbale objects will override to set thier specific functionality
    /// </summary>
    public override void Interact(GameObject interactingObject_ = null)
    {
        myEvent.Invoke();
    }
}
