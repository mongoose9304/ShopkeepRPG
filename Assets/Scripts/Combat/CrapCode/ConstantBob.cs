using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ConstantBob : MonoBehaviour
{
    public Vector3 up;
    public Vector3 down;
    public float speed;
    bool movingUp;
    public bool changeSpeedBasedOnDistance;
    public float speedInceraseOnGoingDown = 1;
    public bool useEvents;
    public UnityEvent endDownEvent;
    public UnityEvent endUpEvent;
    private void OnEnable()
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
                if (useEvents)
                {
                    endUpEvent.Invoke();
                }
            }
        }
        else
        {
            float x = speed * Time.deltaTime;
            if (changeSpeedBasedOnDistance)
                x *= Vector3.Distance(transform.position, down);
            transform.position = Vector3.MoveTowards(transform.position, down,x*speedInceraseOnGoingDown);
            if (Vector3.Distance(transform.position, down) < .1f)
            {
                movingUp = true;
                if(useEvents)
                {
                    endDownEvent.Invoke();
                }
            }
        }
    }
}
