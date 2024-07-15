using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : BasicMiningTrap
{
    [SerializeField] GameObject chargeEffect;
    [SerializeField] GameObject fireBurstEffect;
    [SerializeField] GameObject fireEffect;
    [SerializeField] float maxChargeTime;
    [SerializeField] float currentChargeTime;
    [SerializeField] float maxFireDelayTime;
    [SerializeField] float currentFireDelayTime;
    [SerializeField] float maxFireLengthTime;
    [SerializeField] float currentFireLengthTime;
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
        else
        {
           if(currentFireLengthTime > 0)
            {
                currentFireLengthTime -= Time.deltaTime;
                if(currentFireLengthTime<=0)
                {
                    fireBurstEffect.SetActive(false);
                }
            }
        }
    }

    public override void PlayerInRange()
    {
        if(!isFiring&&currentFireLengthTime<=0)
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
        fireBurstEffect.SetActive(true);
        currentFireLengthTime = maxFireLengthTime;
      //  GameObject.Instantiate(fireEffect, transform.position, transform.rotation);
    }
}
