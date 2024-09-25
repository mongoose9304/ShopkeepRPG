using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Activate effects by holding the specified button down
/// </summary>
public class HoldUseEvent : MonoBehaviour
{
    
    [Tooltip("The event that will play after being held")]
    public UnityEvent triggeredEvent;
    [Tooltip("How long the player must hold for the event to activate ")]
    public float maxHoldTime;
    float currentHoldTime;
    bool isActive;
    [Tooltip("REFERNCE to the UI bar that fills up as held")]
    public MMProgressBar myUIBar;
    [Tooltip("REFERNCE to the objects that are toggled on or off when in range ")]
    public List<GameObject> toggleObjects = new List<GameObject>();
    virtual protected void Update()
    {
        if (!isActive)
            return;
        if (currentHoldTime < maxHoldTime)
            currentHoldTime += Time.deltaTime;
       
        if (Input.GetButton("Fire3"))
        {
            currentHoldTime -= Time.deltaTime*2;
            
            if(currentHoldTime<=0)
            {
                triggeredEvent.Invoke();
            }
        }
        AdjustBar();

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            ToggleVisibility(true);
            currentHoldTime = maxHoldTime;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ToggleVisibility(false);
            currentHoldTime = maxHoldTime;
        }
    }
    public void ToggleVisibility(bool visible_)
    {
        isActive = visible_;
        foreach (GameObject obj in toggleObjects)
        {
            obj.SetActive(isActive);
        }
    }
    /// <summary>
    /// Adjusts the UI bar based on how long you hold down for
    /// </summary>
    private void AdjustBar()
    {
        myUIBar.UpdateBar01(currentHoldTime / maxHoldTime);
    }
}
