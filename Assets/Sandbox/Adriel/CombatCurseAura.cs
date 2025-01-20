using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CombatCurseAura : MonoBehaviour
{
    float attackInterval;
    float damage;
    float radius;
    float attackTimer;

    public float attackIntervalBonus;
    public float damageBonus;
    public float radiusBonus;
    public CombatCurseAura instance;

    // Update is called once per frame
    void Update(){
        UpdateTimer();
    }

    void UpdateTimer() {
        attackTimer += Time.deltaTime;
        if(attackTimer > attackInterval) {
            attackTimer = 0;
            Attack();
        }
    }

    void ResetTimer() {
        attackTimer = 0;
    }

    void Attack() {
        Vector3 parentTransform = transform.parent.position;
        Debug.Log(parentTransform);

    }
}
