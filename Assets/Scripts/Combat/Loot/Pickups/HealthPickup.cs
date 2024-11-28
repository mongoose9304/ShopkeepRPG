
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healAmount;
    public void EnablePickUp(float healAmount_)
    {
        healAmount = healAmount_;
        gameObject.SetActive(true);
    }
   private void OnPickUp()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            other.GetComponent<CombatPlayerMovement>().HealthPickup(healAmount);
            OnPickUp();
        }
        if (other.tag == "PlayerFamiliar")
        {
            other.GetComponent<CombatCoopFamiliar>().combatPlayerMovement.HealthPickup(healAmount);
            OnPickUp();
        }
    }
}
