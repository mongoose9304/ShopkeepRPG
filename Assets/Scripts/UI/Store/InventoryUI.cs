using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();
    private void Start()
    {
        LoadInventory();
    }
    public void LoadInventory()
    {
        int index = 0;
        foreach(InventorySlot slot_ in slots)
        {
            slot_.Clear();
        }
        foreach (InventoryItem item_ in PlayerInventory.instance.masterItemList)
        {
            if(item_.amount>0)
            {
                slots[index].SetItem(item_.myItem, item_.amount);
                slots[index].gameObject.SetActive(true);
                index += 1;
            }
        }
    }
}
