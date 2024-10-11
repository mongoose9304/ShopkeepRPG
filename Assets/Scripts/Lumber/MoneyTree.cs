using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MoneyTree : Tree
{
    public int comboRequired;
    public TextMeshProUGUI comboText;
    public UnityEvent openChestEvent;
    public int value;
    public int myQuality;
    public float chanceForHigherLoot;
    public LootDropper myDropper;

    protected override void Start()
    {

        base.Start();
        comboText.text = comboRequired.ToString();
    }
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
        if(TreeManager.instance.GetCurrentCombo()>=comboRequired)
        {
            OpenChest();
        }
    }
    public void OpenChest()
    {
        SetUpLootDrop();
        openChestEvent.Invoke();
        myDropper.DropItems();
    }
    public void SetUpLootDrop()
    {
        int x = myQuality;
        if (Random.Range(0, 100) < chanceForHigherLoot)
        {
            x += 1;
        }
        myDropper.SetLootTable(LumberLevelManager.instance.currentLevel.GetDigItemTier(x));
    }
}
