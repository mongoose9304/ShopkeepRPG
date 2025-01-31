 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissile : PlayerDamageCollider
{
    HomingAttack hAttack;
    public bool canRicochet = false;
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
                basicEnemyRef.ApplyDamage(damage, hitStun, element, knockBack, this.gameObject, "Ranged",isMysticalDamage);
                if (lifeSteal > 0)
                {
                    CombatPlayerManager.instance.PlayerLifeSteal(damage * lifeSteal);
                }

                if (canRicochet) {
                    Ricochet();
                    return;
                }
                if (canPierceEnemies)
                    hAttack.target = null;

                else
                    canRicochet = true;
                    hAttack.homingType = HomingAttack.HomingType.smooth;
                    gameObject.SetActive(false);
            }
        }
    }

    void Ricochet() {
        //Setting the homing to sharp 
        hAttack.homingType = HomingAttack.HomingType.sharp;
        //Check radius
        float radius = 10.0f;
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, radius, Vector3.up);
        foreach(RaycastHit h in hit) {
            if(h.collider.gameObject.tag == "Enemy") {
                if(hAttack.target == h.collider.gameObject) {
                    continue;
                }
                hAttack.target = h.collider.gameObject.transform;
                Debug.Log(string.Format("Found new target: {0}", h.collider.name));
                Debug.DrawLine(transform.position, h.collider.gameObject.transform.position, Color.green, 2.0f);
                canRicochet = false;
                return;
            }
        }
    }
}
