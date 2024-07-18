using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField]protected GameObject gamepadIcon;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gamepadIcon.SetActive(true);
            other.GetComponent<MiningPlayer>().SetInteractableObject(this);
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            gamepadIcon.SetActive(false);
            other.GetComponent<MiningPlayer>().SetInteractableObject(null);
        }
    }
    public virtual void Interact()
    {
        Debug.Log("Interact");
    }
}
