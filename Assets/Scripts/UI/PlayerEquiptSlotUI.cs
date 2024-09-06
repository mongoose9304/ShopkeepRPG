using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEquiptSlotUI : MonoBehaviour
{
    public string description;
    public string title;
    public CombatEquiptUI equiptUI;
    public void SetSlot(string title_,string desc_)
    {
        title = title_;
        description = desc_;
    }
    public void SetTitle()
    {
        equiptUI.SetDescription(title, description);
    }
}
