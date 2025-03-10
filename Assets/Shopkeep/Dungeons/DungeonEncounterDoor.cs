using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DungeonEncounterDoor : MonoBehaviour
{
    private bool isOpen;
    private UnityEvent onOpen;
    private UnityEvent onClose;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        if (isOpen == false)
        {
            isOpen = true;
            onOpen.Invoke();
        }
    }

    public void Close()
    {
        if (isOpen == true)
        {
            isOpen = false;
            onClose.Invoke();
        }
    }
}
