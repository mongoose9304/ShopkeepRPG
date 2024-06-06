using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] Image previewImage;
    [SerializeField] string roomType;



    public void Teleport(GameObject objectToMove)
    {
        objectToMove.transform.position = destination.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            Teleport(other.gameObject);
        }
    }
    public void SetUpTeleporter(Sprite icon_,string roomType_)
    {
        previewImage.sprite = icon_;
        roomType = roomType_;
    }

}
