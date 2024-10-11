using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMatcher : MonoBehaviour
{
    //crap code to prevent sprites from flipping when using look at functions;
    public Transform target;
    public float targetRot;
    public Quaternion newRot;
    public Quaternion oldRot;
    private void Update()
    {
        if(target.transform.rotation.eulerAngles.y <= targetRot)
        {
            transform.localRotation = newRot;
        }
        else
        {
            transform.localRotation = oldRot;
        }
    }
}
