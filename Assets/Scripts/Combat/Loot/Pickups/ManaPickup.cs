using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPickup : MonoBehaviour
{
    public float manaAmount;
    public void EnablePickUp(float manaAmount_)
    {
        manaAmount = manaAmount_;
        gameObject.SetActive(true);
    }
    private void OnPickUp()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<CombatPlayerMovement>().ManaPickup(manaAmount);
            OnPickUp();
        }
    }
}
