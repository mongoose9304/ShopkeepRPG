using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeRequirementSlot : MonoBehaviour
{
    public Image itemImage; 
    public TextMeshProUGUI itemName; 
    public TextMeshProUGUI itemAmountRequired;
    public TextMeshProUGUI itemAmountHelds;
    

    public void SetUp(Sprite image_,string name_,int amount_,int amountHeld)
    {
        itemAmountRequired.text ="X " +amount_.ToString();
        itemAmountHelds.text = amountHeld.ToString();
        itemName.text = name_;
        itemImage.sprite = image_;
        if(amount_>amountHeld)
        {
            itemImage.color = Color.red;
        }
        else
        {
            itemImage.color = Color.white;
        }
    }
    public void SetAmount(int amount_, int amountHeld)
    {
        itemAmountRequired.text = "X " + amount_.ToString();
        itemAmountHelds.text = amountHeld.ToString();
        if (amount_ > amountHeld)
        {
            itemImage.color = Color.red;
        }
        else
        {
            itemImage.color = Color.white;
        }
    }
}
