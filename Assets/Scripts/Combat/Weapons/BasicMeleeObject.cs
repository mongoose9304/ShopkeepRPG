using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
public class BasicMeleeObject : MonoBehaviour
{
    [SerializeField] private float swingSpeed;
    [SerializeField] float xSwingRotation;
    float swingSpeedModified;
    [SerializeField] private Vector3 startRotaton;
    [SerializeField] private Vector3 startRotatonB;
    [SerializeField] private Vector3 startRotatonC;
    [SerializeField] private float attackDurationMax;
    float attackDurationMaxModified;
    private float attackDurationCurrent;
    public PlayerDamageCollider damageCollider;
    [SerializeField] private float hitStun;
    [SerializeField] private GameObject weaponObject;
    
     private bool queuedAttack;
     private bool rightAttackDirection;
     private int comboCount;
     [SerializeField] private int comboCountMax;
    [SerializeField] MMMiniObjectPooler specialAttackPool;
    [SerializeField] Transform specialAttackSpawn;
    [SerializeField] AudioClip[] audioClips;

    private void Update()
    {
        if(weaponObject.activeInHierarchy)
        {
            if (comboCount >= comboCountMax)
            {
                transform.Rotate(0, -swingSpeedModified * Time.deltaTime, 0.0f, Space.Self);
            }
            else
            {

                if (rightAttackDirection)
                    transform.Rotate(0, swingSpeedModified * Time.deltaTime, 0.0f, Space.Self);
                else
                    transform.Rotate(0, -swingSpeedModified * Time.deltaTime, 0.0f, Space.Self);
            }
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
        comboCount = 0;
        MMSoundManager.Instance.PlaySound(audioClips[1], MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
          false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.9f, 1.1f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
          1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    public void EndAttack()
    {
        if(queuedAttack)
        {
            comboCount += 1;
             if (comboCount > comboCountMax)
             {
                 weaponObject.SetActive(false);
                 queuedAttack = false;
              GameObject obj=specialAttackPool.GetPooledGameObject();
                obj.transform.position = specialAttackSpawn.position;
                obj.transform.rotation = specialAttackSpawn.rotation;
                obj.GetComponent<PlayerDamageCollider>().damage = damageCollider.damage * 2;
                obj.SetActive(true);
                 return;
             }
            
            if (comboCount >= comboCountMax)
            {
                transform.localRotation = Quaternion.Euler(startRotatonC.x, startRotatonC.y, startRotatonC.z);
                MMSoundManager.Instance.PlaySound(audioClips[2], MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
           false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.9f, 1.1f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
           1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
            }
            else
            {
                if (rightAttackDirection)
                {
                    rightAttackDirection = false;
                    transform.localRotation = Quaternion.Euler(startRotatonB.x, startRotatonB.y, startRotatonB.z);
                    MMSoundManager.Instance.PlaySound(audioClips[1], MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
           false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.9f, 1.1f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
           1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
                }
                else
                {
                    rightAttackDirection = true;
                    MMSoundManager.Instance.PlaySound(audioClips[0], MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
            false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.9f, 1.1f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
            1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
                    transform.localRotation = Quaternion.Euler(startRotatonB.x, startRotatonB.y, startRotatonB.z);
                }
            }
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
