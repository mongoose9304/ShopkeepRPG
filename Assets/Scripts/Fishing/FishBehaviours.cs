using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehaviours
{
    // How many seconds until he moves again.
    private static float moveDelay;
    // Whether or not the fish is moving
    private static bool isMoving;
    // The point on the circle the fish is moving towards (measured in degrees
    private static float targetAngleDegrees;
    // How far from the center of the circle he's trying to go
    private static float targetDistance;

    public static void Initialize()
    {
        moveDelay = 0.0f;
        isMoving = false;
        targetAngleDegrees = Random.Range(0.0f, 360.0f);
        targetDistance = 100.0f;
    }

    // Totally random target angle and target distance
    public static Vector2 CompleteRandom(Vector2 currentPos)
    {
        // Relative angle to right
        float currentAngleDegrees = Vector2.SignedAngle(new Vector2(1.0f, 0.0f), currentPos);
        float currentDistance = currentPos.magnitude;

        Debug.Log("moveDelay: " + moveDelay);
        Debug.Log("isMoving: " + isMoving);

        // This will be called like an Update
        if (isMoving == false)
        {
            moveDelay -= Time.deltaTime;
            if (moveDelay <= 0.0f)
            {
                targetDistance = Random.Range(80.0f, 250.0f);
                targetAngleDegrees = Random.Range(0.0f, 360.0f);
                isMoving = true;
            }
        }
        else
        {
            float angleDifference = Mathf.DeltaAngle(currentAngleDegrees, targetAngleDegrees);
            currentAngleDegrees += angleDifference * 0.005f; // move .5% of the way there each frame.

            currentDistance = Mathf.Lerp(currentDistance, targetDistance, 0.001f);

            Debug.Log("Target: " + targetAngleDegrees);
            Debug.Log("Current: " + currentAngleDegrees);

            if (Mathf.Abs(angleDifference) < 2.0f)
            {
                Debug.Log("Reached Target");
                isMoving = false;
                moveDelay = Random.Range(0.5f, 3.5f);
            }
        }

        // Reconstruct the position vector using current angle and distance
        return new Vector2(Mathf.Cos(currentAngleDegrees * Mathf.Deg2Rad), Mathf.Sin(currentAngleDegrees * Mathf.Deg2Rad)) * currentDistance;
    }

    // Carp are tricky fish, once they know they're on a fishing line they'll
    // try to wrap it around rocks or logs to get away. This fish will move in long
    // arcs, 70-90% of the way from the center to try and throw you off.
    public static Vector2 Carp(Vector2 currentPos)
    {
        Debug.Log("Carp");

        // Relative angle to right
        float currentAngleDegrees = Vector2.SignedAngle(new Vector2(1.0f, 0.0f), currentPos);
        float currentDistance = currentPos.magnitude;

        // This will be called like an Update
        if (isMoving == false)
        {
            moveDelay -= Time.deltaTime;
            if (moveDelay <= 0.0f)
            {
                int mult = Random.Range(0, 2);
                if (mult == 0)
                {
                    mult = -1;
                }

                targetDistance = Random.Range(180.0f, 230.0f);
                targetAngleDegrees = currentAngleDegrees + Random.Range(100.0f, 220.0f) * mult;
                if (targetAngleDegrees >= 360.0f)
                {
                    targetAngleDegrees -= 360.0f;
                }
                else if (targetAngleDegrees < 0.0f)
                {
                    targetAngleDegrees += 360.0f;
                }
                isMoving = true;
            }
        }
        else
        {
            float angleDifference = Mathf.DeltaAngle(currentAngleDegrees, targetAngleDegrees);
            currentAngleDegrees += angleDifference * 0.005f; // move .5% of the way there each frame.

            currentDistance = Mathf.Lerp(currentDistance, targetDistance, 0.001f);

            if (Mathf.Abs(angleDifference) < 2.0f)
            {
                isMoving = false;
                moveDelay = Random.Range(0.1f, 3.0f);
            }
        }

        // Reconstruct the position vector using current angle and distance
        return new Vector2(Mathf.Cos(currentAngleDegrees * Mathf.Deg2Rad), Mathf.Sin(currentAngleDegrees * Mathf.Deg2Rad)) * currentDistance;
    }
}
