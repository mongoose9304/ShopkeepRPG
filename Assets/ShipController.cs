using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private Vector3 inputAxis;
    private float accelerationSpeed = 2.0f;
    private Vector3 velocity = new Vector3();
    private float angle = 0.0f;
    private float targetAngle = 0.0f;

    private float oilTime = 0.0f;
    private float velMult = 1.0f;
    private bool velocityResetFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (oilTime > 0.0f)
        {
            oilTime -= Time.deltaTime;

            angle += Time.deltaTime * 5.0f;

            Vector3 spinAxis = new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle));
            transform.LookAt(transform.position + spinAxis);
        }
        else
        {
            Vector3 acceleration = inputAxis * accelerationSpeed;


            // Drag force
            Vector3 drag = velocity * -0.2f;
            acceleration += drag;

            velocity += acceleration * Time.deltaTime; // Shoutout Isaac Newton

            transform.LookAt(transform.position + velocity.normalized);
        }

        if (velMult < 1.0f)
        {
            velMult += Time.deltaTime * 2.0f;
            if (velMult >= 0.0f && velocityResetFlag == true)
            {
                velocity = new Vector3(0.0f, 0.0f, 0.0f);
                velocityResetFlag = false;
            } 
        }
        else
        {
            velMult = 1.0f;
        }

        transform.position += velocity * Time.deltaTime * velMult;
    }

    public void PassInputs(Vector3 lstick)
    {
        inputAxis = lstick;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            velMult = -2.3f;
            velocityResetFlag = true;
        }
        else if (other.gameObject.tag == "OilSlick")
        {
            if (oilTime <= 0.01f)
            {
                angle = Vector3.Angle(new Vector3(1.0f, 0.0f, 0.0f), velocity);
                velocity *= 1.3f; // Speed boost
                oilTime = 3.0f;
            }
        }
    }
}
