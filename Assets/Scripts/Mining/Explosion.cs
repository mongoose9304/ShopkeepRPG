using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float lifetime;
    float currentLifetime;
    [SerializeField] GameObject rockExplosionParticleEffect;
    private void Update()
    {
        currentLifetime -= Time.deltaTime;
        if(currentLifetime<=0)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        currentLifetime = lifetime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Bomb")
        {
            other.GetComponent<Bomb>().Explode();
        }
        if (other.tag == "Rock")
        {
            GameObject.Instantiate(rockExplosionParticleEffect, other.transform.position,Quaternion.Euler(-90,0,0));
            Destroy(other.gameObject);
        }
    }
}
