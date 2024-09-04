using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ConstantBob : MonoBehaviour
{
    public Vector3 up;
    private Vector3 trueUp;
    public Vector3 down;
    public Vector3 trueDown;
    public Vector3 originalPosition;
    public float speed;
    bool movingUp;
    public bool changeSpeedBasedOnDistance;
    public float speedInceraseOnGoingDown = 1;
    public bool useEvents;
    public UnityEvent endDownEvent;
    public UnityEvent endUpEvent;
    private bool isQuitting;
    private void OnEnable()
    {
        originalPosition = transform.position;
        trueDown = transform.position+down;
        trueUp = transform.position+up;
    }
    // Update is called once per frame
    void Update()
    {
        if(movingUp)
        {
            float x = speed * Time.deltaTime;
            if (changeSpeedBasedOnDistance)
                x *= Vector3.Distance(transform.position, trueUp);
            transform.position = Vector3.MoveTowards(transform.position, trueUp, x);
            if(Vector3.Distance(transform.position, trueUp) <.1f)
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
                x *= Vector3.Distance(transform.position, trueDown);
            transform.position = Vector3.MoveTowards(transform.position, trueDown, x*speedInceraseOnGoingDown);
            if (Vector3.Distance(transform.position, trueDown) < .1f)
            {
                movingUp = true;
                if(useEvents)
                {
                    endDownEvent.Invoke();
                }
            }
        }
    }
    private void OnDisable()
    {
        if (isQuitting)
            return;
        transform.position = originalPosition;
    }
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }
}
