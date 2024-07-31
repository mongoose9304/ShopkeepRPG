using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The teleporter that connects mining levels together 
/// </summary>
public class Tunnel : InteractableObject
{
    [Tooltip("The location to send the player")]
    public Transform teleportLocation;
    [Tooltip("The object to activate on teleporting, usually the next level")]
    public GameObject objectToSetActive;
    [Tooltip("The object to deactivate on teleporting, usually the previous level")]
    public GameObject objectToSetInactive;
    [Tooltip("The time you must hold the interact button before the tunnel will teleport a player")]
    [SerializeField] float maxHoldDuration;
    float currentHoldDuration;
    [Tooltip("REFERNCE to the UI bar that fills up as held")]
    public MMProgressBar myUIBar;
    private void Update()
    {
        if(currentHoldDuration>=maxHoldDuration)
        {
            Use();
            currentHoldDuration = 0;
        }
        if(currentHoldDuration>0)
            currentHoldDuration -= Time.deltaTime;
        if (currentHoldDuration < 0)
            currentHoldDuration = 0;

        AdjustBar();
    }
    /// <summary>
    /// Teleport an object to the target location 
    /// </summary>
    public void Teleport(GameObject obj_)
    {
        obj_.transform.position = teleportLocation.position;
    }
    public override void Interact(GameObject interactingObject_ = null)
    {
        currentHoldDuration += Time.deltaTime*2;
    }
    /// <summary>
    /// Will teleport the player and set the objects active/inactive as necessary 
    /// </summary>
    public void Use()
    {
        Teleport(GameObject.FindGameObjectWithTag("Player"));
        if (objectToSetActive)
            objectToSetActive.SetActive(true);
        if (objectToSetInactive)
            objectToSetInactive.SetActive(false);
        Debug.Log("Interact");
    }
    /// <summary>
    /// Adjusts the UI bar based on how long you hold down for
    /// </summary>
    private void AdjustBar()
    {
        myUIBar.UpdateBar01(currentHoldDuration / maxHoldDuration);
    }
}
