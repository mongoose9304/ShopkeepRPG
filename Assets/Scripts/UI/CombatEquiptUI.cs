using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CombatEquiptUI : MonoBehaviour
{
    public StatBlock playerStatBlock;
    public TextMeshProUGUI skillPointsText;
    public TextMeshProUGUI levelText;
    public List<PlayerStatUIObject> playerStatObjects = new List<PlayerStatUIObject>();
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
}
