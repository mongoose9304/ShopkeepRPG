using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDisableEvent : MonoBehaviour
{
    public UnityEvent endEvent;
    bool quitting;
    private void OnDisable()
    {
        if (quitting)
            return;
        endEvent.Invoke();
    }
    void OnApplicationQuit()
    {
        quitting = true;
    }
}