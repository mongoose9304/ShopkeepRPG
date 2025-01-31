using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeButton : MonoBehaviour
{
    public CraftingUI craftUI;
    public CraftingRecipe myRecipe;
    public Image myImage;
    public TextMeshProUGUI myName;
    public void SetUpBtn(CraftingRecipe recipe_)
    {
        myRecipe = recipe_;
        myName.text = myRecipe.itemToCraft.itemName;
        myImage.sprite = myRecipe.itemToCraft.itemSprite;
    }
    public void ButtonPressed()
    {
        craftUI.SwitchActiveRecipe(myRecipe);
    }
    public void OnHover()
    {
        craftUI.SwitchActiveRecipe(myRecipe);
    }
}
