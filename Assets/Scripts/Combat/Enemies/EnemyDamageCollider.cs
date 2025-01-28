using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageCollider : MonoBehaviour
{
    public float damage;
    public Element myElement;
    public bool isMysticalDamage;
    public string myTeam;
    protected virtual void OnTriggerEnter(Collider collision)
    {
            if (collision.gameObject.tag == "Player")
            {

                collision.gameObject.GetComponent<CombatPlayerMovement>().TakeDamage(damage, 0, myElement, 0, this.gameObject, isMysticalDamage);

            }
            else if (collision.gameObject.tag == "PlayerFamiliar")
            {

                collision.gameObject.GetComponent<CombatCoopFamiliar>().TakeDamage(damage, 0, myElement, 0, this.gameObject, isMysticalDamage);

            }
            else if (collision.gameObject.tag == "Familiar")
            {

                collision.gameObject.GetComponent<CombatFamiliar>().TakeDamage(damage, 0, myElement, 0, this.gameObject);

            }
        else if (collision.tag == "Enemy")
        {
            if (collision.gameObject.TryGetComponent<TeamUser>(out TeamUser t_))
            {
                if (t_.myTeam == myTeam)
                    return;

            }
            collision.gameObject.GetComponent<BasicEnemy>().ApplyDamage(damage, 0, myElement, 0, this.gameObject, "", isMysticalDamage);
        }
        else if (collision.tag == "Follower")
        {
            if (collision.gameObject.TryGetComponent<TeamUser>(out TeamUser t_))
            {
                if (t_.myTeam == myTeam)
                    return;

            }
            collision.gameObject.GetComponent<BasicFollower>().TakeDamage(damage, 0, myElement, 0, this.gameObject);
        }
    }
}
