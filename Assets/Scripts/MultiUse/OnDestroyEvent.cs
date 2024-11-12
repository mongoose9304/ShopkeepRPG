using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDestroyEvent : MonoBehaviour
{
    public UnityEvent destroyEvent;
    private bool isQuitting;
    private void OnDestroy()
    {
        if (isQuitting)
            return;
        destroyEvent.Invoke();
    }
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }
}
