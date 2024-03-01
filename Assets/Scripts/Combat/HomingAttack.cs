using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingAttack : MonoBehaviour
{
    [SerializeField] float maxLifeTime;
    [SerializeField] float moveSpeed;
    [SerializeField] float lookSpeed;
    [SerializeField] bool isHoming;
    [SerializeField] string homingTag;
    Transform target;
    Quaternion rotation;
    Vector3 direction;
    float currentLifeTime;
    RaycastHit hit;
    private void Update()
    {
        //look at target if you have one
        if (target)
        {
            direction = target.position - transform.position;
            direction.y = 0; // keep the direction strictly horizontal
            rotation = Quaternion.LookRotation(direction);
            // slerp to the desired rotation over time
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, lookSpeed * Time.deltaTime);
        }
        else
        {
            if(Physics.Raycast(transform.position, transform.forward,out hit,10))
            {
              if( hit.transform.tag==homingTag)
                {
                    target = hit.transform;
                }
            }
        }

        transform.position += (transform.forward * Time.deltaTime * moveSpeed);
        currentLifeTime -= Time.deltaTime;
        if(currentLifeTime<=0)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        currentLifeTime = maxLifeTime;
    }

}

