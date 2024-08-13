using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantBob : MonoBehaviour
{
    public Vector3 up;
    public Vector3 down;
    public float speed;
    bool movingUp;

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
            transform.position = Vector3.MoveTowards(transform.position, up, speed*Time.deltaTime* Vector3.Distance(transform.position, up));
            if(Vector3.Distance(transform.position,up)<.1f)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, down, speed*Time.deltaTime* Vector3.Distance(transform.position, down));
            if (Vector3.Distance(transform.position, down) < .1f)
            {
                movingUp = true;
            }
        }
    }
}
