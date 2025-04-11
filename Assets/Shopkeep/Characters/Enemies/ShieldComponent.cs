using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldComponent : MonoBehaviour
{
    private float activeUntil;
    private float protectionAngle;
    private bool active = false;

    public void Activate(float duration, float angle)
    {
        activeUntil = Time.time + duration;
        protectionAngle = angle;
        active = true;
    }
    public bool Block(Vector3 incomingDirection)
    {
        if (!active || Time.time > activeUntil)
        {
            active = false;
            return false;
        }

        Vector3 forward = transform.forward;
        incomingDirection.y = 0;
        forward.y = 0;

        float angle = Vector3.Angle(-incomingDirection, forward);
        return angle <= protectionAngle * 0.5f;
    }
}
