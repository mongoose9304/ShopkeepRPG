using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageCollider : MonoBehaviour
{
    protected BasicEnemy basicEnemyRef;
    public float damage;
    public float hitStun;
    public float knockBack;
    public float lifeSteal;
    public Element element;
    public bool canPierceEnemies;
    //Currently only used in tutorial
    public string damageTag;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Enemy")
        {
           
            if(other.gameObject.TryGetComponent<BasicEnemy>(out basicEnemyRef))
            {
                basicEnemyRef.ApplyDamage(damage, hitStun, element,knockBack,this.gameObject,damageTag) ;
              if(lifeSteal>0)
                {
                    CombatPlayerManager.instance.HealPlayer(damage * lifeSteal);
                }
            }
        }
    }
}
