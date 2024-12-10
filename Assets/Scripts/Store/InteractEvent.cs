using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Generic event that can be used when interacted with. Will happen every frame unless disabled after use.
/// </summary>
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
