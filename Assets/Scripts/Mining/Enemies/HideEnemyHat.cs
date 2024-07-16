using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class HideEnemyHat : MonoBehaviour
{
    [SerializeField] MMF_Player AttackEffect;
    [SerializeField] float damage;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            other.GetComponent<MiningPlayer>().TakeDamage(damage);
            AttackEffect.PlayFeedbacks();
        }
    }
}
