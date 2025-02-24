using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    // Experimental - a turbulent current pushes objects around
    public float currentStrength = 1.0f;

    // Expiremental - weighted pull based on relative strength
    private float playerStrength;
    private float fishStrength;

    public bool isActive;
    public float winRadius = 60.0f;

    private CanvasGroup canvas;
    private Slider progressBar;

    public GameObject circleObject;
    private RectTransform circleProgress;

    public delegate Vector2 BehaviourDelegate(Vector2 currentPosition);
    public BehaviourDelegate behaviour;

    Fish fish;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<CanvasGroup>();
        progressBar = GetComponent<Slider>();
        circleProgress = circleObject.GetComponent<RectTransform>();
        catchProgress = catchLimit * 0.3f;
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
        FishBehaviours.UpdateGameState(playerPosition, catchProgress);

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

    public void Activate(Fish f, float rodStrength)
    {
        fish = f;
        FishBehaviours.Initialize();
        switch (f.species) 
        {
            case FishType.Pike:
                behaviour = FishBehaviours.Pike;
                break;
            case FishType.Carp:
                behaviour = FishBehaviours.Carp;
                break;
            case FishType.Trout:
                behaviour = FishBehaviours.Trout;
                break;
            case FishType.GoldScaleSturgeon:
                behaviour = FishBehaviours.GoldScaleSturgon;
                break;
            case FishType.Burbot:
                behaviour = FishBehaviours.Burbot;
                break;
            case FishType.Eel:
                behaviour = FishBehaviours.Eel;
                break;

            default:
                Debug.LogWarning("Trying to use the unfinished fish behaviour " + f.species + ".");
                break;
        }

        fishStrength = fish.strength;
        playerStrength = rodStrength;

        // Default positions for the 3 objects. 0, 0 is the center of the screen.
        playerPosition = new Vector2(-100.0f, 0.0f);
        fishPosition = new Vector2(100.0f, 0.0f);
        bobberPosition = new Vector2(0.0f, 0.0f);

        // Make the canvas visible and interactible
        isActive = true;
        canvas.alpha = 1.0f;
        canvas.blocksRaycasts = true;

        // Reset catch progress
        catchProgress = catchLimit * 0.3f;
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
        // Call on this fish's unique behaviour. All stored in FishBehaviours file. 
        // FishInWaterBehaviour.cs defines a delegate for a function that takes a vec2 and then returns a vec2
        fishPosition = behaviour(fishPosition);
        if (behaviour == FishBehaviours.Pike)
        {
            Debug.Log("Pike");
        }
        else if (behaviour == FishBehaviours.Carp)
        {
            Debug.Log("Carp");
        }
        // Should be redundant, but just in case a fish behaviour places you outside the circle.
        fishPosition = ClampToRadius(fishPosition);
    }

    private void MoveBobber()
    {
        Vector2 directionToPlayer = playerPosition - bobberPosition;
        directionToPlayer.Normalize();

        Vector2 directionToFish = fishPosition - bobberPosition;
        directionToFish.Normalize();

        float totalStrength = playerStrength + fishStrength;
        float playerMult = playerStrength / totalStrength;

        // Minimum value, if your relative strength is less than 35% clamp it so you can always make progress even if the fish is on the outer rim
        playerMult = Mathf.Clamp(playerMult, 0.35f, 1.0f);

        bobberPosition = Vector2.Lerp(fishPosition, playerPosition, playerMult);

        //bobberPosition += directionToPlayer * playerDistance * 0.01f;
        //bobberPosition += directionToFish * fishDistance * 0.01f;

        // Check bobber distance to middle
        if (bobberPosition.magnitude <= winRadius)
        {
            catchProgress += 4.5f * Time.deltaTime;
        }
        else
        {
            catchProgress -= 4.5f * Time.deltaTime;
        }
         
        progressBar.value = catchProgress;

        // Make graphic slightly smaller, so that as soon as the the circle touches its outline you'll win.
        // Also makes losing slightly more generous (it looks like you have a slight buffer after losing to come back).
        // However because of that I need to clamp the value to not be less than 0, or negative scale would look weird.
        float scale = Mathf.Max(catchProgress / catchLimit - 0.04f, 0.0f); 
        circleProgress.localScale = new Vector2(scale, scale);

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
        // 250 looks very good with the current sprite. When art is finalized I'll need to update this
        const float radius = 250.0f;

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
            FishStorage storage = GameObject.Find("FishStorage").GetComponent<FishStorage>();
            storage.AddFish(fish);
        }
        else
        {
            // TODO: Negative consequences for losing? For now just close the game.
        }

        FishingPlayer player = GameObject.Find("FishingPlayer").GetComponent<FishingPlayer>();
        player.TransitionOutOfMinigame();
        Deactivate();
    }
}
