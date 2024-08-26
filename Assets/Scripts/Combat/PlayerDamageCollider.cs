using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageCollider : MonoBehaviour
{
    BasicEnemy basicEnemyRef;
    public float damage;
    public float hitStun;
    public float knockBack;
    public Element element;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Enemy")
        {
           
            if(other.gameObject.TryGetComponent<BasicEnemy>(out basicEnemyRef))
            {
                basicEnemyRef.ApplyDamage(damage, hitStun, element,knockBack,this.gameObject) ;
              
            }
        }
    }
}
