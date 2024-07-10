using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineyGemEnemy : BasicMiningEnemy
{
    bool isRotating;
    public float rotationSpeed;
    [SerializeField] float startRotation;
    bool isGrounded=true;
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        if (!isGrounded)
            DetectObstacle();
        Rotate();
        Move();
    }
    private void Move()
    {
        if(!isRotating)
        transform.position += transform.forward * moveSpeed*Time.deltaTime;
    }
    private void Rotate()
    {
        if(isRotating)
        {
            transform.localEulerAngles += new Vector3(0, 1, 0) * Time.deltaTime * rotationSpeed;
            
            if(startRotation==270)
            {
                if (transform.localEulerAngles.y<92&& transform.localEulerAngles.y>88)
                {
                    transform.localEulerAngles = new Vector3(0,90,0);
                    isRotating = false;
                    isGrounded = true;
                }
            }
            if (startRotation == 90)
            {
                if (transform.localEulerAngles.y > 268 && transform.localEulerAngles.y < 272)
                {
                    transform.localEulerAngles = new Vector3(0, 270, 0);
                    isRotating = false;
                    isGrounded = true;
                }
            }

        }
    }
    public override void DetectObstacle()
    {
        if (isRotating)
            return;
        startRotation = transform.localEulerAngles.y;
        if (startRotation > 260 && startRotation < 280)
            startRotation = 270;
        else
        {
            startRotation = 90;
        }
        isRotating = true;
    }
    public override void DetectNoGround()
    {
        if(!isRotating)
        isGrounded = false;
    }
    public override void DetectGround()
    {
        if (!isRotating)
            isGrounded = true;
    }
}
