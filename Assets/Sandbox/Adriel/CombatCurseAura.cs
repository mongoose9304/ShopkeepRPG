using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class CombatCurseAura : MonoBehaviour
{
    float attackInterval = 1.5f;
    float damage = 30.0f;
    float radius = 1.0f;
    float attackTimer;
    

    public float attackIntervalBonus;
    public float damageBonus;
    public float radiusBonus;
    public bool blockProjectiles = false;

    // Update is called once per frame
    void Update(){
        UpdateTimer();
    }

    void UpdateTimer() {
        attackTimer += Time.deltaTime;
        if(attackTimer >= attackInterval - attackIntervalBonus) {
            attackTimer = 0;
            Attack();
        }
    }

    void ResetTimer() {
        attackTimer = 0;
    }

    public void Init() {
        float objScale = 4.0f + ((radiusBonus / 0.6f) * 2.5f);
        transform.localScale = new Vector3(objScale, objScale, objScale);
        ResetTimer();
    }

    void Attack() {
        Vector3 parentTransform = transform.parent.position;

        //Changing the scale of the curse aura object
        //The reason why it has to be like that is from eyeballing it
        float objScale = 4.0f + ((radiusBonus / 0.6f) * 2.5f); 
        transform.localScale = new Vector3(objScale, objScale, objScale);

        //Raycasting does all the work
        Debug.DrawRay(parentTransform, Vector3.right * (radius + radiusBonus), Color.yellow, 5.0f);
        RaycastHit[] hit =  Physics.SphereCastAll(parentTransform, radius + radiusBonus, Vector3.up);

        //Debugging 
        string result = "";
        //iterating through the hits
        foreach(RaycastHit h in hit) {

            if (blockProjectiles) {
                EnemyProjectile p = h.collider.GetComponent<EnemyProjectile>();
                if(p != null) {
                    result += h.collider.gameObject.name + " \n";
                    Debug.Log("Projectile DELETED!");
                    p.gameObject.SetActive(false);
                    if (p.projectileExplosionObject) {
                        p.CreateExplosion();
                    }
                    continue;
                }
            }
            BasicEnemy enemy = h.collider.gameObject.GetComponent<BasicEnemy>();
            if(enemy == null) { continue; }
            enemy.ApplyDamage(damage + damageBonus, 0.0f, Element.Neutral);

            result += h.collider.gameObject.name + " \n";
        }

        Debug.Log(result);
    }
}
