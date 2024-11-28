using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// The doors at the front of the store, the shopmanager opens the others when one door is opened to keep them in sync
/// </summary>
public class ShopDoor : MonoBehaviour
{
    public Vector3 endPos;
    public GameObject wallObject;
    public GameObject interactableObject;

    public void ResetDoor()
    {
        wallObject.SetActive(true);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        if (interactableObject)
            interactableObject.SetActive(true);
    }
    public void RotateDoor()
    {
        transform.DORotate(endPos, 2, RotateMode.Fast);
        wallObject.SetActive(false);
        if (interactableObject)
        {
            ShopManager.instance.RemoveInteractableObject(interactableObject);
            interactableObject.SetActive(false);
        }
    }
}
