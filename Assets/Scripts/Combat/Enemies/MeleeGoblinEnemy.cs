using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeGoblinEnemy : BasicEnemy
{
    public Animator anim;
    [Tooltip("REFERENCE to the particle effect that plays when I use my basic attack")]
    public ParticleSystem basicAttackSystem;
    public override void Attack()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > attackDistance)
        {
            return;
        }
        MeleeAttack();

    }
    public void MeleeAttack()
    {
        anim.SetTrigger("basicAttack");
        currentAttackCooldown = maxAttackCooldown;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * 1, 1.5f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Player")
            {
                hitCollider.gameObject.GetComponent<CombatPlayerMovement>().TakeDamage(damage, 0, myElement, 0, this.gameObject, isMysticalDamage);
            }
            else if (hitCollider.tag == "Familiar")
            {
                hitCollider.gameObject.GetComponent<CombatFamiliar>().TakeDamage(damage, 0, myElement, 0, this.gameObject, isMysticalDamage);
            }
            else if (hitCollider.tag == "PlayerFamiliar")
            {
                hitCollider.gameObject.GetComponent<CombatCoopFamiliar>().TakeDamage(damage, 0, myElement, 0, this.gameObject, isMysticalDamage);
            }
            else if (hitCollider.tag == "Enemy")
            {
                if (CheckTeam(hitCollider.gameObject))
                    hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(damage, 0, myElement, 0, this.gameObject);

            }
            else if (hitCollider.tag == "Follower")
            {
                if (CheckTeam(hitCollider.gameObject))
                    hitCollider.gameObject.GetComponent<BasicFollower>().TakeDamage(damage, 0, myElement, 0, this.gameObject);

            }
        }
        basicAttackSystem.Play();
    }
}
