using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The logic for the explosion spawned by bombs
/// </summary>
public class Explosion : MonoBehaviour
{
    [Tooltip("Length of time before the object disappears")]
    [SerializeField] float lifetime;
    float currentLifetime;
    [Tooltip("The damage this will deal to players who touch it")]
    [SerializeField] float damageToPlayer;
    [Tooltip("REFERENCE to the effect when destroying rocks")]
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
    /// <summary>
    /// The explosion will destroy rocks, explode other bombs and hurt players or enemies 
    /// </summary>
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
