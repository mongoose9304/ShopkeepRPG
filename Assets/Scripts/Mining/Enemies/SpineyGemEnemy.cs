using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineyGemEnemy : BasicMiningEnemy
{
    bool isRotating;
    public float rotationSpeed;
    [SerializeField] float startRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
                if (transform.localEulerAngles.y<100&& transform.localEulerAngles.y>80)
                {
                    transform.localEulerAngles = new Vector3(0,90,0);
                    isRotating = false;
                }
            }
            if (startRotation == 90)
            {
                if (transform.localEulerAngles.y > 260 && transform.localEulerAngles.y < 280)
                {
                    transform.localEulerAngles = new Vector3(0, 270, 0);
                    isRotating = false;
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
}
