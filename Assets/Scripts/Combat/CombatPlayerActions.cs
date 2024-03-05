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

    private void Update()
    {
        if(Input.GetButton("Fire1"))
        {
           
            BasicMelee();
          
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
    }

    private void BasicMelee()
    {
        if(meleeObject.TryToAttack())
        {
            BasicMeleeCooldown =BasicMeleeCooldownMax;
        }
    }
   
}
