using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
   [SerializeField] bool goingRight;
   [SerializeField] bool slowingdown;
   [SerializeField] Vector3 rotateDirection;
   [SerializeField] float rotateSpeed;
   [SerializeField] float rotateSpeedModifier;
   [SerializeField] float rotateSpeedModifierIncrease;
   [SerializeField] float rotateSpeedModifierMax;
   [SerializeField] float minRotation;
   [SerializeField] float maxRotation;

    private void Update()
    {
        Rotate();
    }
    private void Rotate()
    {
        if (!slowingdown)
        {
            rotateSpeedModifier += Time.deltaTime * rotateSpeedModifierIncrease;
            if (rotateSpeedModifier > rotateSpeedModifierMax)
                rotateSpeedModifier = rotateSpeedModifierMax;
        }
        else
        {
            rotateSpeedModifier -= Time.deltaTime * rotateSpeedModifierIncrease*2.0f;
        }
        Debug.Log("Rx:" + transform.rotation.z);
        if (goingRight)
        {
            transform.Rotate(rotateDirection * rotateSpeed*Time.deltaTime* rotateSpeedModifier, Space.Self);

            if (transform.rotation.z > maxRotation)
            {
                slowingdown = true;
                if(rotateSpeedModifier<=0)
                {
                    slowingdown = false;
                    goingRight = false;
                }
      
            }
        }

        else
        {
            transform.Rotate(rotateDirection * -rotateSpeed * Time.deltaTime* rotateSpeedModifier, Space.Self);
            if (transform.rotation.z < minRotation)
            {
                slowingdown = true;
                if (rotateSpeedModifier <= 0)
                {
                    slowingdown = false;
                    goingRight = true;
                }

            }
        }

    }
}
