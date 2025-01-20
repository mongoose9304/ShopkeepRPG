using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapChest_behaviour : MonoBehaviour
{
    //Ray casting
    Vector3 RayOrigin;
    Vector3 RayDirection;

    private float timer =0.0f;
    private float rotationCD = 4.0f;
    private bool startCooldown = false;
    private float changeInAngle = 25.0f;
    private float currentAngle = 0.0f;
    private float targetAngle = 0.0f;
    private float rotationSpeed = 35.0f;
    private bool pauseRotation = false;

    float rotationTick = 0.0f;

    float[] angles = { 0.0f, 45.0f, 90.0f, 135.0f, 180.0f };
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        RayOrigin = transform.position;
        RayDirection = new Vector3(10.0f, 0.0f, 0.0f);

       targetAngle = angles[0];
    }

 

    // Update is called once per frame
    void Update()
    {
        //i want to make the pause, rotate, pause, rotate movement for this mob.

        if (pauseRotation)
        {   
           
            timer += Time.deltaTime;
            if (timer >= rotationCD)
            {
                pauseRotation = false;
                timer = 0.0f;

                //choose next angle depending on the index
                index = (index + 1) % angles.Length;

                targetAngle = angles[index];
            }
        }
        else { 
            rotationTick = rotationSpeed * Time.deltaTime;

            float angle  = Mathf.MoveTowards(currentAngle, targetAngle, rotationTick);

            currentAngle = angle;

            transform.rotation = Quaternion.Euler(0.0f, currentAngle, 0.0f);
      

            if (Mathf.Approximately(currentAngle, targetAngle))
            {
                pauseRotation = true;
            }
        }

 

    }
}