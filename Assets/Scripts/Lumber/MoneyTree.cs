using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTree : Tree
{
    public int value;
    
    protected override void SpawnWood()
    {
        foreach (GameObject obj in myTreeSections)
        {
            CoinSpawner.instance_.CreateRegularCoins(value, obj.transform);
        }
        if (TreeManager.instance.GetCurrentCombo() == 1)
        {
            CoinSpawner.instance_.CreateRegularCoins(value, transform);
        }
        else if (TreeManager.instance.GetCurrentCombo() > 1)
        {
            CoinSpawner.instance_.CreateRegularCoins(value* TreeManager.instance.GetCurrentCombo(), transform);
        }
    }
}
