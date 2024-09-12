using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilitySlotUI : MonoBehaviour
{
    public string description;
    public string title;
    public Image abilityImage;
    public CombatEquiptUI equiptUI;
    public void SetSlot(string title_, string desc_,Sprite abilitySprite_)
    {
        title = title_;
        description = desc_;
        abilityImage.sprite = abilitySprite_;
    }
    public void SetTitle()
    {
        equiptUI.SetDescription(title, description);
    }
    public void SetAsPlayerAbility()
    {
        equiptUI.SetAbilityAsEquipt(title);
    }
}
