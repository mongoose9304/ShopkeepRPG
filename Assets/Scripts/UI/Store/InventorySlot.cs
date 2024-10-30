using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public ItemData myItem;
    public int amount;
    [SerializeField] TextMeshProUGUI myAmountText;
    [SerializeField] Image myItemImage;
    [SerializeField] InventoryUI myUI;
    public void SetItem(ItemData item_, int amount_)
    {
        myItem = item_;
        amount = amount_;
        myAmountText.text = amount.ToString();
        myItemImage.sprite = myItem.itemSprite;
    }
    public void SetNullItem()
    {
        myItem = null;
        amount = 0;
        myAmountText.text = amount.ToString();
        myItemImage.sprite = null;
    }
    public void Clear()
    {
        myItem = null;
        amount = 0;
        gameObject.SetActive(false);
    }
    public void UpdateAmount(int amount_)
    {
        amount = amount_;
        myAmountText.text = amount.ToString();
    }
    public void ClickedObject()
    {
        if(myUI)
        {
            myUI.InventoryButtonClicked(this);
        }
    }
}