using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Activate effects by holding the specified button down
/// </summary>
public class HoldUseEvent : InteractableObject
{
    
    [Tooltip("The event that will play after being held")]
    public UnityEvent triggeredEvent;
    [Tooltip("How long the player must hold for the event to activate ")]
    public float maxHoldTime;
    [SerializeField]float currentHoldTime;
    [Tooltip("REFERNCE to the UI bar that fills up as held")]
    public MMProgressBar myUIBar;
    [Tooltip("REFERNCE to the objects that are toggled on or off when in range ")]
    public List<GameObject> toggleObjects = new List<GameObject>();
    public AudioSource holdAudioSource;
    public bool RemoveFromPlayerList;
    virtual protected void Update()
    {
        if (currentHoldTime > 0)
        {
            currentHoldTime -= Time.deltaTime;
        }
        else
        {
            if(holdAudioSource)
            {
                holdAudioSource.Stop();
            }
        }
      
        AdjustBar();

    }
    /// <summary>
    /// The virtual function all interactbale objects will override to set thier specific functionality
    /// </summary>
    public override void Interact(GameObject interactingObject_ = null,InteractLockOnButton btn= null)
    {
        currentHoldTime += Time.deltaTime * 2;
        if (holdAudioSource)
        {
            if (!holdAudioSource.isPlaying)
            {
                holdAudioSource.Play();
            }
        }
        if (currentHoldTime >= maxHoldTime)
        {
            triggeredEvent.Invoke();
            if (RemoveFromPlayerList)
            {
                CombatPlayerManager.instance.RemoveInteractableObject(gameObject);
            }
        }
        if (btn)
        {
            btn.IsInteracting(maxHoldTime, currentHoldTime);
        }
    }
  
    /// <summary>
    /// Adjusts the UI bar based on how long you hold down for
    /// </summary>
    private void AdjustBar()
    {
        //myUIBar.UpdateBar01(currentHoldTime / maxHoldTime);
    }
}
