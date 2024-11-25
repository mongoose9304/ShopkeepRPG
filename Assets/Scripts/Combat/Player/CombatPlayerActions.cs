using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using TMPro;
using UnityEngine.InputSystem;
public class CombatPlayerActions : MonoBehaviour
{
    public CombatPlayerMovement combatMovement;
    [Header("BasicMelee")]
    [SerializeField] private float BasicMeleeCooldownMax;
    private float BasicMeleeCooldown = 0.0f;
    [SerializeField] private GameObject BasicMeleePivotObject;
    public BasicMeleeObject meleeObject;
    [Header("BasicRanged")]
    [SerializeField] float basicRangedDamage;
    public Element basicRangedElement;
   
    [SerializeField] private float fireCost;
    private float currentFireRate = 0.0f;
    [SerializeField] private GameObject rangedProjectile;
    [SerializeField] private List<GameObject> projectiles=new List<GameObject>();
    [SerializeField] private int projectileLimit;
    [SerializeField] Transform spawnPosition;

    [Header("SpecialAttacks")]
    [SerializeField] PlayerSpecialAttack specialA;
    float currentSpecialACooldown;
    [SerializeField] PlayerSpecialAttack specialB;
    public bool isBusy;
    public bool isUsingBasicAttackRanged;
    public bool isUsingBasicAttackMelee;
    float currentSpecialBCooldown;
    public GameObject specialAbilityHolder;
    public PlayerSpecialAbilities specialAbilities;
    [Header("Modifiers")]
    [SerializeField] private float fireRate;
    public float fireRateMod = 1;
    public float attackSpeedMod = 1;
    public float lifeStealPercent = 0;
    public bool rangedPierce;
    
    [Header("Familiar")]
    public CombatFamiliar myFamiliar;
    float familarRespawnTimer;
    private GameObject tempObj;
    [Header("Feel")]
    public MMProgressBar specialCoolDownBarA;
    public GameObject chargesUIBGA;
    public TextMeshProUGUI chargesTextA;
    public MMProgressBar specialCoolDownBarB;
    public GameObject chargesUIBGB;
    public TextMeshProUGUI chargesTextB;
    public MMProgressBar ultimateCoolDownBar;
    [Header("Audio")]
    public AudioClip basicRangedAudio;
    [Header("Inputs")]
    private bool PlayerIsHoldingMelee;

    private void Start()
    {
        SetUpProjectiles();
        SwapSpecials();
        combatMovement.myPlayerInputActions.Player.XAction.performed += OnMeleePressed;
        combatMovement.myPlayerInputActions.Player.XAction.canceled += OnMeleeReleased;
        combatMovement.myPlayerInputActions.Player.XAction.Enable();
    }
    private void OnEnable()
    {
        
    }

   

    private void OnDisable()
    {
        combatMovement.myPlayerInputActions.Player.XAction.Disable();
    }
    private void Update()
    {
        if (TempPause.instance.isPaused)
            return;
        isUsingBasicAttackRanged = false;
        isUsingBasicAttackMelee = false;
        if (specialA.isBusy||specialB.isBusy||myFamiliar.isBusy)
        {
            isBusy = true;
            meleeObject.ForceEndAttack();
            return;
        }
        else
        {
            isBusy = false;
            
        }

        if(PlayerIsHoldingMelee)
        {
           
            BasicMelee();
            isUsingBasicAttackMelee = true;


        }
        /*
        else if(Input.GetButton("Fire1"))
        {
            BasicRanged();
            isUsingBasicAttackRanged = true;
            meleeObject.ForceEndAttack();
        }
        else if(Input.GetButton("Special1"))
        {
            UseSpecialAttack(true);
        }
        else if (Input.GetButton("Special2"))
        {
            UseSpecialAttack(false);
        }
        else if (Input.GetButton("Special3"))
        {
            UseFamiliarUltimate();
        }
        else if (Input.GetAxis("Special3")==1)
        {
            UseFamiliarUltimate();
        }


        if (Input.GetButtonUp("Fire3"))
        {
            meleeObject.ReleaseMeleeButton();
        }
      */
        Cooldowns();
    }

