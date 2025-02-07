using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FamiliarProjectile : MonoBehaviour
{
    public float damage;
    public Element myElement;
    public bool isMysticalDamage;
    public GameObject projectileExplosionObject;
    public string myTeam;

    //Adriel stuff for ricocheting
    public bool canRicochet = false;
    public int ricochetCount = 0;
    private GameObject ricochetTarget;
    [HideInInspector] public UnityEvent<GameObject, GameObject> RichochetTravelEquation; //Gameobject 1 is for the projectile, Gameobject 2 is the target
     

    private void Start()
    {
        if (projectileExplosionObject)
        {
            projectileExplosionObject = Instantiate(projectileExplosionObject, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            projectileExplosionObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall" || other.tag == "Ground")
        {
            if (canRicochet) { Ricochet();} 
            else {
                gameObject.SetActive(false);
            }

            if (projectileExplosionObject)
            {
                CreateExplosion();
            }
        }
        else if (other.tag == "Player")
        {
            if (canRicochet) { Ricochet(); } else {
                gameObject.SetActive(false);
            }
            if (projectileExplosionObject)
            {
                CreateExplosion();
            }
        }
        else if (other.tag == "Enemy")
        {
            if (canRicochet) { Ricochet(); } 
            else {
                gameObject.SetActive(false);
            }

            if (projectileExplosionObject)
            {
                CreateExplosion();
            }
            else
            {
                other.gameObject.GetComponent<BasicEnemy>().ApplyDamage(damage, 0, myElement, 0, this.gameObject,"",isMysticalDamage);
            }
        }
        else if (other.tag == "Follower")
        {
            if (other.gameObject.TryGetComponent<TeamUser>(out TeamUser t_))
            {
                if (t_.myTeam == myTeam)
                    return;

            }
            if (canRicochet) { Ricochet(); } else {
                gameObject.SetActive(false);
            }
            if (projectileExplosionObject)
            {
                CreateExplosion();
            }
            else
            {
                other.gameObject.GetComponent<BasicFollower>().TakeDamage(damage, 0, myElement, 0, this.gameObject);
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

    public void Ricochet() {
        //if(RichochetTravelEquation.GetPersistentEventCount()  <= 0) { return;  }
        if(ricochetCount <= 0) {
            gameObject.SetActive(false);
            canRicochet = false;
        }

        ricochetCount -= 1;
        //Check radius
        float radius = 5.0f;
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, radius, transform.forward);
        foreach (RaycastHit h in hit) {
            if (h.collider.gameObject.tag == "Enemy") {
                if(h.collider.gameObject == ricochetTarget) { continue; }
                ricochetTarget = h.collider.gameObject;
                RichochetTravelEquation.Invoke(gameObject, h.collider.gameObject);
                Debug.Log(string.Format("Found new target: {0}", h.collider.name));

                if (projectileExplosionObject) {
                    CreateExplosion();
                } else {
                    h.collider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(damage, 0, myElement, 0, this.gameObject, "", isMysticalDamage);
                }

                Debug.DrawLine(transform.position, h.collider.gameObject.transform.position, Color.green, 2.0f);
                return;
            }
        }

        //If you can't find a target just null
        RichochetTravelEquation.Invoke(gameObject, null);
    }
}
