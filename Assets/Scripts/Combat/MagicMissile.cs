using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissile : PlayerDamageCollider
{
    HomingAttack hAttack;
    private void Awake()
    {
        hAttack = GetComponent<HomingAttack>();
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            if (other.gameObject.TryGetComponent<BasicEnemy>(out basicEnemyRef))
            {
                basicEnemyRef.ApplyDamage(damage, hitStun, element, knockBack, this.gameObject, "Ranged");
                if (lifeSteal > 0)
                {
                    CombatPlayerManager.instance.HealPlayer(damage * lifeSteal);
                }
                if (canPierceEnemies)
                    hAttack.target = null;
                else
                    gameObject.SetActive(false);
            }
        }
    }
}
