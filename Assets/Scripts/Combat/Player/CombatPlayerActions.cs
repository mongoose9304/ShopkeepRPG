using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
public class CombatPlayerActions : MonoBehaviour
{
    public CombatPlayerMovement combatMovement;
    [Header("BasicMelee")]
    [SerializeField] private float BasicMeleeCooldownMax;
    private float BasicMeleeCooldown = 0.0f;
    [SerializeField] private GameObject BasicMeleePivotObject;
    [SerializeField] BasicMeleeObject meleeObject;
    [Header("BasicRanged")]
    [SerializeField] private float fireRate;
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
    float currentSpecialBCooldown;
    [Header("Familiar")]
    [SerializeField] CombatFamiliar myFamiliar;
    float familarRespawnTimer;
    private GameObject tempObj;
    [Header("Feel")]
    public MMProgressBar specialCoolDownBarA;
    public MMProgressBar specialCoolDownBarB;
    public MMProgressBar ultimateCoolDownBar;
    private void Start()
    {
        SetUpProjectiles();
    }
    private void Update()
    {
        if(specialA.isBusy||specialB.isBusy||myFamiliar.isBusy)
        {
            isBusy = true;
            meleeObject.ForceEndAttack();
            return;
        }
        else
        {
            isBusy = false;
        }

        if(Input.GetButton("Fire3"))
        {
           
            BasicMelee();
          
        }
        else if(Input.GetButton("Fire1"))
        {
            BasicRanged();
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
        Cooldowns();
    }

    private void Cooldowns()
    {
        BasicMeleeCooldown -= Time.deltaTime;
        currentFireRate -= Time.deltaTime;
        if (currentSpecialACooldown > 0)
        {
            currentSpecialACooldown -= Time.deltaTime;
            //specialCoolDownBarA.UpdateBar01((specialA.maxCoolDown-currentSpecialACooldown)/specialA.maxCoolDown);
            specialCoolDownBarA.SetBar01((specialA.maxCoolDown-currentSpecialACooldown)/specialA.maxCoolDown);
        
           
           
        }
        if (currentSpecialBCooldown > 0)
        {
            currentSpecialBCooldown -= Time.deltaTime;
            //specialCoolDownBarA.UpdateBar01((specialB.maxCoolDown-currentSpecialBCooldown)/specialB.maxCoolDown);
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
            currentFireRate = fireRate;
        }
    }
    private void UseSpecialAttack(bool specialA_)
    {
        if (specialA.isBusy || specialB.isBusy)
            return;

        if(specialA_)
        {
            if (combatMovement.GetCurrentMana() < specialA.manaCost||currentSpecialACooldown>0)
                return;
            combatMovement.UseMana(specialA.manaCost);
            currentSpecialACooldown = specialA.maxCoolDown;
            specialA.OnPress(this.gameObject);
           
        }
        else
        {
            if (combatMovement.GetCurrentMana() < specialB.manaCost || currentSpecialBCooldown > 0)
                return;
            combatMovement.UseMana(specialB.manaCost);
            currentSpecialBCooldown = specialB.maxCoolDown;
            specialB.OnPress(this.gameObject);
          
        }

    }
    private void UseFamiliarUltimate()
    {
        if(myFamiliar)
        {
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
        myFamiliar.transform.position = this.transform.position + new Vector3(0.5F, 0, 0);
    }
    public void DisableFamiliar(bool enable_)
    {
        myFamiliar.gameObject.SetActive(enable_);
    }
}
