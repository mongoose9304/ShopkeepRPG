using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
            interactableObject.SetActive(false);
    }
}
