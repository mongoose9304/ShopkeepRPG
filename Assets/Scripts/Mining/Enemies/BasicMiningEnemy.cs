using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMiningEnemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth;
    public float maxAttackCooldown;
    public float moveSpeed;
    public float attackDistance;
    public float damage;
   [SerializeField] protected LootDropper lootDropper;
    [Header("References")]
    public GameObject player;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected AudioClip deathAudio;
    [SerializeField] protected AudioClip attackAudio;
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lootDropper = GetComponent<LootDropper>();
        currentHealth = maxHealth;
    }
    

    public virtual void DetectObstacle()
    {

    }
    public virtual void DetectNoGround()
    {

    }
    public virtual void DetectGround()
    {

    }
    public virtual void TakeDamage(int damage_=1)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Death();
    }
    public virtual void Death()
    {
        lootDropper.DropItems();
        Destroy(gameObject);
        if(deathAudio)
        MMSoundManager.Instance.PlaySound(deathAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
    false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
    1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
}
