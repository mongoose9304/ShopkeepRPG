using Shopkeeper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFlying: Enemy
{
    private SpellComponent spellComponent;
    [SerializeField]
    private Spell spell;

    [SerializeField]
    CapsuleCollider trigger;

    [SerializeField] private float fleeChance = 0.2f; // 20% chance to flee on cooldown
    [SerializeField] private float fleeDuration = 0.5f;
    private bool isFleeing = false;
    private float fleeTimer = 0f;

    [SerializeField] private float fleeCooldown = 5f;
    private float lastFleeTime = -Mathf.Infinity;

    private void Start()
    {
        spellComponent = gameObject.GetComponent<SpellComponent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        trigger.enabled = false;
    }

    private void Update()
    {
        if (isFleeing)
        {
            fleeTimer -= Time.deltaTime;
            FleeFromPlayer();

            if (fleeTimer <= 0f)
            {
                isFleeing = false;
                enemyState = EnemyState.Idle;
            }

            return;
        }

        if (Time.time - lastAttackTime > cooldown)
        {
            enemyState = EnemyState.Move;
            Move();

            if (enemyState != EnemyState.Attack && Vector3.Distance(transform.position, target.position) < 2.5f)
            {
                Attack();
            }
        }
    }

    private void StartFleeing()
    {
        isFleeing = true;
        fleeTimer = fleeDuration;
        lastFleeTime = Time.time;
        enemyState = EnemyState.Move;
    }

    private void FleeFromPlayer()
    {
        if (target == null) return;

        Vector3 dirAway = (transform.position - target.position).normalized;
        Vector3 fleeTarget = transform.position + dirAway * 5f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleeTarget, out hit, 5f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
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

        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0;
        transform.forward = dir;

        yield return new WaitForSeconds(0.15f);

        spellComponent.CastSpell(spell);

        yield return new WaitForSeconds(0.1f);

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        agent.Warp(transform.position);
        agent.isStopped = false;
        agent.updatePosition = true;

        trigger.enabled = false;
        lastAttackTime = Time.time;

        StartFleeing();
    }

}