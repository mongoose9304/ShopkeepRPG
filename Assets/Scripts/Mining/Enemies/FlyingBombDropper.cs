using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBombDropper : BasicMiningEnemy
{
    Vector3 moveDirection;
    [SerializeField] float maxTimeMovingADirection;
    [SerializeField] float minTimeMovingADirection;
    float currentTimeMovingADirection;
    

    private void Update()
    {
         currentTimeMovingADirection -= Time.deltaTime;
         if (currentTimeMovingADirection <= 0)
             Rotate();
        Move();
    }
    private void Move()
    {
    
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    private void Rotate()
    {
        currentTimeMovingADirection = Random.Range(minTimeMovingADirection, maxTimeMovingADirection);
        float x = Random.Range(0, 4);
       if(moveDirection==Vector3.right)
        {
            if (x == 1)
                x = 0;
        }
        else if (moveDirection == Vector3.left)
        {
            if (x == 2)
                x = 3;
        }
        else if (moveDirection == Vector3.forward)
        {
            if (x == 3)
                x = 1;
        }
        else if (moveDirection == Vector3.back)
        {
            if (x == 0)
                x = 2;
        }
        switch (x)
        {
            case 1:
                moveDirection = Vector3.right;
                break;
            case 2:
                moveDirection = Vector3.left;
                break;
            case 3:
                moveDirection = Vector3.forward;
                break;
            case 0:
                moveDirection = Vector3.back;
                break;
        }
    }
}
