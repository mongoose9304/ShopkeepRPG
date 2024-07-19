using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float lifetime;
    float currentLifetime;
    [SerializeField] GameObject rockExplosionParticleEffect;
    [SerializeField] float damageToPlayer;
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
        else if (other.tag == "Rock")
        {
            GameObject.Instantiate(rockExplosionParticleEffect, other.transform.position,Quaternion.Euler(-90,0,0));
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Enemy")
        {
          
            if (other.gameObject.TryGetComponent(out BasicMiningEnemy enemy))
            {
                Debug.Log("RobHitEnemy");
                enemy.TakeDamage();
            }
        }
        else if (other.tag == "Player")
        {

            if (other.gameObject.TryGetComponent(out MiningPlayer player))
            {
                Debug.Log("RobHitEnemy");
                player.TakeDamage(damageToPlayer);
            }
        }
    }
}
