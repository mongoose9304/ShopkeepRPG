using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    public List<CraftingRecipe> unlockedRecipes=new List<CraftingRecipe>();
    public List<RecipeButton> recipeBtns=new List<RecipeButton>();
    public List<RecipeRequirementSlot> requirementSlots=new List<RecipeRequirementSlot>();

    private void OnEnable()
    {
        SetUpScreen();
    }

    protected void SetUpScreen()
    {
        foreach(RecipeButton btn in recipeBtns)
        {
            btn.gameObject.SetActive(false);
        }
        for(int i=0;i<unlockedRecipes.Count;i++)
        {
            recipeBtns[i].SetUpBtn(unlockedRecipes[i]);
            recipeBtns[i].gameObject.SetActive(true);
        }
    }
    public void SwitchActiveRecipe(CraftingRecipe recipe_)
    {
        foreach (RecipeRequirementSlot btn in requirementSlots)
        {
            btn.gameObject.SetActive(false);
        }
        for (int i = 0; i < recipe_.requiredItems.Count; i++)
        {
            requirementSlots[i].SetUp(recipe_.requiredItems[i].itemSprite, recipe_.requiredItems[i].itemName, recipe_.requiredAmounts[i]);
            requirementSlots[i].gameObject.SetActive(true);
        }
    }



}
