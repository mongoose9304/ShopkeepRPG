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

    public void SetUp(Sprite image_,string name_,int amount_)
    {
        itemAmountRequired.text ="X " +amount_.ToString();
        itemName.text = name_;
        itemImage.sprite = image_;
    }
}
