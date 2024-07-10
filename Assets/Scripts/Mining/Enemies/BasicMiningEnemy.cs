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
   [SerializeField]protected LootDropper lootDropper;
    [Header("References")]
    public GameObject player;
    float currentHealth;

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lootDropper = GetComponent<LootDropper>();
        currentHealth = maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        
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
    }
}
