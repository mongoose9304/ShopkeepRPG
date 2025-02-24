using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum FishType
{
    Bass,
    Trout,
    Carp,
    Pike,
    Burbot,
    Eel,
    GoldScaleSturgeon
}

public class Fish
{
    public Fish(FishType _species, float _size)
    {
        species = _species;
        size = _size;

        switch (species)
        {
            case FishType.Bass:
                name = "Bass";
                strength = 5.0f;
                break;
            case FishType.Pike:
                name = "Pike";
                strength = 5.0f;
                break;
            case FishType.Trout:
                name = "Trout";
                strength = 3.0f;
                break;
            case FishType.Carp:
                name = "Carp";
                strength = 1.5f;
                break;
            case FishType.GoldScaleSturgeon:
                name = "Gold Scale Sturgeon";
                strength = 12.0f;
                break;
            case FishType.Burbot:
                name = "Burbot";
                strength = 200.0f;
                break;
            case FishType.Eel:
                name = "Eel";
                strength = 2.5f;
                break;
            default:
                name = "Undefined";
                strength = 1.0f;
                break;
        }
    }

    public Fish()
    {
        species = FishType.Pike;
        size = 1.0f;
        strength = 1.0f;
        name = "Undefined";
    }

    public string GetName()
    {
        return name;
    }

    // Final fishItem stats:
    public FishType species;
    public float size;
    public string name;
    public float strength;
}

public class FishInWaterBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    public Rigidbody playerRB = null;
    public GameObject playerRef;
    public GameObject ship = null;
    public Material goldMaterial;

    private float ystart;


    // How skittish this fish is: how much time you need to spend inside their radius before
    // they will swim deep under water
    public float skittishness = 0.4f;
    // How close you can get without the fish swimming away
    public float scareRadius = 30.0f;
    // The time this fish will wait after moving before moving again.
    // Randomized after each movement.
    public float moveDelay = 1.8f;

    public float baitRadius = 5.0f;
    public float maxVisionAngle = 25.0f;

    private float angle;
    private float speed;
    private bool isFleeing;
    public float targetY;

    public Fish fish = new Fish();

    private bool hasNoticedBobber = false;
    private float bobberDistance = 0.0f;
    private float targetBobberDistance = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        angle = Random.Range(0.0f, 360.0f);
        RotateToFace();
        speed = Random.Range(1.0f, 5.0f);
        isFleeing = false;
        ystart = transform.position.y;

        ship = GameObject.Find("Ship");

        // Make the rare fish appear as a different material.
        // Can add a seperate mesh too once art starts coming in.
        if (fish.species == FishType.GoldScaleSturgeon)
        {
            GetComponent<Renderer>().material = goldMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, targetY, 0.01f), transform.position.z);

        GameObject bobber = GameObject.FindGameObjectWithTag("Bobber");
        if (bobber != null)
        {
            if (bobber.GetComponent<BobberLogic>().isActive == true)
            {
                float bobberDistance = Vector3.Distance(transform.position, bobber.transform.position);
                if (bobberDistance <= baitRadius && bobber.GetComponent<BobberLogic>().isActive == true)
                {
                    hasNoticedBobber = true;
                    targetBobberDistance = 0.5f;
                }
            }

            if (hasNoticedBobber == true)
            {
                Vector3 targetPos = new Vector3(bobber.transform.position.x, -0.5f, bobber.transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.01f);

                if (Vector3.Distance(transform.position, bobber.transform.position) <= 1.0f)
                {
                    // Biting the hook
                    GameObject.Find("FishingPlayer").GetComponent<FishingPlayer>().InitiateMinigame(fish);
                    Destroy(gameObject);
                    Destroy(bobber);
                }
            }
        }
        else
        {
            hasNoticedBobber = false;
        }

     

        // If the player exists
        if (playerRB != null)
        {
            // Check if player is too close
            if (Vector3.Distance(rb.position,   ship.transform.position) < scareRadius)
            {
                skittishness -= Time.deltaTime;
                if (skittishness <= 0.0f)
                {
                    isFleeing = true;
                }

                Vector3 runDirection = rb.position - ship.transform.position;
                angle = Mathf.Rad2Deg * Mathf.Atan2(runDirection.z, runDirection.x);
                RotateToFace();
                speed = 7.0f;
            }
        }
        else
        {
            // Check if player has joined
            playerRef = GameObject.FindGameObjectWithTag("Player");
            if (playerRef != null)
            {
                playerRB = playerRef.GetComponent<Rigidbody>();
            }
        }

        speed = Mathf.Lerp(speed, 0.0f, 0.005f);

        if (speed < 0.01f)
        {
            moveDelay -= Time.deltaTime;
            if (moveDelay <= 0.0f)
            {
                moveDelay = Random.Range(0.1f, 1.0f);
                speed = Random.Range(1.0f, 6.0f);
                angle = Random.Range(0.0f, 360.0f);
                RotateToFace();
            }
        }

        float vertSpeed = 0.0f;
        if (isFleeing == true)
        {
            vertSpeed = -0.2f;
            if (transform.position.y < -1.0f)
            {
                Destroy(gameObject);
            }
        }

        float angleRad = Mathf.Deg2Rad * angle;
        rb.velocity = new Vector3(Mathf.Cos(angleRad), vertSpeed, Mathf.Sin(angleRad)) * speed;
    }

    void RotateToFace() {
        transform.rotation = Quaternion.AngleAxis(angle - 90.0f, new Vector3(0.0f, -1.0f, 0.0f));
    }
}
