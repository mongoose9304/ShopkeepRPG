using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 2.5f;
    
    [SerializeField] private float turnSpeed = 720;
    [SerializeField] private Transform model;
    private Vector3 input;
    public Inventory playerInventory;
    void Start()
    {

    }

    void Update()
    {
        if (playerInventory.isOpen() == false){
            GetPlayerInputs();
        }
        PlayerLook();
    }

    void FixedUpdate() {
        Move();
    }

    void GetPlayerInputs(){
        // gets both controller and keyboard input. theres a better way for gamepad using 
        // Vector2 stickValue = Gamepad.current.leftStick.ReadValue();
        // input = new Vector3(stickValue.x, 0, stickValue.y);
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    void Move(){
        // If we want jumping we may need to find another way to handle player movement, if you jump and dont press any buttons
        // you'll just fall vertical.
        if (input != Vector3.zero){
            Vector3 movement = input.normalized * speed * Time.deltaTime;
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        } else {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void PlayerLook(){
        // TODO: add a model for testin
        if (input == Vector3.zero) return;

        // this logic is specifically for having a model be a child of the gameobject with a rigidbody (rigidbody doesn't rotate)
        Quaternion rotation = Quaternion.LookRotation(input.normalized, Vector3.up);
        model.rotation = Quaternion.RotateTowards(model.rotation, rotation, turnSpeed * Time.deltaTime);
        
    }
}