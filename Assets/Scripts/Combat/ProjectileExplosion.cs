using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosion : MonoBehaviour
{
    public float range;
    public float damage;
    public Element myElement;
    public bool isMysticalDamage;
    public string myTeam;

    private void OnEnable()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Player")
            {
                hitCollider.gameObject.GetComponent<CombatPlayerMovement>().TakeDamage(damage, 0, myElement, 0, this.gameObject, isMysticalDamage);
            }
            if (hitCollider.tag == "Familiar")
            {
                hitCollider.gameObject.GetComponent<CombatFamiliar>().TakeDamage(damage, 0, myElement, 0, this.gameObject);
            }
            if (hitCollider.tag == "Enemy")
            {
                if (hitCollider.gameObject.TryGetComponent<TeamUser>(out TeamUser t_))
                {
                    if (t_.myTeam == myTeam)
                        return;

                }
                hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(damage, 0, myElement, 0, this.gameObject);
            }
        }
    }
}
