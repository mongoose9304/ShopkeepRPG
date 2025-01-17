using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    private Vector2 playerPosition;
    private Vector2 fishPosition;
    private Vector2 bobberPosition;

    private Vector2 latestInput;
    public float catchLimit = 50.0f;
    private float catchProgress;

    public bool isActive;

    private CanvasGroup canvas;
    private Slider progressBar;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<CanvasGroup>();
        progressBar = GetComponent<Slider>();
        catchProgress = catchLimit * 0.6f;
        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        // If the minigame is shut down, don't bother.
        if (isActive == false)
            return;

        MovePlayer();
        MoveFish();
        MoveBobber();

        // Synchronize graphics with logic.
        Transform[] pictureTransforms = GetComponentsInChildren<Transform>();
        // Write parent's position as a vec2 so I can easily add it to each sprite's position.
        // This whole game treats the centre of the panel as the origin which simplifies a lot
        // of the gameplay.
        Vector2 positionVec2 = new Vector2(transform.position.x, transform.position.y);
        foreach (Transform spriteTransform in pictureTransforms)
        {
            if (spriteTransform.name == "Player")
            {
                spriteTransform.position = playerPosition + positionVec2;
            }
            else if (spriteTransform.name == "Fish")
            {
                spriteTransform.position = fishPosition + positionVec2;
            }
            else if (spriteTransform.name == "Bobber")
            {
                spriteTransform.position = bobberPosition + positionVec2;
            }
        }
    }

    public void Activate()
    {
        // Default positions for the 3 objects. 0, 0 is the center of the screen.
        playerPosition = new Vector2(-100.0f, 0.0f);
        fishPosition = new Vector2(100.0f, 0.0f);
        bobberPosition = new Vector2(0.0f, 0.0f);

        // Make the canvas visible and interactible
        isActive = true;
        canvas.alpha = 1.0f;
        canvas.blocksRaycasts = true;

        // Reset catch progress
        catchProgress = catchLimit * 0.6f;
    }

    public void Deactivate()
    {
        // Make the canvas invisible
        isActive = false;
        canvas.alpha = 0.0f;
        canvas.blocksRaycasts = false;
    }

    // TODO: Process inputs in here.
    public void HandleInputs(Vector2 input)
    {
        latestInput = input;
    }

    private void MovePlayer()
    {
        if (latestInput.magnitude < 0.07f)
        {
            // Ignore very small inputs.
            return;
        }

        Vector2 direction = latestInput.normalized;
        playerPosition += direction * 0.7f;
        playerPosition = ClampToRadius(playerPosition);
    }

    private void MoveFish()
    {
        // Jitter around, later each fish will have a unique behaviour here.
        fishPosition += new Vector2(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f));

        fishPosition = ClampToRadius(fishPosition);
    }

    private void MoveBobber()
    {
        Vector2 directionToPlayer = playerPosition - bobberPosition;
        float playerDistance = directionToPlayer.magnitude;
        directionToPlayer.Normalize();
        Vector2 directionToFish = fishPosition - bobberPosition;
        float fishDistance = directionToFish.magnitude;
        directionToFish.Normalize();

        bobberPosition += directionToPlayer * playerDistance / 100.0f;
        bobberPosition += directionToFish * fishDistance / 100.0f;

        // Check bobber distance to middle
        Debug.Log(bobberPosition.magnitude);
        if (bobberPosition.magnitude < 100.0f)
        {
            catchProgress += 4.5f * Time.deltaTime;
        }
        else
        {
            catchProgress -= 3.5f * Time.deltaTime;
        }
        progressBar.value = catchProgress;

        if (catchProgress <= 0.0f)
        {
            EndGame(false);
            catchProgress = 0.0f;
        }
        else if (catchProgress > catchLimit)
        {
            EndGame(true);
            catchProgress = catchLimit;
        }
    }

    private Vector2 ClampToRadius(Vector2 inputPosition)
    {
        const float radius = 250.0f;

        // 250 max radius for now, can update this with testing.
        // Should just keep the fish icon within the circle graphic.
        if (inputPosition.magnitude > radius)
        {
            return inputPosition.normalized * radius;
        }
        else
        {
            return inputPosition;
        }
    }

    private void EndGame(bool hasWon)
    {
        if (hasWon)
        {
            // TODO: Add a fish to your inventory
        }
        else
        {
            // TODO: Negative consequences for losing?
        }

        GameObject.FindGameObjectWithTag("Player").GetComponent<FishingPlayer>().canMove = true;
        Deactivate();
    }
}
