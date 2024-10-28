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

    public void SetItem(ItemData item_, int amount_)
    {
        myItem = item_;
        amount = amount_;
        myAmountText.text = amount.ToString();
        myItemImage.sprite = myItem.itemSprite;
    }
    public void Clear()
    {
        myItem = null;
        amount = 0;
        gameObject.SetActive(false);
    }
}
