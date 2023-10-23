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
    Vector3 Vec;


    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
       
        // just a simple jump function
        //works well with the movement of player.
        //speed can be adjusted by increasing the Y and or increase the 6 to more.
        // the speed to side to side maybe can be less but i like the movement and jump 
        //movement feels ok i think.


        //also the jump though needs a condition if player is touching ground but thats after.
        //im happy with this for starting out movement but can be adjusted as we go.

        if (Input.GetKeyDown("space"))
        {
            rb.velocity = new Vector3(0, 6, 0);

        }

        Vec = transform.localPosition;
        
        Vec.x += Input.GetAxis("Horizontal") * Time.deltaTime * 20;
        Vec.z += Input.GetAxis("Vertical") * Time.deltaTime * 20;
        transform.localPosition = Vec;

       




    }

   
}