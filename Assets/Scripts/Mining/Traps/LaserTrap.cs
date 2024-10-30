using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : BasicMiningTrap
{
    [SerializeField] GameObject chargeEffect;
    [SerializeField] ParticleSystem[] fireBurstEffect;
    [SerializeField] GameObject fireEffect;
    [SerializeField] GameObject damageCollider;
    [SerializeField] float maxChargeTime;
    [SerializeField] float currentChargeTime;
    [SerializeField] float maxFireDelayTime;
    [SerializeField] float currentFireDelayTime;
    [SerializeField] AudioSource laserChargeAudio;
    [SerializeField] AudioClip attackAudio;
    float activeDamageTimer;
    bool isFiring;
    private void Start()
    {
        isFiring = false;
        currentChargeTime = maxChargeTime;
        currentFireDelayTime = maxFireDelayTime;

    }
    private void Update()
    {
        if(isFiring)
        {
            currentFireDelayTime -= Time.deltaTime;
            if(currentFireDelayTime<=0)
            {
                currentFireDelayTime = maxFireDelayTime;
                isFiring = false;
                Fire();
            }
        }
        if(activeDamageTimer>0)
        {
            activeDamageTimer -= Time.deltaTime;
            if(activeDamageTimer<=0)
            {
                damageCollider.SetActive(false);

            }
        }
       
    }

    public override void PlayerInRange()
    {
        if(!isFiring)
        {
            chargeEffect.SetActive(true);
            currentChargeTime -= Time.deltaTime;
            if(!laserChargeAudio.isPlaying)
            {
                laserChargeAudio.Play();
            }
            if (currentChargeTime <= 0)
            {
                isFiring = true;
                chargeEffect.SetActive(false);
                currentChargeTime = maxChargeTime;
            }
        }
    }
    public override void PlayerLeavesRange()
    {
            chargeEffect.SetActive(false);
        if (!isFiring)
        {
            currentChargeTime = maxChargeTime;
            laserChargeAudio.Stop();
        }
    }
    private void Fire()
    {
        damageCollider.SetActive(true);
        foreach (ParticleSystem par in fireBurstEffect)
        {
            par.Play();
        }
        activeDamageTimer = 0.5f;
        laserChargeAudio.Stop();
        if (attackAudio)
            MMSoundManager.Instance.PlaySound(attackAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
        false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
        1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
        //  GameObject.Instantiate(fireEffect, transform.position, transform.rotation);
    }
}
