using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class HideEnemyHat : MonoBehaviour
{
    [SerializeField] MMF_Player AttackEffect;
    [SerializeField] float damage;
    [SerializeField] AudioClip attackAudio;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            other.GetComponent<MiningPlayer>().TakeDamage(damage);
            AttackEffect.PlayFeedbacks();
            if (attackAudio)
                MMSoundManager.Instance.PlaySound(attackAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
            false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
            1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
        }
    }
}
