using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    // Player movement speed
    public float moveSpeed = 5.0f;
    // Player turn speed
    public float turnSpeed = 10.0f;
    // Player jump force
    public float jumpForce = 10.0f;
    // Camera rotation speed
    public float cameraRotationSpeed = 2.0f;
    // Camera follow speed
    public float cameraFollowSpeed = 5.0f;
    // Layer mask for ground check
    public LayerMask groundLayer;
    // Transform for camera pivot
    public Transform cameraPivot;

    // Get the player's rigidbody
    private Rigidbody rb;
    // Get the main camera's transform
    private Transform playerCamera;
    // Check if the player is grounded
    [SerializeField] bool isGrounded;
    // Player horizontal rotation
    private float horizontalRotation;
    // Player vertical rotation
    private float verticalRotation;

    void Start()
    {
        // Get the player's rigidbody
        rb = GetComponent<Rigidbody>();
        // Get the main camera's transform
        playerCamera = Camera.main.transform;
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(transform.position, 1.5f, groundLayer);

        // Get input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 forward = playerCamera.forward;
        Vector3 right = playerCamera.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        Vector3 moveDirection = forward * verticalInput + right * horizontalInput;

        // Apply movement
        Vector3 velocity = moveDirection * moveSpeed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        // Rotate the player to face the direction of movement
        if (moveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
        }

        // Rotate the camera
        float cameraRotationY = Input.GetAxis("Mouse X") * cameraRotationSpeed;
        float cameraRotationX = -Input.GetAxis("Mouse Y") * cameraRotationSpeed;
        horizontalRotation += cameraRotationY;
        verticalRotation += cameraRotationX;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);

        // Apply camera rotation
        cameraPivot.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, horizontalRotation, 0);

        // Move the camera to follow the player smoothly
        Vector3 targetCameraPos = transform.position - transform.forward * 5.0f + Vector3.up * 2.0f;
        playerCamera.position = Vector3.Lerp(playerCamera.position, targetCameraPos, cameraFollowSpeed * Time.deltaTime);

        // Make the camera automatically look at the player
        playerCamera.LookAt(transform.position + transform.up * 1.5f);
        
        // Jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // Apply jump force
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}