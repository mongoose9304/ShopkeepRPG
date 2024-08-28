using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlotsChest : TreasureChest
{
   public SlotMachineSingle slotMachine;
    public int cost;
    public TextMeshProUGUI costText;
    protected override void OpenChest()
    {
        if (slotMachine.isSpinning)
            return;
        slotMachine.Spin();
    }
    public void Jackpot()
    {
        value = DungeonManager.instance.currentDungeon.GetTreasureChestAmount();
        CoinSpawner.instance_.CreateDemonCoins(value*2, spawnLocation);
        base.OpenChest();
    }
    public void Loss()
    {

    }
    public void Win()
    {
        value = DungeonManager.instance.currentDungeon.GetTreasureChestAmount();
        base.OpenChest();
    }
    protected override void ToggleInteractablity(bool inRange_)
    {
        if (isOpening)
            return;
        cost = DungeonManager.instance.currentDungeon.GetTreasureChestAmount() / 3;
        costText.text = cost.ToString();
        myText.SetActive(inRange_);
        playerInRange = inRange_;

    }
}
