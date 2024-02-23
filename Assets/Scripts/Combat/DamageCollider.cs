using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    BasicEnemy basicEnemyRef;
    public float damage;
    public float hitStun;
    public Element element; 
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Enemy")
        {
            Debug.Log("RobHitEnemy");
            if(other.gameObject.TryGetComponent<BasicEnemy>(out basicEnemyRef))
            {
                basicEnemyRef.ApplyDamage(damage, hitStun, element) ;
            }
        }
    }
}
