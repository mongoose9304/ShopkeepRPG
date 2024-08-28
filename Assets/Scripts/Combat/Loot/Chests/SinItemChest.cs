using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinItemChest : TreasureChest
{
    public int itemTier;
    public int numberOfItemsToDrop;
    [SerializeField] LootDropper dropper;
    protected override void OpenChest()
    {
        if (isOpening)
            return;
        base.OpenChest();
        for (int i = 0; i < numberOfItemsToDrop; i++)
        {
            dropper.DropSpecificItem(LootManager.instance.GetTieredItem(itemTier));
        }
    }
    private void OnEnable()
    {
        itemTier = DungeonManager.instance.currentItemTier;
    }
}
