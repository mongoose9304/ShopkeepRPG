using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseRock : MonoBehaviour
{
    public float damage;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            other.GetComponent<MiningPlayer>().TakeDamage(damage);
        }
        if (other.tag == "Wall")
        {
            other.gameObject.SetActive(false);
        }
        if (other.tag == "Rock")
        {
            other.gameObject.SetActive(false);
        }
        if (other.tag == "EndZone")
        {
            //GetComponent<ConstantMoveForward>().enabled = false;
            //GetComponentInChildren<ConstantRotate>().enabled = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
