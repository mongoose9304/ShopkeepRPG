using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractLockOnButton : MonoBehaviour
{
    public Slider holdSlider;
    public InteractLockOnButton otherPlayerButton;
    bool isQuitting;
    private void Update()
    {
        if (holdSlider.value >0)
        {
            holdSlider.value -= Time.deltaTime;
        }
    }
    private void OnDisable()
    {
        if(!isQuitting)
        {
            holdSlider.value = 0;
        }

    }
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }
    public void IsInteracting(float maxHoldTime=0,float currentHoldTime=0)
    {
        if(maxHoldTime>0)
        {
            holdSlider.gameObject.SetActive(true);
        }
        else
        {
            holdSlider.gameObject.SetActive(false);
        }
        holdSlider.maxValue = maxHoldTime;
        holdSlider.value = currentHoldTime;
        if (otherPlayerButton)
        {
            if (Vector3.Distance(transform.position, otherPlayerButton.transform.position) < 1)
            {
                otherPlayerButton.holdSlider.maxValue = maxHoldTime;
                otherPlayerButton.holdSlider.value = currentHoldTime;
            }
        }
    }
}
