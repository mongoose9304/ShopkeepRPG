using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FamStatObject : MonoBehaviour
{
    public TextMeshProUGUI amountText;
    public int amount;
    public FamiliarEquiptUI famUI;
    public string description;
    public string title;
    public void SetUpStat(int amount_)
    {
        amount = amount_;
        amountText.text = amount.ToString();
    }
    public void IncreaseStat()
    {
        if (!famUI.TryToLevelUp())
            return;
        amount += 1;
        amountText.text = amount.ToString();
        famUI.LevelUp();
    }
    public void SetTitle()
    {
        famUI.SetDescription(title, description);
    }
}
