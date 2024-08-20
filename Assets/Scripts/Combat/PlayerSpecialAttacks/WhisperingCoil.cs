using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhisperingCoil : PlayerSpecialAttack
{
    public GameObject particleEffect;
    public Collider myCollider;
    public float duration;
    public float pullSpeed;
    public float minDistance;
    float currentTime;
    // Start is called before the first frame update
    public override void OnPress(GameObject obj_)
    {
        Player = obj_;
        currentTime = duration;
        particleEffect.SetActive(true);
        myCollider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime>0)
        {
            currentTime -= Time.deltaTime;

            if(currentTime<=0)
            {
                myCollider.enabled = false;
                particleEffect.SetActive(false);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Enemy")
        {

            if(other.TryGetComponent<BasicEnemy>(out BasicEnemy enemy_))
            {
                if(Vector3.Distance(transform.position,other.transform.position)>minDistance)
                enemy_.AffectMovement(other.transform.position+(transform.position-other.transform.position).normalized*pullSpeed*Time.deltaTime);
            }
        }
    }
}
