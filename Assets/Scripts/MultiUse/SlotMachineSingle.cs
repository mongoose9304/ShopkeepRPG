using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlotMachineSingle : MonoBehaviour
{
    [SerializeField] float SpinSpeedStart;
    [SerializeField] float SpinSpeedCurrent;
    [SerializeField] float SpinSpeedDecrease;
    [SerializeField] float maxYValue;
    [SerializeField] float minYValue;
    [SerializeField] Transform slotSpinner;
    public bool isSpinning;
    [SerializeField] float stoptarget;
    [SerializeField] bool isStopping;
    [SerializeField] UnityEvent winEvent;
    [SerializeField] UnityEvent loseEvent;
    [SerializeField] UnityEvent jackpotEvent;
    //0=loss, 1= win, 2=big win
    [SerializeField] int[] outComesInOrder;
    private void Update()
    {
        if(isSpinning)
        {
            slotSpinner.localPosition += new Vector3(0, SpinSpeedCurrent, 0)*Time.deltaTime;
            if(slotSpinner.localPosition.y>maxYValue)
            {
                slotSpinner.localPosition = new Vector3(0, minYValue, 0);
            }
            
            if(SpinSpeedCurrent<=0.5f)
            {
                if (!isStopping)
                {
                    stoptarget = Mathf.Round(slotSpinner.localPosition.y+1);
                    if (stoptarget >= maxYValue)
                    {
                        return;
                    }
                    isStopping = true;
                }
                if(slotSpinner.localPosition.y>=stoptarget)
                {
                    isSpinning = false;
                    EndSpinEvent();
                    slotSpinner.localPosition  = new Vector3(0, stoptarget, 0);
                }
            }
            else
            {
                SpinSpeedCurrent -= Time.deltaTime * SpinSpeedDecrease;
            }
        }
    }
    public void Spin()
    {
        isSpinning = true;
        isStopping = false;
        SpinSpeedCurrent = SpinSpeedStart * Random.Range(1.0f, 2.0f); ;
    }
    public void EndSpinEvent()
    {
        switch(outComesInOrder[Mathf.RoundToInt(slotSpinner.transform.localPosition.y)])
        {
            case 0:
                loseEvent.Invoke();
                break;
            case 1:
                winEvent.Invoke();
                break;
            case 2:
                jackpotEvent.Invoke();
                break;
        }
    }
}
