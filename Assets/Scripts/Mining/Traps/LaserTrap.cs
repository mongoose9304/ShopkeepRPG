using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : BasicMiningTrap
{
    [SerializeField] GameObject chargeEffect;
    [SerializeField] GameObject fireEffect;
    [SerializeField] float maxChargeTime;
    [SerializeField] float currentChargeTime;
    [SerializeField] float maxFireDelayTime;
    [SerializeField] float currentFireDelayTime;
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
        GameObject.Instantiate(fireEffect, transform.position, transform.rotation);
    }
}
