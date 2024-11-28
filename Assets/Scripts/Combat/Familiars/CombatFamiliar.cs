using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The mostly virtual class all familiars (allies that follow the player) inherit from
/// </summary>
public class CombatFamiliar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject player;
    [SerializeField] protected NavMeshAgent agent;
    public GameObject target;
    public bool isBusy;
    protected Animator anim;
    private CombatPlayerMovement combatPlayerMovement;
    private CombatPlayerActions combatPlayerActions;
    [SerializeField] public StatBlock monsterStats;
    [SerializeField] GameObject deathEffect;

    bool hasLookedForNewtarget;
    [Header("Stats")]
    [SerializeField]protected  float specialAttackCooldownMax;
    [SerializeField]protected float ultimateAttackCooldownMax;
    [SerializeField]protected float AttackCooldownMax;
    [SerializeField]public float RespawnTimeMax;
    protected float specialAttackCooldowncurrent;
    protected float AttackCooldowncurrent;
    protected float ultimateAttackCooldowncurrent;
    [SerializeField] float maxDistanceToPlayer;
    [SerializeField] float maxDistanceToTarget;
    [SerializeField] float respawnTimeMax;
    [SerializeField] float delayBeforeLookingForAnotherTargetMax;
    float delayBeforeLookingForAnotherTargetCurrent;
    //StatsFromData
    float currentHealth;
    //Stats Calculated based on Stat block
    public float maxHealth;
    public Element myWeakness;
    public Element myElement;
    public float PhysicalAtk;
    public float MysticalAtk;
    public float PhysicalDef;
    public float MysticalDef;
    public float LevelModifier;
    public float HealthRegenPercent;
    public List<EquipModifier> externalModifiers = new List<EquipModifier>();
    [Header("Feel")]
    [SerializeField] MMF_Player textSpawner;
    [SerializeField] MMF_Player hitEffects;
    public MMF_FloatingText floatingText;
    private void Awake()
    {
        if (player)
        {
            combatPlayerMovement = player.GetComponent<CombatPlayerMovement>();
            combatPlayerActions = player.GetComponent<CombatPlayerActions>();
        }
    }
    protected virtual void Start()
    {
        specialAttackCooldowncurrent = specialAttackCooldownMax;
        ultimateAttackCooldowncurrent = ultimateAttackCooldownMax;
        anim = GetComponent<Animator>();
        floatingText = textSpawner.GetFeedbackOfType<MMF_FloatingText>();
        //fix this later, if the enemies have the same channel their damage numbers will appear even if they are not hit =(
        floatingText.Channel = Random.Range(0, 1000000);
        textSpawner.GetComponent<MMFloatingTextSpawner>().Channel = floatingText.Channel;
        currentHealth = maxHealth;
        //combatPlayerMovement.SetFamiliarHealth(currentHealth / maxHealth);

    }
    protected virtual void Update()
    {
        if (TempPause.instance.isPaused)
            return;
        FollowPlayer();
        EnemyDetection();
        RegenHealth();
    }
    /// <summary>
    /// Causes the Familiar to walk towards the player if they have no current target
    /// </summary>
    public virtual void FollowPlayer()
    {
        if (target)
            return;
        if (Vector3.Distance(this.transform.position, player.transform.position) > maxDistanceToPlayer)
        {
            agent.SetDestination(player.transform.position+new Vector3(0,0,2));
        }
       
    }
    /// <summary>
    /// Will cause the familiar to find the nearest enemy
    /// </summary>
    public virtual void EnemyDetection()
    {
     
        if(!target)
        {
            if(!hasLookedForNewtarget)
            {
                hasLookedForNewtarget = true;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxDistanceToTarget-1);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.tag == "Enemy")
                    {
                        if (!target)
                            target = hitCollider.gameObject;
                        else
                        {
                            if (Vector3.Distance(this.transform.position, target.transform.position) > Vector3.Distance(this.transform.position, hitCollider.transform.position))
                            {
                                target = hitCollider.gameObject;
                            }
                        }
                    }
                }
            }
            else
            {
                delayBeforeLookingForAnotherTargetCurrent -= Time.deltaTime;
                if(delayBeforeLookingForAnotherTargetCurrent <= 0)
                {
                    hasLookedForNewtarget = false;
                    delayBeforeLookingForAnotherTargetCurrent = delayBeforeLookingForAnotherTargetMax;
                }
            }
            return;
        }
        hasLookedForNewtarget = false;
       
        if (Vector3.Distance(this.transform.position, target.transform.position) < maxDistanceToTarget)
        {
            agent.SetDestination(target.transform.position);
            if (!target.activeInHierarchy)
                target = null;
        }
        else
        {
            target = null;
        }
       
    }
    public virtual void TakeDamage(float damage_, float hitstun_, Element element_, float knockBack_ = 0, GameObject knockBackObject = null, bool isMystical = false)
    {
        float newDamage = damage_;
        if (element_ == myWeakness && element_ != Element.Neutral)
        {
            newDamage *= 1.5f;
        }
        if (isMystical)
        {
            newDamage -= MysticalDef;
        }
        else
        {
            newDamage -= PhysicalDef;
        }
        if (newDamage < damage_ * 0.05f)
            newDamage = damage_ * 0.05f;
        currentHealth -= newDamage;
        if (currentHealth <= 0)
        {
            Death();
            return;
        }

        if (textSpawner)
        {
            floatingText.Value = damage_.ToString();
            textSpawner.PlayFeedbacks();
        }
        if (hitEffects)
            hitEffects.PlayFeedbacks();
        if(combatPlayerMovement)
        combatPlayerMovement.UpdateFamiliarHealth(currentHealth/maxHealth);
    }
    private void Death()
    {
        combatPlayerActions.FamiliarDeath(respawnTimeMax);
        gameObject.SetActive(false);
        Instantiate(deathEffect,transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
    }
    public void Respawn()
    {
        specialAttackCooldowncurrent = specialAttackCooldownMax;
        ultimateAttackCooldowncurrent = ultimateAttackCooldownMax;
        currentHealth = maxHealth;
        combatPlayerMovement.UpdateFamiliarHealth(currentHealth / maxHealth);
    }
    public void TeleportToLocation(Transform location_)
    {
        transform.position = location_.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 3.0f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
            //transform.position = hit.position;
        }
    }
    /// <summary>
    /// The most basic attack the familar knows
    /// </summary>
    public virtual void Attack()
    {

    }
    /// <summary>
    /// The more character specific attack the familar knows, same as thier enemy counterpart
    /// </summary>
    public virtual void SpecialAttack()
    {

    }
    /// <summary>
    /// This is a move the player can perform, it is usually a combo attack with the famiiar 
    /// </summary>
    public virtual void UltimateAttack()
    {

    }
    
    public float GetUltimateAttackCooldown()
    {
        return (ultimateAttackCooldownMax - ultimateAttackCooldowncurrent) / ultimateAttackCooldownMax;
    }
    protected virtual void CalculateStats()
    {
        maxHealth = (monsterStats.Vitality * 2) * (1 + (monsterStats.Level * LevelModifier));
        PhysicalAtk = (monsterStats.PhysicalProwess) * (1 + (monsterStats.Level * LevelModifier))/5;
        MysticalAtk = (monsterStats.MysticalProwess) * (1 + (monsterStats.Level * LevelModifier))/5;
        PhysicalDef = (monsterStats.PhysicalDefense) * (1 + (monsterStats.Level * LevelModifier))/5;
        MysticalDef = (monsterStats.MysticalDefense) * (1 + (monsterStats.Level * LevelModifier))/5;
        HealthRegenPercent = 0;
    }
    public virtual void CalculateAllModifiers()
    {
        CalculateStats();
        List<EquipModifier> mods_ = new List<EquipModifier>();
        for (int i = 0; i < externalModifiers.Count; i++)
        {
            if (!externalModifiers[i].isMultiplicative)
            {
                mods_.Insert(0, externalModifiers[i]);
            }
            else
            {
                mods_.Add(externalModifiers[i]);
            }
        }
        for (int i = 0; i < mods_.Count; i++)
        {
            ApplyModifier(mods_[i]);
        }
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        if(combatPlayerMovement)
        combatPlayerMovement.SetFamiliarHealth(currentHealth / maxHealth);

    }
    private void ApplyModifier(EquipModifier mod_)
    {
        if (mod_.uniqueEffect == UniqueEquipEffect.None)
        {
            switch (mod_.affectedStat)
            {
                case Stat.HP:
                    maxHealth = AddOrMultiply(mod_.isMultiplicative, maxHealth, mod_.amount);
                    break;
                case Stat.PATK:
                    PhysicalAtk = AddOrMultiply(mod_.isMultiplicative, PhysicalAtk, mod_.amount);
                    break;
                case Stat.MATK:
                    MysticalAtk = AddOrMultiply(mod_.isMultiplicative, MysticalAtk, mod_.amount);
                    break;
                case Stat.PDEF:
                    PhysicalDef = AddOrMultiply(mod_.isMultiplicative, PhysicalDef, mod_.amount);
                    break;
                case Stat.MDEF:
                    MysticalDef = AddOrMultiply(mod_.isMultiplicative, MysticalDef, mod_.amount);
                    break;
            }
        }
        switch (mod_.uniqueEffect)
        {
            case UniqueEquipEffect.None:
                return;
            case UniqueEquipEffect.HealthRegen:
                HealthRegenPercent += mod_.amount;
                break;
        }
    }
    private float AddOrMultiply(bool multiply_, float A, float B)
    {
        if (multiply_)
        {
            return A * B;
        }
        else
        {
            return A + B;
        }
    }
    public void AddExternalMod(EquipModifier mod_)
    {
        foreach (EquipModifier modX in externalModifiers)
        {
            if (modX.modName == mod_.modName)
            {
                externalModifiers.Remove(modX);
                externalModifiers.Add(mod_);
                return;
            }
        }
        externalModifiers.Add(mod_);
    }
    protected void RegenHealth()
    {
        if (HealthRegenPercent == 0)
            return;
        currentHealth += HealthRegenPercent * maxHealth * Time.deltaTime;

        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;
        combatPlayerMovement.SetFamiliarHealth(currentHealth / maxHealth);

    }
}
