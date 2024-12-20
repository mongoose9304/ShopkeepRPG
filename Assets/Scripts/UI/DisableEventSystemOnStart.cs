using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEventSystemOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (UnityEngine.EventSystems.EventSystem.current)
        UnityEngine.EventSystems.EventSystem.current.gameObject.SetActive(false);
    }
}
