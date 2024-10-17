using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnableObjectsOnDisable : MonoBehaviour
{
    public UnityEvent myEvent;
    private void OnDisable()
    {
        myEvent.Invoke();
    }

   
}
