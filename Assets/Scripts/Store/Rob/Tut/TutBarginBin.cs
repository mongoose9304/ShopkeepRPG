using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutBarginBin : BarginBin
{
    public int tutStateIndex;
    public override void SetSlot(int index, ItemData item_, int amount_)
    {
        binSlots[index].myItem = item_;
        binSlots[index].amount = amount_;
        binImages[index].sprite = item_.itemSprite;
        binImages[index].gameObject.SetActive(true);
        if(tutStateIndex>0)
        {
            ShopTutorialManager.instance.SetTutorialState(tutStateIndex);
            tutStateIndex = 0;
        }
        
    }
}
