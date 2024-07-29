using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberLevel : MonoBehaviour
{
    public LootTableItem[] tier1Items;
    public LootTableItem[] tier2Items;
    public LootTableItem[] tier3Items;
    public LootTableItem[] tier4Items;
    public LootTableItem[] tier5Items;



    public LootTableItem[] GetItemTier(int tier_)
    {
        switch (tier_)
        {
            case 1:
                return tier1Items;
                break;
            case 2:
                return tier2Items;
                break;
            case 3:
                return tier3Items;
                break;
            case 4:
                return tier4Items;
                break;
            case 5:
                return tier5Items;
                break;
            default:
                return tier1Items;
                break;

        }
    }
}
