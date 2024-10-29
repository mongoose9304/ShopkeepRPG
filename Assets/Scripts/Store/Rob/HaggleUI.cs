using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HaggleUI : MonoBehaviour
{
    public InventorySlot currentPedestalSlot;
    public Pedestal openPedestal;
    public GameObject buttons;
    public TextMeshProUGUI currentItemNameText;
    public TextMeshProUGUI currentItemValue;
    public TextMeshProUGUI haggleItemValue;
    public void OpenMenu(Pedestal p_)
    {
        openPedestal = p_;
    }
}
