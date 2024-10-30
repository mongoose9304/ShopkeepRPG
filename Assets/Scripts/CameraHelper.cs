using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraHelper : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public CinemachineVirtualCamera vCam;
    private void Start()
    {
        offset = transform.position - target.position;
    }
    public void ResetPos()
    {
        transform.position = target.position + offset;
    }
    public void Teleport()
    {
        vCam.PreviousStateIsValid = false;
        ResetPos();
    }
}
