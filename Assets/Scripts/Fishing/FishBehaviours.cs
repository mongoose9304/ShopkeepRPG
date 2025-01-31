using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MoveType
{
    None,
    AngleDistance,
    Chase,
}

public class FishBehaviours
{
    // How many seconds until he moves again.
    private static float moveDelay;
    // Whether or not the fish is moving
    private static bool isMoving;
    private static MoveType moveType;
    // The point on the circle the fish is moving towards (measured in degrees
    private static float targetAngleDegrees;
    // How far from the center of the circle he's trying to go
    private static float targetDistance;
    // Player's current position
    private static Vector2 playerPosition;
    // Catch progress, used in some fish to change behaviour based on how close the player is to catching them.
    private static float catchProgress;

    public static void Initialize()
    {
        moveDelay = 0.0f;
        isMoving = false;
        moveType = MoveType.None;
        targetAngleDegrees = Random.Range(0.0f, 360.0f);
        targetDistance = 100.0f;
    }

    public static void UpdateGameState(Vector2 playerPos_, float catchProgress_)
    {
        playerPosition = playerPos_;
        catchProgress = catchProgress_;
    }

    // Totally random target angle and target distance
    public static Vector2 CompleteRandom(Vector2 currentPos)
    {
        // Relative angle to right
        float currentAngleDegrees = Vector2.SignedAngle(new Vector2(1.0f, 0.0f), currentPos);
        float currentDistance = currentPos.magnitude;

        // This will be called like an Update
        if (isMoving == false)
        {
            moveDelay -= Time.deltaTime;
            if (moveDelay <= 0.0f)
            {
                targetDistance = Random.Range(0.0f, 250.0f);
                targetAngleDegrees = Random.Range(0.0f, 360.0f);
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

    // Very aggressive, swims around fast and will rush towards your position when desperate
    public static Vector2 Pike(Vector2 currentPos)
    {
        Debug.Log("Pike");

        // Relative angle to right
        float currentAngleDegrees = Vector2.SignedAngle(new Vector2(1.0f, 0.0f), currentPos);
        float currentDistance = currentPos.magnitude;

        // This will be called like an Update
        if (isMoving == false)
        {
            moveDelay -= Time.deltaTime;
            if (moveDelay <= 0.0f)
            {
                // Choose between a normal move, or a chase move
                if (Random.value <= 0.2f)
                {
                    moveType = MoveType.Chase;
                }
                else
                {
                    moveType = MoveType.AngleDistance;
                    int mult = Random.Range(0, 2);
                    if (mult == 0)
                    {
                        mult = -1;
                    }

                    // 70% chance to move normally
                    targetDistance = currentDistance + Random.Range(50.0f, 100.0f);
                    // Because the fish always takes the shortest path to the angle, the biggest move he can do is 180.
                    targetAngleDegrees = currentAngleDegrees + Random.Range(160.0f, 200.0f) * mult;
                }

                isMoving = true;

                // Clamp angle to range, causes some problems if this isn't done
                if (targetAngleDegrees >= 360.0f)
                {
                    targetAngleDegrees -= 360.0f;
                }
                else if (targetAngleDegrees < 0.0f)
                {
                    targetAngleDegrees += 360.0f;
                }

            }
        }
        else
        {
            if (moveType == MoveType.AngleDistance)
            {
                // Classic stuff, lerp towards the angle and distance you need
                float angleDifference = Mathf.DeltaAngle(currentAngleDegrees, targetAngleDegrees);
                currentAngleDegrees += angleDifference * 0.01f; // move 1% of the way there each frame.

                currentDistance = Mathf.Lerp(currentDistance, targetDistance, 0.001f);

                if (Mathf.Abs(angleDifference) < 5.0f)
                {
                    isMoving = false;
                    moveDelay = Random.Range(0.3f, 1.0f);
                }
            }
            else if (moveType == MoveType.Chase)
            {
                if (Vector2.Distance(currentPos, playerPosition) < 100.0f)
                {
                    isMoving = false;
                    moveDelay = Random.Range(0.2f, 0.6f);
                }
                return Vector2.Lerp(currentPos, playerPosition, 0.01f);
            }
        }

        // Reconstruct the position vector using current angle and distance
        return new Vector2(Mathf.Cos(currentAngleDegrees * Mathf.Deg2Rad), Mathf.Sin(currentAngleDegrees * Mathf.Deg2Rad)) * currentDistance;
    }
}
