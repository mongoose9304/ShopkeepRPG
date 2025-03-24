using Shopkeeper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : Enemy
{
    private SpellComponent spellComponent;
    [SerializeField]
    private Spell spell;

    private void Start()
    {
        spellComponent = gameObject.GetComponent<SpellComponent>();
    }

    private void Update()
    {
        if (!isAttacking && Time.time - lastAttackTime > cooldown && Vector3.Distance(transform.position, playerTransform.position) < 2.5f)
        {
            Attack();
        }

        Move();
        
    }

    protected override void Move()
    {
        if (playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);
        }
    }

    protected override void Attack() 
    {
        isAttacking = true;
        //yield return new WaitForSeconds(0.35f);
        StartCoroutine(CastSpell());

        //yield return new WaitForSeconds(lungeDuration);

        isAttacking = false;
        lastAttackTime = Time.time;
    }

    private IEnumerator CastSpell()
    {
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
        
    }

}
