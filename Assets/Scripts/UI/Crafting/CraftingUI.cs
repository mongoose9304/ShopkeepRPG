using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingUI : MonoBehaviour
{
    public CraftingRecipe currentRecipe;
    public List<CraftingRecipe> unlockedRecipes=new List<CraftingRecipe>();
    public List<RecipeButton> recipeBtns=new List<RecipeButton>();
    public List<RecipeRequirementSlot> requirementSlots=new List<RecipeRequirementSlot>();
    public RecipeRequirementSlot humanCashSlot;
    public RecipeRequirementSlot stoneSlot;
    public RecipeRequirementSlot lumberSlot;
    public int amountToCraft;
    public TextMeshProUGUI amountToCraftText;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemValueText;
    public Image itemImage;

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
    public void TryToCraft()
    {

        for (int i = 0; i < currentRecipe.requiredItems.Count; i++)
        {
         if(PlayerInventory.instance.GetItemAmount(currentRecipe.requiredItems[i].itemName) < currentRecipe.requiredAmounts[i] * amountToCraft)
            {
                Debug.Log("Rob1");
                return;
            }

        }
        if (PlayerInventory.instance.GetHumanCash() < currentRecipe.humanCraftingCost * amountToCraft)
        {
            Debug.Log("Rob2");
            return;
        }
        if (PlayerInventory.instance.GetStone() < currentRecipe.stoneCraftingCost * amountToCraft)
        {
            Debug.Log("Rob3");
            return;
        }
        if (PlayerInventory.instance.GetWood() < currentRecipe.woodCraftingCost * amountToCraft)
        {
            Debug.Log("Rob4");
            return;
        }


        Craft();
    }
    private void Craft()
    {
        for (int i = 0; i < currentRecipe.requiredItems.Count; i++)
        {
            PlayerInventory.instance.ConsumeItem(currentRecipe.requiredItems[i].itemName, currentRecipe.requiredAmounts[i]*amountToCraft);
        }
        if (currentRecipe.humanCraftingCost > 0)
        {
            PlayerInventory.instance.AddHumanCash(-currentRecipe.humanCraftingCost * amountToCraft);
        }
        if (currentRecipe.stoneCraftingCost > 0)
        {
            PlayerInventory.instance.AddStone(-currentRecipe.humanCraftingCost * amountToCraft);
        }
        if (currentRecipe.woodCraftingCost > 0)
        {
            PlayerInventory.instance.AddWood(-currentRecipe.humanCraftingCost * amountToCraft);
        }
        SetAmountText(amountToCraft);
        InstantCraft();
    }
    private void InstantCraft()
    {
        PlayerInventory.instance.AddItemToInventory(currentRecipe.itemToCraft.itemName,amountToCraft);
    }
    public void IncreaseCraftAmount()
    {
        if (amountToCraft < 10)
            amountToCraft += 1;
        SetAmountText(amountToCraft);
    }
    public void DecreaseCraftAmount()
    {
        if (amountToCraft > 1)
            amountToCraft -= 1;
        SetAmountText(amountToCraft);
    }
    private void SetAmountText(int amount_)
    {
        if (amountToCraft <= 0)
            return;
        amountToCraft = amount_;
            amountToCraftText.text = amountToCraft.ToString();
        CalculateNewItemCost();
    }
    private void CalculateNewItemCost()
    {
        if (!currentRecipe)
            return;
        for (int i = 0; i < currentRecipe.requiredItems.Count; i++)
        {
            requirementSlots[i].SetUp(currentRecipe.requiredItems[i].itemSprite, currentRecipe.requiredItems[i].itemName, currentRecipe.requiredAmounts[i]*amountToCraft, PlayerInventory.instance.GetItemAmount(currentRecipe.requiredItems[i].itemName));
            requirementSlots[i].gameObject.SetActive(true);
        }
        if (currentRecipe.humanCraftingCost > 0)
        {
            humanCashSlot.SetAmount(currentRecipe.humanCraftingCost*amountToCraft, PlayerInventory.instance.GetHumanCash());
            humanCashSlot.gameObject.SetActive(true);
        }
        if (currentRecipe.stoneCraftingCost > 0)
        {
            stoneSlot.SetAmount(currentRecipe.stoneCraftingCost*amountToCraft, PlayerInventory.instance.GetStone());
            stoneSlot.gameObject.SetActive(true);
        }
        if (currentRecipe.woodCraftingCost > 0)
        {
            lumberSlot.SetAmount(currentRecipe.woodCraftingCost*amountToCraft, PlayerInventory.instance.GetWood());
            lumberSlot.gameObject.SetActive(true);
        }
    }
    public void SwitchActiveRecipe(CraftingRecipe recipe_)
    {
        humanCashSlot.gameObject.SetActive(false);
        stoneSlot.gameObject.SetActive(false);
        lumberSlot.gameObject.SetActive(false);
        SetAmountText(1);
        itemName.text = recipe_.itemToCraft.itemName;
        itemImage.sprite = recipe_.itemToCraft.itemSprite;
        itemValueText.text = recipe_.itemToCraft.basePrice.ToString();
        foreach (RecipeRequirementSlot btn in requirementSlots)
        {
            btn.gameObject.SetActive(false);
        }
        currentRecipe = recipe_;
        CalculateNewItemCost();
    }



}
