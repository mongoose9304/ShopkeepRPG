using Shopkeeper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGoblin : Enemy
{   
    private SpellComponent spellComponent;
    [SerializeField]
    private Spell spell;
    [SerializeField]
    private Spell shield;

    bool useFirst = true;

    [SerializeField]
    CapsuleCollider trigger;

    [SerializeField] private float idealRange = 10.5f;         
    [SerializeField] private float tooCloseRange = 5.5f;      
    [SerializeField] private float stopDistanceBuffer = 0.5f;

    [SerializeField] private float backAwayCooldown = 3f;
    [SerializeField] private float backAwayChance = 0.5f;
    [SerializeField] private float backAwayDistance = 1.5f;
    [SerializeField] private float backAwaySpeed = 2f;

    [SerializeField] private float turnSpeed = 5f;

    private float lastBackAwayTime = -Mathf.Infinity;

    private void Start()
    {
        spellComponent = gameObject.GetComponent<SpellComponent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        trigger.enabled = false;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (enemyState == EnemyState.Attack)
            return;

        if (distance > idealRange + stopDistanceBuffer)
        {
            Approach();
        }
        else if (distance < tooCloseRange)
        {
            if (enemyState != EnemyState.Move && Time.time - lastBackAwayTime > backAwayCooldown && Random.value < backAwayChance)
            {
                BackAway();
                lastBackAwayTime = Time.time;
                return;
            }

            if (enemyState != EnemyState.Move && Time.time - lastAttackTime > cooldown)
            {
                Attack();
            }
            return;
        }
        else 
        {
            agent.SetDestination(transform.position);
            agent.isStopped = true;

            if (Time.time - lastAttackTime > cooldown)
            {
                Attack();
            }
        }    
    }

    private void Approach()
    {
        enemyState = EnemyState.Move;
        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    private void BackAway()
    {
        enemyState = EnemyState.Move;
        agent.isStopped = false;
        agent.speed = backAwaySpeed;

        Vector3 directionAway = (transform.position - target.position).normalized;
        Vector3 backoffPoint = transform.position + directionAway * backAwayDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(backoffPoint, out hit, 2f, NavMesh.AllAreas))
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

    private IEnumerator LookAt(Vector3 targetPosition)
    {
        Quaternion startRot = transform.rotation;
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;

        if (direction == Vector3.zero)
            yield break;

        Quaternion targetRot = Quaternion.LookRotation(direction);

        float t = 0f;
        while (Quaternion.Angle(transform.rotation, targetRot) > 1f)
        {
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            t += Time.deltaTime * turnSpeed;
            yield return null;
        }

        transform.rotation = targetRot; // snap to final just in case
    }

    private IEnumerator CastSpell()
    {
        enemyState = EnemyState.Attack;
        trigger.enabled = true;
        agent.isStopped = true;
        agent.updatePosition = false;
        rb.isKinematic = false;

        yield return StartCoroutine(LookAt(target.position));

        yield return new WaitForSeconds(0.35f);

        //toggle between 2 spells
        if (useFirst) 
        {
            spellComponent.CastSpell(spell);
            yield return new WaitForSeconds(0.2f);
        }
        else 
        {
            spellComponent.CastSpell(shield);
            yield return new WaitForSeconds(3.0f);
        }

        useFirst = !useFirst;

        

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
