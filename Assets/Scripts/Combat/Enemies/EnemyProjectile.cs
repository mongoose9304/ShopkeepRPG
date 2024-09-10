using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage;
    public Element myElement;
    public bool isMysticalDamage;
    public GameObject projectileExplosionObject;
    public string myTeam;
    private void Start()
    {
        if (projectileExplosionObject)
        {
            projectileExplosionObject = Instantiate(projectileExplosionObject, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            projectileExplosionObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Wall"|| other.tag == "Ground")
        {
            gameObject.SetActive(false);
            if (projectileExplosionObject)
            {
                CreateExplosion();
            }
        }
        else if (other.tag == "Player")
        {
            gameObject.SetActive(false);
            if (projectileExplosionObject)
            {
                CreateExplosion();
            }
            else
            {
            other.gameObject.GetComponent<CombatPlayerMovement>().TakeDamage(damage, 0, myElement, 0, this.gameObject,isMysticalDamage);
            }
        }
        else if (other.tag == "Familiar")
        {
            gameObject.SetActive(false);
            if (projectileExplosionObject)
            {
                CreateExplosion();
            }
            else
            {
            other.gameObject.GetComponent<CombatFamiliar>().TakeDamage(damage, 0, myElement, 0, this.gameObject);
            }
        }
        else if (other.tag == "Enemy")
        {
            if(other.gameObject.TryGetComponent<TeamUser>(out TeamUser t_))
            {
                if (t_.myTeam == myTeam)
                    return;

            }
            gameObject.SetActive(false);
            if (projectileExplosionObject)
            {
                CreateExplosion();
            }
            else
            {
                other.gameObject.GetComponent<BasicEnemy>().ApplyDamage(damage, 0, myElement, 0, this.gameObject);
            }
        }
    }
    public void CreateExplosion()
    {
        projectileExplosionObject.GetComponent<ProjectileExplosion>().damage = damage;
        projectileExplosionObject.GetComponent<ProjectileExplosion>().isMysticalDamage = isMysticalDamage;
        projectileExplosionObject.GetComponent<ProjectileExplosion>().myElement = myElement;
        projectileExplosionObject.transform.position = transform.position;
        projectileExplosionObject.GetComponent<ProjectileExplosion>().myTeam = myTeam;
        projectileExplosionObject.SetActive(true);
    }
}
