using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DemonShopPedestal : MonoBehaviour
{
    public ShopRoom myShop;
    public bool isActive;
    public int itemTier;
    public int amountLeft;
    [SerializeField] ItemData myItem;
   [SerializeField] TextMeshProUGUI itemTitleText;
   [SerializeField] TextMeshProUGUI amountLeftText;
   [SerializeField] TextMeshProUGUI costText;
   [SerializeField] Image itemImage;
    public List<GameObject> toggleObjects = new List<GameObject>();
    virtual protected void Update()
    {
        if (!isActive|| amountLeft <= 0)
            return;


        if (Input.GetButtonDown("Fire3"))
        {
             if(myShop.isBeingRobbed)
            {
                PurchaseItem();
                return;
            }
            if (LootManager.instance.AttemptDemonPayment(myItem.basePrice))
                PurchaseItem();
        }


    }
    public void SetItem(ItemData newItem, int amount = 1)
    {
        myItem = newItem;
        itemTitleText.text = myItem.itemName;
        itemImage.sprite = myItem.itemSprite;
        itemImage.color = myItem.itemColor;
        costText.text = myItem.basePrice.ToString();
        amountLeft = amount;
        amountLeftText.text = "(" + amountLeft + ")";

    }
    public void ToggleVisibility(bool visible_)
    {
        isActive = visible_;
        if (amountLeft <= 0)
            isActive = false;
        foreach (GameObject obj in toggleObjects)
        {
            obj.SetActive(isActive);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            myShop.SetPedestalsInactive();
            ToggleVisibility(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ToggleVisibility(false);
        }
    }
    virtual protected void PurchaseItem()
    {
        if (!myItem)
            return;
        amountLeft -= 1;
        amountLeftText.text = "(" + amountLeft + ")";
        myShop.PurchaseItem(myItem, transform);
        if (amountLeft <= 0)
            ToggleVisibility(false);
    }
    public void SetBeingRobbed()
    {
        costText.text = "Free";
    }
}