    private void Cooldowns()
    {
        BasicMeleeCooldown -= Time.deltaTime;
        currentFireRate -= Time.deltaTime*fireRateMod;
        if (currentSpecialACooldown > 0)
        {
            currentSpecialACooldown -= Time.deltaTime;
            //specialCoolDownBarA.UpdateBar01((specialA.maxCoolDown-currentSpecialACooldown)/specialA.maxCoolDown);
            if (currentSpecialACooldown <= 0)
            {
                if (specialA.useCharges)
                {
                    if (specialA.currentCharges < specialA.maxCharges)
                    {
                        specialA.currentCharges += 1;
                        chargesTextA.text = specialA.currentCharges.ToString();
                        if (specialA.currentCharges < specialA.maxCharges)
                            currentSpecialACooldown = specialA.maxCoolDown;
                    }
                }
            }
            specialCoolDownBarA.SetBar01((specialA.maxCoolDown-currentSpecialACooldown)/specialA.maxCoolDown);
           
        }
        if (currentSpecialBCooldown > 0)
        {
            currentSpecialBCooldown -= Time.deltaTime;
            //specialCoolDownBarA.UpdateBar01((specialB.maxCoolDown-currentSpecialBCooldown)/specialB.maxCoolDown);
            if (currentSpecialBCooldown <= 0)
            {
                if (specialB.useCharges)
                {
                    if (specialB.currentCharges < specialB.maxCharges)
                    {
                        specialB.currentCharges += 1;
                        chargesTextB.text = specialB.currentCharges.ToString();
                        if (specialB.currentCharges < specialB.maxCharges)
                            currentSpecialBCooldown = specialB.maxCoolDown;
                    }
                }
            }
            specialCoolDownBarB.SetBar01((specialB.maxCoolDown - currentSpecialBCooldown) / specialB.maxCoolDown);


        }
        ultimateCoolDownBar.SetBar01(myFamiliar.GetUltimateAttackCooldown());
     
        if(familarRespawnTimer>0)
        {
         
            familarRespawnTimer -= Time.deltaTime;
            float temp = (myFamiliar.RespawnTimeMax - familarRespawnTimer) / myFamiliar.RespawnTimeMax;
            if (temp < 0)
                temp = 0;
            combatMovement.SetFamiliarHealth(temp);
            if(familarRespawnTimer <= 0)
            {
                RespawnFamiliar();
            }
        }
       
    }

