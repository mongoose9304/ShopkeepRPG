using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class PoolCueTrap : BasicTrap
{
    public GameObject poolcue;
    public GameObject target;
    public GameObject aimTarget;
  [SerializeField]  private bool isAiming;
  [SerializeField]  public bool isAttacking;
    public MMF_Player poolStabEffect;
    public float maxAttackCooldown;
    float currentAttackCooldown;
    protected override void Start()
    {
        base.Start();
        currentAttackCooldown = maxAttackCooldown;
    }
    private void Update()
    {
        if(isAiming&&target&&!isAttacking)
        {
            poolcue.transform.LookAt(target.transform);
            //poolcue.transform.eulerAngles = new Vector3(0, poolcue.transform.eulerAngles.y, 0);
            currentAttackCooldown -= Time.deltaTime;
            if(currentAttackCooldown<=0)
            {
                currentAttackCooldown = maxAttackCooldown;
                isAttacking = true;
                aimTarget.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
                aimTarget.transform.rotation = poolcue.transform.rotation;
                poolStabEffect.PlayFeedbacks();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player"|| other.tag == "PlayerFamiliar")
        {
            target = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "PlayerFamiliar")
        {
          if(target)
            {
                if(target==other.gameObject)
                {
                    target = null;
                }
            }
        }
    }
    public void EndAttack()
    {
        isAttacking = false;
    }
}
