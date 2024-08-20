using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage;
    public Element myElement;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Wall")
        {
            gameObject.SetActive(false);
        }
        else if (other.tag == "Player")
        {
            other.gameObject.GetComponent<CombatPlayerMovement>().TakeDamage(damage, 0, myElement, 0, this.gameObject);
            gameObject.SetActive(false);
        }
        else if (other.tag == "Familiar")
        {
            other.gameObject.GetComponent<CombatFamiliar>().TakeDamage(damage, 0, myElement, 0, this.gameObject);
            gameObject.SetActive(false);
        }
    }
}