    private void BasicMelee()
    {
        if(meleeObject.TryToAttack())
        {
            BasicMeleeCooldown =BasicMeleeCooldownMax;
        }
    }
    private void BasicRanged()
    {
        if(currentFireRate<=0)
        {
            if (combatMovement.GetCurrentMana() < fireCost)
                return;
            combatMovement.UseMana(fireCost);
            tempObj = GetAvailableProjectile();
            tempObj.transform.position = spawnPosition.position;
            tempObj.transform.rotation = spawnPosition.rotation;
            tempObj.SetActive(true);
            if (combatMovement.GetCurrentTarget() != null)
                tempObj.GetComponent<HomingAttack>().target = combatMovement.GetCurrentTarget().transform;
            else
                tempObj.GetComponent<HomingAttack>().target = null;
            tempObj.GetComponent<PlayerDamageCollider>().damage = basicRangedDamage;
            tempObj.GetComponent<PlayerDamageCollider>().element = basicRangedElement;
            tempObj.GetComponent<PlayerDamageCollider>().canPierceEnemies = rangedPierce;
            currentFireRate = fireRate;
            MMSoundManager.Instance.PlaySound(basicRangedAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
          false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.9f, 1.1f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
          1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
        }
    }
    private void UseSpecialAttack(bool specialA_)
    {
        if (specialA.isBusy || specialB.isBusy)
            return;

        if(specialA_)
        {
            if (combatMovement.GetCurrentMana() < specialA.manaCost||!specialA.CanBeUsed())
                return;
            if(specialA.useCharges)
            {
                if (specialA.currentCharges <= 0)
                    return;
                specialA.currentCharges -= 1;
                chargesTextA.text = specialA.currentCharges.ToString();
            }
            else
            {
                if ( currentSpecialACooldown > 0)
                    return;
            }
            currentSpecialACooldown = specialA.maxCoolDown;
            combatMovement.UseMana(specialA.manaCost);
            specialA.OnPress(this.gameObject);
           
        }
        else
        {
            if (combatMovement.GetCurrentMana() < specialB.manaCost|| !specialB.CanBeUsed())
                return;
            if (specialB.useCharges)
            {
                if (specialB.currentCharges <= 0)
                    return;
                specialB.currentCharges -= 1;
                chargesTextB.text = specialB.currentCharges.ToString();
            }
            else
            {
                if (currentSpecialBCooldown > 0)
                    return;
            }
            currentSpecialBCooldown = specialB.maxCoolDown;
            combatMovement.UseMana(specialB.manaCost);
            specialB.OnPress(this.gameObject);
          
        }

    }
    private void UseFamiliarUltimate()
    {
        if(myFamiliar)
        {
            if(!myFamiliar.gameObject.activeInHierarchy)
            {
                return;
            }
            if(familarRespawnTimer<=0)
            myFamiliar.UltimateAttack();
        }
    }
    private void SetUpProjectiles()
    {
        if (projectiles.Count > 0)
        {
            foreach (GameObject obj in projectiles)
                Destroy(obj);
            projectiles.Clear();
        }
        for(int i=0;i<projectileLimit;i++)
        {
            tempObj = Instantiate(rangedProjectile, spawnPosition.position, spawnPosition.rotation);
            tempObj.SetActive(false);
            projectiles.Add(tempObj);
            tempObj = null;
        }
    }
    GameObject GetAvailableProjectile()
    {
        foreach (GameObject obj in projectiles)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }
        return null;
    }
   public void FamiliarDeath(float respawnTime_)
    {
        familarRespawnTimer = respawnTime_;
    }
    public void RespawnFamiliar()
    {
        myFamiliar.transform.position = this.transform.position + new Vector3(2,0,0);
        myFamiliar.gameObject.SetActive(true);
        myFamiliar.Respawn();
    }
    public void ReturnFamiliar()
    {
        myFamiliar.TeleportToLocation(transform);
    }
    public void DisableFamiliar(bool enable_)
    {
        myFamiliar.gameObject.SetActive(enable_);
    }
    public void SetStats(float basicMeleeDamage,float basicRangedDamage_,Element rangedE_,Element meleeE_)
    {
        meleeObject.SetDamage(basicMeleeDamage, meleeE_,attackSpeedMod,lifeStealPercent);
        basicRangedDamage = basicRangedDamage_;
        basicRangedElement = rangedE_;
    }
    public void SetSpecialDamages(float Patk_,float Matk_)
    {
        if(specialA)
        {
            specialA.CalculateDamage(Patk_, Matk_);
        }
        if(specialB)
        {
            specialB.CalculateDamage(Patk_, Matk_);
        }
    }
    public void SwapSpecials()
    {
        if(specialA)
        {
            Destroy(specialA.gameObject);
        }
        if(specialB)
        {
            Destroy(specialB.gameObject);
        }
        specialA = GameObject.Instantiate(specialAbilities.currentlyEquipt[0].theSpecialAttack.gameObject,specialAbilityHolder.transform).GetComponent<PlayerSpecialAttack>();
        specialB = GameObject.Instantiate(specialAbilities.currentlyEquipt[1].theSpecialAttack.gameObject,specialAbilityHolder.transform).GetComponent<PlayerSpecialAttack>();
        if(specialA.useCharges)
        {
            chargesUIBGA.SetActive(true);
            chargesTextA.text = specialA.currentCharges.ToString();
        }
        else
        {

            chargesUIBGA.SetActive(false);
        }
        if (specialB.useCharges)
        {
            chargesUIBGB.SetActive(true);
            chargesTextB.text = specialB.currentCharges.ToString();

        }
        else
        {
            chargesUIBGB.SetActive(false);
        }
        combatMovement.CalculateAllModifiers();
    }
    //New Inputs
    private void OnMeleePressed(InputAction.CallbackContext obj)
    {
        PlayerIsHoldingMelee = true;
    }
    private void OnMeleeReleased(InputAction.CallbackContext obj)
    {
        PlayerIsHoldingMelee = false;
    }
}
