using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableEventOnHold : InteractableObject
{
    [Tooltip("The time you must hold the interact button before the tunnel will teleport a player")]
    [SerializeField] float maxHoldDuration;
    protected float currentHoldDuration;
    [Tooltip("REFERNCE to the UI bar that fills up as held")]
    public MMProgressBar myUIBar;
    public UnityEvent useEvent;
    private void Update()
    {
        if (currentHoldDuration >= maxHoldDuration)
        {
            Use();
            currentHoldDuration = 0;
        }
        if (currentHoldDuration > 0)
            currentHoldDuration -= Time.deltaTime;
        if (currentHoldDuration < 0)
            currentHoldDuration = 0;

        AdjustBar();
    }
    public void Use()
    {
        useEvent.Invoke();
    }
    private void AdjustBar()
    {
        //myUIBar.UpdateBar01(currentHoldDuration / maxHoldDuration);
    }
    public override void Interact(GameObject interactingObject_ = null,InteractLockOnButton btn=null)
    {
        currentHoldDuration += Time.deltaTime * 2;
        if(btn)
        {
            btn.IsInteracting(maxHoldDuration, currentHoldDuration);
        }
    }
}
