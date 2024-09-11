using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class BasicFollower : MonoBehaviour
{
    [Header("Stats")]
    public int Level;
    public BasicMonsterData myBaseData;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float maxHitstun;
    [SerializeField] protected float maxAttackCooldown;
    [SerializeField] protected Element myElement;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float knockBackMax;
    [SerializeField] protected float damage;
    [SerializeField] protected bool isMysticalDamage;

    [Header("CurrentValues")]
    public bool canMove;
    float currentTimeStunned;
    //count for how long an enemy has been stunned, after this passes the max an enemy should be able to act regardless of if the player could stun them again
    protected float currentHitstun;
    protected float currentAttackCooldown = 0.2f;
    protected float currentHealth;
    bool superArmor;

    [Header("References")]
    public GameObject myMaster;
    Element myWeakness;
    public GameObject stunIcon;
    public GameObject player;
    public GameObject target;
    public EnemyCounter myEnemyCounter;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected TextMeshProUGUI damageText; 
    [SerializeField] float maxTimeBeforeDamageTextFades;
    [SerializeField] float currentTimeBeforeDamageTextFades;
    [SerializeField] float fadeTimeMultiplier;
    [SerializeField] TeamUser myTeamUser;


    [Header("Feel")]
    [SerializeField] MMF_Player textSpawner;
    [SerializeField] MMF_Player hitEffects;
    public MMF_FloatingText floatingText;
    [SerializeField] protected MMMiniObjectPooler attackIconPooler;
    public bool useFlicker;

    
}
