using Shopkeeper;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Move,
    Attack
}

public abstract class Enemy : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Rigidbody rb;
    protected CharacterHealth health;
    public Transform playerTransform;
    protected EnemyState enemyState;

    protected bool isAttacking = false;
    protected float cooldown = 2f;
    protected float lastAttackTime = -999f;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        health = GetComponent<CharacterHealth>();
    }

    protected virtual void Move() { }
    protected virtual void Attack() { }
}
