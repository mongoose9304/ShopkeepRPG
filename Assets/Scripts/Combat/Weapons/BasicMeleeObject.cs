using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeObject : MonoBehaviour
{
    [SerializeField] private float swingSpeed;
    float swingSpeedModified;
    [SerializeField] private Vector3 startRotaton;
    [SerializeField] private float attackDurationMax;
    float attackDurationMaxModified;
    private float attackDurationCurrent;
    public PlayerDamageCollider damageCollider;
    [SerializeField] private float hitStun;
    [SerializeField] private GameObject weaponObject;
    
     private bool queuedAttack;
     private bool rightAttackDirection;

    private void Update()
    {
        if(weaponObject.activeInHierarchy)
        {

           
            if(rightAttackDirection)
            transform.Rotate(0.0f, swingSpeedModified*Time.deltaTime, 0.0f, Space.Self);
            else
                transform.Rotate(0.0f, -swingSpeedModified * Time.deltaTime, 0.0f, Space.Self);

            attackDurationCurrent -= Time.deltaTime;
            if (attackDurationCurrent<=0)
                EndAttack();

            //queuedAttack = false;
        }
    }


    public void StartAttack()
    {
        transform.localRotation=Quaternion.Euler(startRotaton.x, startRotaton.y, startRotaton.z);
        weaponObject.SetActive(true);
        attackDurationCurrent = attackDurationMaxModified;
    }
    public void EndAttack()
    {
        if(queuedAttack)
        {
            rightAttackDirection = !rightAttackDirection;
            attackDurationCurrent = attackDurationMaxModified;
            queuedAttack=false;
            weaponObject.GetComponent<Collider>().enabled = false;
            weaponObject.GetComponent<Collider>().enabled = true;
        }
        else
        weaponObject.SetActive(false);
    }
    public void ForceEndAttack()
    {
        attackDurationCurrent = 0;
        weaponObject.SetActive(false);
    }
    public bool TryToAttack()
    {
        if(weaponObject.gameObject.activeInHierarchy)
        {
            queuedAttack = true;
            return false;
        }
        else
        {
            StartAttack();
            rightAttackDirection = true;
            return true;
        }
    }
    public void ReleaseMeleeButton()
    {
        queuedAttack = false;
    }
    public void SetDamage(float damage_,Element damageType_,float swingSpeed_=1,float lifeSteal=0)
    {
        damageCollider.damage = damage_;
        damageCollider.element = damageType_;
        damageCollider.lifeSteal = lifeSteal;
        swingSpeedModified = swingSpeed * swingSpeed_;
        attackDurationMaxModified = attackDurationMax / swingSpeed_;
    }
  
   
}
