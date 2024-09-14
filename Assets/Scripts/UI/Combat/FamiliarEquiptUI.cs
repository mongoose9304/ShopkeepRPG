using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FamiliarEquiptUI : MonoBehaviour
{
    public StatBlock famStatBlock;
    public CombatFamiliar combatFam;
    public TextMeshProUGUI skillPointsText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI descriptionTitle;
    public TextMeshProUGUI descriptionText;
    public List<FamStatObject> famStatObjects = new List<FamStatObject>();
    public bool TryToLevelUp()
    {
        if (famStatBlock.remainingSkillPoints > 0)
        {

            return true;
        }
        return false;
    }
    public void LevelUp()
    {
        famStatBlock.remainingSkillPoints -= 1;
        skillPointsText.text = famStatBlock.remainingSkillPoints.ToString();
        SaveChanges();
        if (combatFam)
            combatFam.CalculateAllModifiers();
    }
    public void SetDescription(string title_, string description_)
    {
        descriptionTitle.text = title_;
        descriptionText.text = description_;
    }
    public void SaveChanges()
    {
        famStatBlock.Vitality = famStatObjects[0].amount;
        famStatBlock.PhysicalProwess = famStatObjects[1].amount;
        famStatBlock.MysticalProwess = famStatObjects[2].amount;
        famStatBlock.PhysicalDefense = famStatObjects[3].amount;
        famStatBlock.MysticalDefense = famStatObjects[4].amount;
    }
    private void OnEnable()
    {
        SetUpUI();
    }
    public void SetUpUI()
    {
        levelText.text = famStatBlock.Level.ToString();
        skillPointsText.text = famStatBlock.remainingSkillPoints.ToString();
        famStatObjects[0].SetUpStat(famStatBlock.Vitality);
        famStatObjects[1].SetUpStat(famStatBlock.PhysicalProwess);
        famStatObjects[2].SetUpStat(famStatBlock.MysticalProwess);
        famStatObjects[3].SetUpStat(famStatBlock.PhysicalDefense);
        famStatObjects[4].SetUpStat(famStatBlock.MysticalDefense);
    }
}
