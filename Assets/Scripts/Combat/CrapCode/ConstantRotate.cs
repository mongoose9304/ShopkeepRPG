using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotate : MonoBehaviour
{
    public Vector3 rotation;
    public float rotationSpeed;
    public float speedLossOverTime;
   

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += rotation * Time.deltaTime * rotationSpeed;
        rotationSpeed -= speedLossOverTime*Time.deltaTime;
        if (rotationSpeed <= 0)
            rotationSpeed = 0;
    }
    public void SetSpeed(float speed_)
    {
        rotationSpeed = speed_;
}

}
