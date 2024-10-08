using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawMeleeWeapon : BasicMeleeObject
{
    [SerializeField] protected MMMiniObjectPooler regularAttackPool;
    [SerializeField] protected Transform regularAttackSpawn;
    [SerializeField] protected Transform inverseAttackSpawn;
    [SerializeField] protected Transform UltimateAttackSpawn;
    public override void EndAttack()
    {
        if (queuedAttack)
        {
            comboCount += 1;
            if (comboCount > comboCountMax)
            {
                weaponObject.SetActive(false);
                queuedAttack = false;
                GameObject obj = specialAttackPool.GetPooledGameObject();
                obj.transform.position = specialAttackSpawn.position;
                obj.transform.rotation = specialAttackSpawn.rotation;
                obj.GetComponent<PlayerDamageCollider>().damage = damageCollider.damage * 2;
                obj.SetActive(true);

                return;
            }

            if (comboCount >= comboCountMax)
            {
                GameObject obj = regularAttackPool.GetPooledGameObject();
                obj.transform.position = UltimateAttackSpawn.position;
                obj.transform.rotation = UltimateAttackSpawn.rotation;
                obj.GetComponent<PlayerDamageCollider>().damage = damageCollider.damage;
                obj.SetActive(true);
                MMSoundManager.Instance.PlaySound(audioClips[2], MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
           false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.9f, 1.1f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
           1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
            }
            else
            {
                if (rightAttackDirection)
                {
                    rightAttackDirection = false;
                    GameObject obj = regularAttackPool.GetPooledGameObject();
                    obj.transform.position = inverseAttackSpawn.position;
                    obj.transform.rotation = inverseAttackSpawn.rotation;
                    obj.GetComponent<PlayerDamageCollider>().damage = damageCollider.damage;
                    obj.SetActive(true);
                    MMSoundManager.Instance.PlaySound(audioClips[1], MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
           false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.9f, 1.1f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
           1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
                }
                else
                {
                    rightAttackDirection = true;
                    GameObject obj = regularAttackPool.GetPooledGameObject();
                    obj.transform.position = regularAttackSpawn.position;
                    obj.transform.rotation = regularAttackSpawn.rotation;
                    obj.GetComponent<PlayerDamageCollider>().damage = damageCollider.damage;
                    obj.SetActive(true);
                    MMSoundManager.Instance.PlaySound(audioClips[0], MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
            false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.9f, 1.1f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
            1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
                }
            }
            attackDurationCurrent = attackDurationMaxModified;
            queuedAttack = false;

        }
        else
        {
            weaponObject.SetActive(false);
        }
    }
    public override void StartAttack()
    {
        weaponObject.SetActive(true);
        GameObject obj = regularAttackPool.GetPooledGameObject();
        obj.transform.position = regularAttackSpawn.position;
        obj.transform.rotation = regularAttackSpawn.rotation;
        obj.GetComponent<PlayerDamageCollider>().damage = damageCollider.damage;
        obj.SetActive(true);
        attackDurationCurrent = attackDurationMaxModified;
        comboCount = 0;
        rightAttackDirection = true;
        MMSoundManager.Instance.PlaySound(audioClips[1], MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
          false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.9f, 1.1f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
          1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    protected override void Update()
    {
        if (weaponObject.activeInHierarchy)
        {
            
            attackDurationCurrent -= Time.deltaTime;
            if (attackDurationCurrent <= 0)
                EndAttack();

            //queuedAttack = false;
        }
    }
    public override void ForceEndAttack()
    {
        attackDurationCurrent = 0;
        weaponObject.SetActive(false);
        regularAttackPool.ResetAllObjects();
    }
}
