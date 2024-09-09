using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerTalentUi : MonoBehaviour
{
    public SavedTalents playerTalents;
    public TextMeshProUGUI talentPointsRemainingText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI talentTitleText;
    public List<TalentTree> myTrees = new List<TalentTree>();
    public ScrollRectController rectController;
    private void OnEnable()
    {
        SetUp();
    }
    private void SpendPoint()
    {
        playerTalents.unspentTalents -= 1;
        talentPointsRemainingText.text = playerTalents.unspentTalents.ToString();
    }
    public bool InvestPoint(string ID)
    {
      
        for (int i= 0;i < playerTalents.talents.Count;i++)
        {
            if (playerTalents.talents[i].ID ==ID)
            {
                Talent tx = playerTalents.talents[i];
                tx.levelInvested += 1;
                playerTalents.talents[i] = tx;
                SpendPoint();
                return true;
            }
        }
        return false;
    }

    public void SetUp()
    {
        talentPointsRemainingText.text = playerTalents.unspentTalents.ToString();
        foreach(TalentTree tree in myTrees)
        {
           foreach(Talent t_ in playerTalents.talents)
            {
                if(t_.ID==tree.ID)
                {
                    tree.SetUp(t_.levelInvested);
                    
                }
            }
            
        }
    }
    public void SetDescription(string title_, string description_)
    {
        talentTitleText.text = title_;
        descriptionText.text = description_;
    }
    public void SetScrollTarget(int target_)
    {
        rectController.target = target_;
    }
}
