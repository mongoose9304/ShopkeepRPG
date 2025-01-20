using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    private float rotationTick = 0.0f;

    private float[] angles = { 0.0f, 45.0f, 90.0f, 135.0f, 180.0f };
    private int index = 0;


    //information needed for the field of view 
    private float viewDistance = 40.0f;
    private float viewAngle = 90.0f;
    private int amountOfRays = 10;


    // Start is called before the first frame update
    void Start()
    {
        RayOrigin = transform.position;
        RayDirection = transform.forward;

       targetAngle = angles[0];
    }

 

    // Update is called once per frame
    void Update()
    {
       

        Patrol();
        CastRays();
 

    }

    void CastRays()
    {
        //this is to ganerate the cone- like field of view. 
        //Since for this mob we cant use simple colliders

        float startingPoint = -viewAngle / 2;
        float angleBetweenRays = viewAngle / amountOfRays;

        for (int i = 0; i < amountOfRays; i++)
        {
            float newAngle = startingPoint + angleBetweenRays * i;
            RayDirection = Quaternion.Euler(0, newAngle, 0) * transform.forward;

            if (Physics.Raycast(RayOrigin, RayDirection, out RaycastHit hit, viewDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Player detected");
                }

            }


        }
    }
    //i want to make the pause, rotate, pause, rotate movement for this mob.
    void Patrol() {

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
        else
        {
            rotationTick = rotationSpeed * Time.deltaTime;

            float angle = Mathf.MoveTowards(currentAngle, targetAngle, rotationTick);

            currentAngle = angle;

            transform.rotation = Quaternion.Euler(0.0f, currentAngle, 0.0f);


            if (Mathf.Approximately(currentAngle, targetAngle))
            {
                pauseRotation = true;
            }
        }
    }
}


