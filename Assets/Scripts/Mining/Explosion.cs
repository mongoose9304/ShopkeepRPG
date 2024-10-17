using MoreMountains.Tools;
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
    [Tooltip("REFERENCE to the audio when destroying a Rock")]
    [SerializeField] AudioClip[] rockExplosionAudios;
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
            MMSoundManager.Instance.PlaySound(rockExplosionAudios[Random.Range(0,rockExplosionAudios.Length)], MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
      false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
      1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
        }
        else if (other.tag == "Enemy")
        {
          
            if (other.gameObject.TryGetComponent(out BasicMiningEnemy enemy))
            {
                enemy.TakeDamage();
            }
        }
        else if (other.tag == "Player")
        {

            if (other.gameObject.TryGetComponent(out MiningPlayer player))
            {
                player.TakeDamage(damageToPlayer);
            }
        }
    }
}
