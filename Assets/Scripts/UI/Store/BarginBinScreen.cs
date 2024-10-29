using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BarginBinScreen : MonoBehaviour
{
    public InventorySlot currentlySelectedSlot;
    public InventoryUI inventoryUI;
    public InventorySlot currentInventorySlot;
    public BarginBin openBarginBin;
    public GameObject buttons;
    public TextMeshProUGUI currentItemNameText;
    public TextMeshProUGUI currentItemValue;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public void OpenMenu(BarginBin bin_)
    {
        openBarginBin = bin_;
        LoadInventory(bin_);
    }
    public void LoadInventory(BarginBin bin_)
    {
        int index = 0;
        foreach (InventorySlot slot_ in slots)
        {
            slot_.Clear();
            slot_.gameObject.SetActive(true);
        }
        foreach (BarginBinSlot item_ in bin_.binSlots)
        {
            if (item_.myItem)
            {
                slots[index].SetItem(item_.myItem, item_.amount);
                slots[index].gameObject.SetActive(true);
                index += 1;
            }
        }
    }
}
