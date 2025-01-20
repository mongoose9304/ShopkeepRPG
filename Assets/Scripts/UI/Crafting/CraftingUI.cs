using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    public List<CraftingRecipe> unlockedRecipes=new List<CraftingRecipe>();
    public List<RecipeButton> recipeBtns=new List<RecipeButton>();
    public List<RecipeRequirementSlot> requirementSlots=new List<RecipeRequirementSlot>();
    public RecipeRequirementSlot humanCashSlot;
    public RecipeRequirementSlot stoneSlot;
    public RecipeRequirementSlot lumberSlot;

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
        humanCashSlot.gameObject.SetActive(false);
        stoneSlot.gameObject.SetActive(false);
        lumberSlot.gameObject.SetActive(false);
        foreach (RecipeRequirementSlot btn in requirementSlots)
        {
            btn.gameObject.SetActive(false);
        }
        for (int i = 0; i < recipe_.requiredItems.Count; i++)
        {
            requirementSlots[i].SetUp(recipe_.requiredItems[i].itemSprite, recipe_.requiredItems[i].itemName, recipe_.requiredAmounts[i],PlayerInventory.instance.GetItemAmount(recipe_.requiredItems[i].itemName));
            requirementSlots[i].gameObject.SetActive(true);
        }
        if (recipe_.humanCraftingCost>0)
        {
            humanCashSlot.SetAmount(recipe_.humanCraftingCost,PlayerInventory.instance.GetHumanCash());
            humanCashSlot.gameObject.SetActive(true);
        }
        if (recipe_.stoneCraftingCost > 0)
        {
            stoneSlot.SetAmount(recipe_.stoneCraftingCost, PlayerInventory.instance.GetStone());
            stoneSlot.gameObject.SetActive(true);
        }
        if (recipe_.woodCraftingCost > 0)
        {
            lumberSlot.SetAmount(recipe_.woodCraftingCost, PlayerInventory.instance.GetWood());
            lumberSlot.gameObject.SetActive(true);
        }
    }



}
