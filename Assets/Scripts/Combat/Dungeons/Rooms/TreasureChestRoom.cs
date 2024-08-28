using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestRoom : BasicRoom
{
    [SerializeField] GameObject regularChest;
    [SerializeField] GameObject slotsChest;
    [SerializeField] TreasureChest myChest;
    [SerializeField] int curseSeverity = 1;
   
    private void OnEnable()
    {
        if(DungeonManager.instance.currentSin==SinType.Capriciousness)
        {
            slotsChest.SetActive(true);
            regularChest.SetActive(false);
            myChest = slotsChest.GetComponent<TreasureChest>();
        }
        else
        {
            slotsChest.SetActive(false);
            regularChest.SetActive(true);
            myChest = regularChest.GetComponent<TreasureChest>();
        }
        if (myType == RoomType.CursedLoot)
        {
            myChest.SetIsCursed(true, curseSeverity);
        }
        else
        {
            myChest.SetIsCursed(false, curseSeverity);
        }
    }


}
