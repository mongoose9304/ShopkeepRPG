using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsChest : TreasureChest
{
   public SlotMachineSingle slotMachine;
    protected override void OpenChest()
    {
        if (slotMachine.isSpinning)
            return;
        slotMachine.Spin();
    }
    public void Jackpot()
    {
        CoinSpawner.instance_.CreateDemonCoins(value*2, spawnLocation);
        base.OpenChest();
    }
    public void Loss()
    {

    }
    public void Win()
    {
        base.OpenChest();
    }
}
