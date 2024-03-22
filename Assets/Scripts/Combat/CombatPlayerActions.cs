using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayerActions : MonoBehaviour
{
    [SerializeField] CombatPlayerMovement combatMovement;
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
    private GameObject tempObj;
    private void Start()
    {
        SetUpProjectiles();
    }
    private void Update()
    {
        if(specialA.isBusy||specialB.isBusy)
        {
            isBusy = true;
            return;
        }
        else
        {
            isBusy = false;
        }

        if(Input.GetButton("Fire1"))
        {
           
            BasicMelee();
          
        }
        else if(Input.GetButton("Fire3"))
        {
            BasicRanged();
        }
        else if(Input.GetButton("Special1"))
        {
            UseSpecialAttack(true);
        }
        else if (Input.GetButton("Special2"))
        {
            UseSpecialAttack(false);
        }


        if (Input.GetButtonUp("Fire1"))
        {
            meleeObject.ReleaseMeleeButton();
        }
        Cooldowns();
    }

    private void Cooldowns()
    {
        BasicMeleeCooldown -= Time.deltaTime;
        currentFireRate -= Time.deltaTime;
        currentSpecialACooldown -= Time.deltaTime;
        currentSpecialBCooldown -= Time.deltaTime;
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

            currentSpecialACooldown = specialA.maxCoolDown;
            specialA.OnPress(this.gameObject);
            Debug.Log("SpecA");
        }
        else
        {
            if (combatMovement.GetCurrentMana() < specialB.manaCost || currentSpecialBCooldown > 0)
                return;
            currentSpecialBCooldown = specialB.maxCoolDown;
            specialB.OnPress(this.gameObject);
            Debug.Log("SpecB");
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
   
}
