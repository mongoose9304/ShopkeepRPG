using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialEnemy : BasicEnemy
{
    public string correctString;
    public float damagePerHit;
    public UnityEvent deathEvent;
    protected override void Update()
    {
        
    }
    protected override void Start()
    {
        currentHealth = 100;
        EnemyManager.instance.AddEnemyToEnemyList(gameObject);
    }
    protected override void OnEnable()
    {

    }
    public override void ApplyDamage(float damage_, float hitstun_, Element element_, float knockBack_ = 0, GameObject knockBackObject = null, string playerAttackType = "")
    {
        if(playerAttackType==correctString)
        {
            currentHealth -= damagePerHit;
            if (hitEffects)
                hitEffects.PlayFeedbacks();
        }
        if (currentHealth <= 0)
        {
            Death();
            return;
        }
    }
    public override void Death()
    {
        deathEvent.Invoke();
        gameObject.SetActive(false);
    }
}
