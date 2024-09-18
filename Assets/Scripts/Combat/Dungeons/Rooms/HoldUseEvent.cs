using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class HoldUseEvent : MonoBehaviour
{
    public List<GameObject> toggleObjects = new List<GameObject>();
    public UnityEvent triggeredEvent;
    public float maxHoldTime;
    float currentHoldTime;
    bool isActive;
    [Tooltip("REFERNCE to the UI bar that fills up as held")]
    public MMProgressBar myUIBar;
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
