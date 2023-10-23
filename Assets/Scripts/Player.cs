using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class Player : MonoBehaviour
{
  //Random Varible ideas

    Rigidbody rb;

    public float speed = 3;



    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        TestingUpdate();

        // Testing out the movement of player.

        if (Input.GetKeyDown("space"))
        {
            rb.velocity = new Vector3(0, 6, 0);

        }





       
    }


    //throwing in random functions can be moved after to whatever or stay.
   
    
    private void TestingUpdate()
    {
        Vector3 MovingAllDirections = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        transform.Translate(Vector3.forward * speed * Time.deltaTime);



    }

}