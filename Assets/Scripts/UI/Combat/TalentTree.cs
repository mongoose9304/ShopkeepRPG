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
        
        if (pointsInvested >= 5)
            return;
        if (myTalentUI.playerTalents.unspentTalents <= 0)
            return;


        if(myTalentUI.InvestPoint(ID))
        {
            pointsInvested += 1;
            SetUp(pointsInvested);
            myTalentUI.ApplyPlayerChanges();
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

            mySlots[i].AddPoint();      
        }

    }
    public void SetText(string desc_,string title_)
    {
        myTalentUI.SetDescription(title_,desc_);
        myTalentUI.SetScrollTarget(scrollTarget);
    }
}
