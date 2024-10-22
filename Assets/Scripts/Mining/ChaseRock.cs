using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseRock : MonoBehaviour
{
    public float damage;
    public GameObject rockExplosionParticleEffect;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            other.GetComponent<MiningPlayer>().TakeDamage(damage);
        }
        if (other.tag =="Wall")
        {
            other.gameObject.SetActive(false);
            GameObject.Instantiate(rockExplosionParticleEffect, other.transform.position, Quaternion.Euler(-90, 0, 0));
            Debug.Log("Rock hits wall");
        }
        if (other.tag == "Rock")
        {
            other.gameObject.SetActive(false);
            GameObject.Instantiate(rockExplosionParticleEffect, other.transform.position, Quaternion.Euler(-90, 0, 0));
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
