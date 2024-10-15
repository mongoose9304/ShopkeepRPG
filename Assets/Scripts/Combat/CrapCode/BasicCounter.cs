using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicCounter : MonoBehaviour
{
    public int maxCount;
    [SerializeField] int currentCount;
    public UnityEvent countEndEvent;
    public void AddToCount(int amount)
    {
        currentCount += amount;
        if(currentCount>=maxCount)
        {
            countEndEvent.Invoke();
        }
    }

}
