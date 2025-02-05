using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingAttack : MonoBehaviour
{
    [SerializeField] float maxLifeTime;
    [SerializeField] float moveSpeed;
    [SerializeField] float lookSpeed;
    [SerializeField] bool isHoming;
    public Transform target;

    //Extra stuff courtesy of Adriel
    [HideInInspector] public float moveSpeedBonus;
    [HideInInspector] public float lifeTimeBonus;
    [HideInInspector] public enum HomingType { smooth, sharp }
    [HideInInspector] public HomingType homingType = HomingType.smooth; 

    Quaternion rotation;
    Vector3 direction;
    float currentLifeTime;

    private void Update()
    {
        //look at target if you have one
        if (target)
        {
            direction = target.position - transform.position;
            direction.y = 0; // keep the direction strictly horizontal
            rotation = Quaternion.LookRotation(direction);
            // slerp to the desired rotation over time*
            //* if the homing type is smooth
            switch (homingType) {
                case HomingType.smooth:
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, lookSpeed * Time.deltaTime);
                    break;

                case HomingType.sharp:
                    transform.rotation = rotation;
                    break;
            }
            if(!target.gameObject.activeInHierarchy)
            { 
                target = null;
            }
        }
       

        transform.position += (transform.forward * Time.deltaTime * (moveSpeed + moveSpeed * moveSpeedBonus));
        currentLifeTime -= Time.deltaTime;
        if(currentLifeTime<=0)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        currentLifeTime = maxLifeTime + lifeTimeBonus;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag =="Wall")
        {
          
            this.gameObject.SetActive(false);
        }
    }

}

