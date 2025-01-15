using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCraftingRecipe", menuName = "ScriptableObjects/CraftingRecipe", order = 1)]
public class CraftingRecipe : ScriptableObject
{
    public List<ItemData> requiredItems=new List<ItemData>();
    public List<int> requiredAmounts=new List<int>();
    public int unlockCost;
    public int humanCraftingCost;
    public int hellCraftingCost;
    public int woodCraftingCost;
    public int stoneCraftingCost;
    public ItemData itemToCraft;
}
