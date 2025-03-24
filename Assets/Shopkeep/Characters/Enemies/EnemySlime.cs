using Shopkeeper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : Enemy
{
    private SpellComponent spellComponent;
    [SerializeField]
    private Spell spell;

    [SerializeField]
    CapsuleCollider trigger;

    private void Start()
    {
        spellComponent = gameObject.GetComponent<SpellComponent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        trigger.enabled = false;
    }

    private void Update()
    {
        if (Time.time - lastAttackTime > cooldown)
        {
            enemyState = EnemyState.Move;
            Move();

            //need to turn towards player before attack
            if (enemyState != EnemyState.Attack && Vector3.Distance(transform.position, target.position) < 2.5f)
            {
                Attack();
            }
        }    
    }

    protected override void Move()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    protected override void Attack() 
    {
        StartCoroutine(CastSpell());
    }

    private IEnumerator CastSpell()
    {
        enemyState = EnemyState.Attack;
        trigger.enabled = true;
        agent.isStopped = true;
        agent.updatePosition = false;
        rb.isKinematic = false;

        yield return new WaitForSeconds(0.35f);

        spellComponent.CastSpell(spell);

        yield return new WaitForSeconds(0.2f);

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        agent.Warp(transform.position); 
        agent.isStopped = false;
        agent.updatePosition = true;

        enemyState = EnemyState.Idle;
        trigger.enabled = false;
        lastAttackTime = Time.time;
    }

}
