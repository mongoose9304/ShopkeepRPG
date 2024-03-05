using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingAttack : MonoBehaviour
{
    [SerializeField] float maxLifeTime;
    [SerializeField] float moveSpeed;
    [SerializeField] float lookSpeed;
    [SerializeField] float homingCheckDelayMax;
    float homingCheckDelay;
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
            homingCheckDelay -= Time.deltaTime;
            if (homingCheckDelay <= 0)
            {
                homingCheckDelay = homingCheckDelayMax;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position+transform.forward*2, 3);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.transform.tag == homingTag)
                    {
                        if (!target)
                            target = hitCollider.transform;
                        if(Vector3.Distance(transform.position,target.position)> Vector3.Distance(transform.position,hitCollider.transform.position))
                        target = hitCollider.transform;
                    }
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
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag!="Player")
        this.gameObject.SetActive(false);
    }

}

