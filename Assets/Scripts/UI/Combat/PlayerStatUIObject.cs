using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatUIObject : MonoBehaviour
{
    public TextMeshProUGUI amountText;
    public int amount;
    public CombatEquiptUI equiptUI;
    public string description;
    public string title;
    public void SetUpStat(int amount_)
    {
        amount = amount_;
        amountText.text = amount.ToString();
    }
    public void IncreaseStat()
    {
        if (!equiptUI.TryToLevelUp())
            return;
        amount += 1;
        amountText.text = amount.ToString();
        equiptUI.UseLevel();
    }
    public void IncreaseStatByTen()
    {
        if (!equiptUI.TryToLevelUpTen())
            return;
        amount += 10;
        amountText.text = amount.ToString();
        equiptUI.UseTenLevels();
    }
    public void SetTitle()
    {
        equiptUI.SetDescription(title, description);
    }
}
