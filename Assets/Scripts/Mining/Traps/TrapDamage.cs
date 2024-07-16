using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    public float damage;
    public bool canHitEnemies;
    public bool canDestroyEverything;
    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.tag=="Player")
        {
            gameObject.GetComponent<MiningPlayer>().TakeDamage(damage);
        }
        if (canHitEnemies)
        {
            if (gameObject.tag == "Enemy")
            {
                //gameObject.GetComponent<BasicMiningEnemy>().TakeDamage(damage);
            }
        }
        if(canDestroyEverything)
        {
            if (other.gameObject.tag != "Player"&& other.gameObject.tag != "Ground")
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}
