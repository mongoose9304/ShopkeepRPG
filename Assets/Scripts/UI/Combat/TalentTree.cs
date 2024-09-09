using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentTree : MonoBehaviour
{
    public int pointsInvested;
    public int scrollTarget;
    public string ID;
    public PlayerTalentUi myTalentUI;
    public List<TalentSlot> mySlots = new List<TalentSlot>();
    public void InvestPoint()
    {
        
        if (pointsInvested >= 11)
            return;
        if (myTalentUI.playerTalents.unspentTalents <= 0)
            return;


        if(myTalentUI.InvestPoint(ID))
        {
            pointsInvested += 1;
            SetUp(pointsInvested);
        }


    }
    public void SetUp(int points_)
    {
        foreach(TalentSlot slot in mySlots)
        {
            slot.ResetPoints();
        }
        pointsInvested = points_;
        if (points_ == 0)
            return;
        for(int i=0;i<points_;i++)
        {
            if(i<5)
            {
                mySlots[0].AddPoint();
            }
            else if(i<10)
            {
                mySlots[1].AddPoint();
            }
            else if(i < 11)
            {
                mySlots[2].AddPoint();
            }
        }

    }
    public void SetText(string desc_,string title_)
    {
        myTalentUI.SetDescription(title_,desc_);
        myTalentUI.SetScrollTarget(scrollTarget);
    }
}
