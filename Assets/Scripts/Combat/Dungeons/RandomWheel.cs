using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RandomWheel : MonoBehaviour
{
    bool isSpinning;
    bool isComingToAStop;
    public float spinDecayDelayMin; 
    public float spinDecayDelayMax; 
    public float spinDecay; 
    public float speed;
    public int winSlot;
    float spinDecayDelayCurrent;
    [SerializeField] ConstantRotate rotater;
    [SerializeField] GameObject stopPos;
    [SerializeField] GameObject currentShortestSlot;
    [SerializeField] List<GameObject> slots = new List<GameObject>();
    
    public UnityEvent endSpinEvent;

    private void Update()
    {
        if(isSpinning)
        {
            spinDecayDelayCurrent -= Time.deltaTime;
            if (!isComingToAStop)
            {
                if (spinDecayDelayCurrent <= 0)
                {
                    isComingToAStop = true;
                    rotater.speedLossOverTime = spinDecay;
                }
            }
            else if(rotater.rotationSpeed<=0)
            {
                SpinEnded();
            }
        }
    }
    public void StartSpin()
    {
        rotater.speedLossOverTime = 0;
        rotater.rotationSpeed = speed;
        spinDecayDelayCurrent = Random.Range(spinDecayDelayMin, spinDecayDelayMax);
        isSpinning = true;
    }
    public void SpinEnded()
    {
        isSpinning = false;
        isComingToAStop = false;
        currentShortestSlot = slots[0];
        foreach (GameObject obj in slots)
        {
            if (Vector3.Distance(stopPos.transform.position, obj.transform.position) < Vector3.Distance(stopPos.transform.position, currentShortestSlot.transform.position))
                currentShortestSlot = obj;
        }
        Debug.Log(slots.IndexOf(currentShortestSlot));
        winSlot = slots.IndexOf(currentShortestSlot);
        endSpinEvent.Invoke();

    }

}
