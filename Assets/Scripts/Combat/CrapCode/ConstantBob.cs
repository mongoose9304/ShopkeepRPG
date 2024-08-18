using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantBob : MonoBehaviour
{
    public Vector3 up;
    public Vector3 down;
    public float speed;
    bool movingUp;
    public bool changeSpeedBasedOnDistance;
    private void Start()
    {
        up += transform.position;
        down += transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if(movingUp)
        {
            float x = speed * Time.deltaTime;
            if (changeSpeedBasedOnDistance)
                x *= Vector3.Distance(transform.position, up);
            transform.position = Vector3.MoveTowards(transform.position, up,x);
            if(Vector3.Distance(transform.position,up)<.1f)
            {
                movingUp = false;
            }
        }
        else
        {
            float x = speed * Time.deltaTime;
            if (changeSpeedBasedOnDistance)
                x *= Vector3.Distance(transform.position, down);
            transform.position = Vector3.MoveTowards(transform.position, down,x);
            if (Vector3.Distance(transform.position, down) < .1f)
            {
                movingUp = true;
            }
        }
    }
}
