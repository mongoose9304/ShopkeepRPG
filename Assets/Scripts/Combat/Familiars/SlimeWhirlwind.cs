using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeWhirlwind : MonoBehaviour
{
    public SphereCollider collider;
    public float damage;
    public float pullSpeed;
    public Element element;
    public float damageInterval;
    public float minDistance;
    float currentdamageInterval;
    public AudioSource audio;
    private void OnEnable()
    {
        currentdamageInterval = 0;
        audio.Play();
    }
    private void Update()
    {
        currentdamageInterval -= Time.deltaTime;
        if(currentdamageInterval<=0)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position,collider.radius );
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.tag == "Enemy")
                {
                    hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(damage, 0f, element, 0, this.gameObject);
                }
            }
                currentdamageInterval = damageInterval;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {

            if (other.TryGetComponent<BasicEnemy>(out BasicEnemy enemy_))
            {
                if (Vector3.Distance(transform.position, other.transform.position) > minDistance)
                    enemy_.AffectMovement(other.transform.position + (transform.position - other.transform.position).normalized * pullSpeed * Time.deltaTime);
            }
        }
    }
}
