using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Activate an event when a gamobject with a particular tag enters
/// </summary>
public class TagDetector : MonoBehaviour
{
    [SerializeField] string tagToDetect;
    [SerializeField] bool oneTimeUse;
    [SerializeField] UnityEvent onEnterEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag==tagToDetect)
        {
            onEnterEvent.Invoke();
            if (oneTimeUse)
                gameObject.SetActive(false);
        }
    }
}
