using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayerActions : MonoBehaviour
{
    
    [Header("BasicMelee")]
    [SerializeField] private float BasicMeleeCooldownMax;
    private float BasicMeleeCooldown = 0.0f;
    [SerializeField] private GameObject BasicMeleePivotObject;
    [SerializeField] BasicMeleeObject meleeObject;
    [Header("BasicRanged")]
    [SerializeField] private float fireRate;
    private float currentFireRate = 0.0f;
    [SerializeField] private GameObject rangedProjectile;
    [SerializeField] Transform spawnPosition;

    private void Update()
    {
        if(Input.GetButton("Fire1"))
        {
           
            BasicMelee();
          
        }
        else if(Input.GetButton("Fire3"))
        {
            BasicRanged();
        }



        if(Input.GetButtonUp("Fire1"))
        {
            meleeObject.ReleaseMeleeButton();
        }
        Cooldowns();
    }

    private void Cooldowns()
    {
        BasicMeleeCooldown -= Time.deltaTime;
        currentFireRate -= Time.deltaTime;
    }

    private void BasicMelee()
    {
        if(meleeObject.TryToAttack())
        {
            BasicMeleeCooldown =BasicMeleeCooldownMax;
        }
    }
    private void BasicRanged()
    {
        if(currentFireRate<=0)
        {
            Instantiate(rangedProjectile, spawnPosition.position, spawnPosition.rotation);
            currentFireRate = fireRate;
        }
    }
   
}
