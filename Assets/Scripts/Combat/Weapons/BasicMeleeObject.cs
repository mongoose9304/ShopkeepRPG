using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeObject : MonoBehaviour
{
    [SerializeField] private float swingSpeed;
    [SerializeField] private Quaternion startRotaton;
    [SerializeField] private Quaternion startRotatonB;
    [SerializeField] private float attackDurationMax;
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
            transform.Rotate(0.0f, swingSpeed*Time.deltaTime, 0.0f, Space.Self);
            else
                transform.Rotate(0.0f, -swingSpeed * Time.deltaTime, 0.0f, Space.Self);

            attackDurationCurrent -= Time.deltaTime;
            if (attackDurationCurrent<=0)
                EndAttack();

            //queuedAttack = false;
        }
    }


    public void StartAttack()
    {
        transform.localRotation = startRotaton;
        weaponObject.SetActive(true);
        attackDurationCurrent = attackDurationMax;
    }
    public void EndAttack()
    {
        if(queuedAttack)
        {
            rightAttackDirection = !rightAttackDirection;
            attackDurationCurrent = attackDurationMax;
            queuedAttack=false;
            weaponObject.GetComponent<CapsuleCollider>().enabled = false;
            weaponObject.GetComponent<CapsuleCollider>().enabled = true;
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
    public void SetDamage(float damage_,Element damageType_)
    {
        damageCollider.damage = damage_;
        damageCollider.element = damageType_;
    }
  
   
}
