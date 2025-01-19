using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishInWaterBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    public Rigidbody playerRB = null;
    public GameObject playerRef;

    // How skittish this fish is: how much time you need to spend inside their radius before
    // they will swim deep under water
    public float skittishness = 0.4f;
    public float scareRadius = 4.0f;
    // The time this fish will wait after moving before moving again.
    // Randomized after each movement.
    public float moveDelay = 1.8f;

    public float baitRadius = 3.0f;
    public float maxVisionAngle = 25.0f;

    private float angle;
    private float speed;
    private bool isFleeing;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        angle = Random.Range(0.0f, 360.0f);
        RotateToFace();
        speed = Random.Range(1.0f, 5.0f);
        isFleeing = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject bobber = GameObject.FindGameObjectWithTag("Bobber");
        if (bobber != null)
        {
            if (bobber.GetComponent<BobberLogic>().isActive == true)
            {
                float bobberDistance = Vector3.Distance(rb.position, bobber.transform.position);
                if (bobberDistance <= baitRadius)
                {
                    float cosBobberAngle = Vector3.Dot(transform.forward, Vector3.Normalize(bobber.transform.position - transform.position));
                        //Vector3.Angle(rb.transform.forward, Vector3.Normalize(bobber.transform.position - rb.position));
                    if (cosBobberAngle <= Mathf.Cos(maxVisionAngle))
                    {
                        Debug.Log("Start fishing minigame...");
                        GameObject.FindGameObjectWithTag("Player").GetComponent<FishingPlayer>().InitiateMinigame();
                        Destroy(gameObject);
                        Destroy(bobber);
                    }
                }
            }
        }

        // If the player exists
        if (playerRB != null)
        {
            // Check if player is too close
            if (Vector3.Distance(rb.position, playerRB.position) < scareRadius)
            {
                skittishness -= Time.deltaTime;
                if (skittishness <= 0.0f)
                {
                    isFleeing = true;
                }

                Vector3 runDirection = rb.position - playerRB.position;
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
