using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotate : MonoBehaviour
{
    public Vector3 rotation;
    public float rotationSpeed;
   

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += rotation * Time.deltaTime * rotationSpeed;
    }
}
