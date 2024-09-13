using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosion : MonoBehaviour
{
    public float range;
    public bool ignorePlayer;
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
                if(!ignorePlayer)
                hitCollider.gameObject.GetComponent<CombatPlayerMovement>().TakeDamage(damage, 0, myElement, 0, this.gameObject, isMysticalDamage);
            }
            else if (hitCollider.tag == "Familiar")
            {
                if (!ignorePlayer)
                    hitCollider.gameObject.GetComponent<CombatFamiliar>().TakeDamage(damage, 0, myElement, 0, this.gameObject);
            }
            else if (hitCollider.tag == "Enemy")
            {
                if (hitCollider.gameObject.TryGetComponent<TeamUser>(out TeamUser t_))
                {
                    if (t_.myTeam == myTeam)
                        return;

                }
                hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(damage, 0, myElement, 0, this.gameObject);
            }
            else if (hitCollider.tag == "Follower")
            {
                if (hitCollider.gameObject.TryGetComponent<TeamUser>(out TeamUser t_))
                {
                    if (t_.myTeam == myTeam)
                        return;

                }
                hitCollider.gameObject.GetComponent<BasicFollower>().TakeDamage(damage, 0, myElement, 0, this.gameObject);
            }
        }
    }
}
