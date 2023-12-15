using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 2.5f;
    
    [SerializeField] private float turnSpeed = 360;
    private Vector3 input;
    void Start()
    {
        // Get the player's rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetPlayerInputs();
        PlayerLook();
    }

    void FixedUpdate() {
        Move();
    }

    void GetPlayerInputs(){
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    void Move(){
        if (input != Vector3.zero){
            rb.MovePosition(transform.position + (transform.forward * input.magnitude) * speed * Time.deltaTime);
        }
    }

    void PlayerLook(){
        if (input != Vector3.zero){

            var relative = (transform.position + input.SkewToIso()) - transform.position;
            var rotation = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        }
    }
}