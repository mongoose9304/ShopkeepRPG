using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// A random wheel that will spin and provide an event that can be customized. The closest slot will be labled under winSlot, switch your outcomes based on the winSlot after spinning
/// </summary>
public class RandomWheel : MonoBehaviour
{
    [Header("Variables that can be changes")]
    bool isSpinning;
    bool isComingToAStop;
    [Tooltip("Min amount fo time to spin")]
    public float spinDecayDelayMin;
    [Tooltip("Max amount of time to spin")]
    public float spinDecayDelayMax;
    [Tooltip("Speed loss per sec")]
    public float spinDecay;
    [Tooltip("Starting speed")]
    public float speed;
    [Header("The win slot")]
    [Tooltip("The slot that has won the spin, use this for determining outcomes")]
    public int winSlot;
    float spinDecayDelayCurrent;
    [Header("References")]
    [Tooltip("REFERENCE to The indicator where the wheel stops")]
    [SerializeField] GameObject stopPos;
    [Tooltip("The currently closest slot")]
    [SerializeField] GameObject currentShortestSlot;
    [Tooltip("REFERENCE to the rotation object")]
    [SerializeField] ConstantRotate rotater;
    [Tooltip("REFERENCE to all the possible slots")]
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
