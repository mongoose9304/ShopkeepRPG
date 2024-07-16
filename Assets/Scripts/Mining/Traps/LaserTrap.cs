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
    }
    private void Fire()
    {
        damageCollider.SetActive(true);
        foreach (ParticleSystem par in fireBurstEffect)
        {
            par.Play();
        }
        activeDamageTimer = 0.5f;
      //  GameObject.Instantiate(fireEffect, transform.position, transform.rotation);
    }
}
