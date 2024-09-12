using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CombatEquiptUI : MonoBehaviour
{
    public StatBlock playerStatBlock;
    public PlayerSpecialAbilities playerSpecialAbilities;
    public CombatPlayerMovement player;
    public PlayerEquiptmentHolder playerEquiptment;
    public TextMeshProUGUI skillPointsText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI descriptionTitle;
    public TextMeshProUGUI descriptionText;
    public List<PlayerStatUIObject> playerStatObjects = new List<PlayerStatUIObject>();
    public List<PlayerEquiptSlotUI> playerEquiptObjects = new List<PlayerEquiptSlotUI>();
    public List<PlayerAbilitySlotUI> playerAbilityInventorySlots = new List<PlayerAbilitySlotUI>();
    public List<PlayerAbilitySlotUI> playerAbilityCurrentlyEquiptSlots = new List<PlayerAbilitySlotUI>();
    public GameObject playerAbilityInventoryHolder;
    private int currentlySelectedAbility;
    public bool TryToLevelUp()
    {
        if(playerStatBlock.remainingSkillPoints>0)
        {
  
            return true;
        }
        return false;
    }
    public void LevelUp()
    {
        playerStatBlock.remainingSkillPoints -= 1;
        skillPointsText.text = playerStatBlock.remainingSkillPoints.ToString();
        SaveChanges();
        if (player)
            player.CalculateAllModifiers();
    }
    public void SetUpUI()
    {
        levelText.text = playerStatBlock.Level.ToString();
        skillPointsText.text = playerStatBlock.remainingSkillPoints.ToString();
        playerStatObjects[0].SetUpStat(playerStatBlock.Vitality);
        playerStatObjects[1].SetUpStat(playerStatBlock.Soul);
        playerStatObjects[2].SetUpStat(playerStatBlock.PhysicalProwess);
        playerStatObjects[3].SetUpStat(playerStatBlock.MysticalProwess);
        playerStatObjects[4].SetUpStat(playerStatBlock.PhysicalDefense);
        playerStatObjects[5].SetUpStat(playerStatBlock.MysticalDefense);
        playerEquiptObjects[0].SetSlot(playerEquiptment.Armor.equiptName, playerEquiptment.Armor.description);
        playerEquiptObjects[1].SetSlot(playerEquiptment.MeleeWeapon.equiptName, playerEquiptment.MeleeWeapon.description);
        playerEquiptObjects[2].SetSlot(playerEquiptment.RangedWeapon.equiptName, playerEquiptment.RangedWeapon.description);
        playerEquiptObjects[3].SetSlot(playerEquiptment.Rings[0].equiptName, playerEquiptment.Rings[0].description);
        playerEquiptObjects[4].SetSlot(playerEquiptment.Rings[1].equiptName, playerEquiptment.Rings[1].description);
        playerEquiptObjects[5].SetSlot(playerEquiptment.Rings[2].equiptName, playerEquiptment.Rings[2].description);
        for (int i = 0; i < playerAbilityInventorySlots.Count; i++)
        {
            playerAbilityInventorySlots[i].gameObject.SetActive(false);
        }
        for (int i=0;i<playerSpecialAbilities.allAbilities.Count;i++)
        {
            playerAbilityInventorySlots[i].SetSlot(playerSpecialAbilities.allAbilities[i].name_, playerSpecialAbilities.allAbilities[i].description_, playerSpecialAbilities.allAbilities[i].abilitySprite);
            playerAbilityInventorySlots[i].gameObject.SetActive(true);
        }
        playerAbilityCurrentlyEquiptSlots[0].SetSlot(playerSpecialAbilities.currentlyEquipt[0].name_, playerSpecialAbilities.currentlyEquipt[0].description_, playerSpecialAbilities.currentlyEquipt[0].abilitySprite);
        playerAbilityCurrentlyEquiptSlots[1].SetSlot(playerSpecialAbilities.currentlyEquipt[1].name_, playerSpecialAbilities.currentlyEquipt[1].description_, playerSpecialAbilities.currentlyEquipt[1].abilitySprite);
        playerAbilityInventoryHolder.SetActive(false);
    }
    public void SaveChanges()
    {
        playerStatBlock.Vitality = playerStatObjects[0].amount;
        playerStatBlock.Soul = playerStatObjects[1].amount;
        playerStatBlock.PhysicalProwess = playerStatObjects[2].amount;
        playerStatBlock.MysticalProwess = playerStatObjects[3].amount;
        playerStatBlock.PhysicalDefense = playerStatObjects[4].amount;
        playerStatBlock.MysticalDefense = playerStatObjects[5].amount;
    }
    private void OnEnable()
    {
        SetUpUI();
    }
    public void SetDescription(string title_,string description_)
    {
        descriptionTitle.text = title_;
        descriptionText.text = description_;
    }
    public void OpenAbilityChangeSection(int abilityIndex)
    {
        currentlySelectedAbility = abilityIndex;
        playerAbilityInventoryHolder.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(playerAbilityInventorySlots[0].gameObject);
    }
    public void SetAbilityAsEquipt(string name_)
    {
        playerSpecialAbilities.SetEquiptPower(currentlySelectedAbility, name_);
        playerAbilityCurrentlyEquiptSlots[0].SetSlot(playerSpecialAbilities.currentlyEquipt[0].name_, playerSpecialAbilities.currentlyEquipt[0].description_, playerSpecialAbilities.currentlyEquipt[0].abilitySprite);
        playerAbilityCurrentlyEquiptSlots[1].SetSlot(playerSpecialAbilities.currentlyEquipt[1].name_, playerSpecialAbilities.currentlyEquipt[1].description_, playerSpecialAbilities.currentlyEquipt[1].abilitySprite);
        playerAbilityInventoryHolder.SetActive(false);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(playerAbilityCurrentlyEquiptSlots[currentlySelectedAbility].gameObject);
        if (player)
            player.combatActions.SwapSpecials();
    }
}
