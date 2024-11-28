using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// The slots that hold purchaseable items in the demon shop
/// </summary>
public class DemonShopPedestal : InteractableObject
{
    [Tooltip("The shop that I am a part of")]
    public ShopRoom myShop;
    [Tooltip("Is my UI visible in the game")]
    public bool isActive;
    [Tooltip("The tier my item should be from")]
    public int itemTier;
    [Tooltip("The number of items left to buy")]
    public int amountLeft;
    [Tooltip("The item i sell")]
    [SerializeField] ItemData myItem;
    [Tooltip("REFERENCE UI item title")]
    [SerializeField] TextMeshProUGUI itemTitleText;
    [Tooltip("REFERENCE amount left UI text")]
    [SerializeField] TextMeshProUGUI amountLeftText;
    [Tooltip("REFERENCE to the cost UI text")]
    [SerializeField] TextMeshProUGUI costText;
    [Tooltip("REFERENCE to UI image of this item")]
    [SerializeField] Image itemImage;
    [Tooltip("REFERENCE to objects that should be toggled on and off when player is in range")]
    public List<GameObject> toggleObjects = new List<GameObject>();
    private float delayBetweenPurchases;
    private void Update()
    {
        if (delayBetweenPurchases > 0)
            delayBetweenPurchases -= Time.deltaTime;
    }
    /// <summary>
    /// The virtual function all interactbale objects will override to set thier specific functionality
    /// </summary>
    public override void Interact(GameObject interactingObject_ = null)
    {
        if (myShop.isBeingRobbed)
        {
            PurchaseItem();
            return;
        }
        if (delayBetweenPurchases > 0)
            return;
        delayBetweenPurchases = 0.5f;
        if (LootManager.instance.AttemptDemonPayment(myItem.basePrice))
            PurchaseItem();
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

    virtual protected void PurchaseItem()
    {
        if (!myItem)
            return;
        amountLeft -= 1;
        amountLeftText.text = "(" + amountLeft + ")";
        myShop.PurchaseItem(myItem, transform);
        if (amountLeft <= 0)
        {
            ToggleVisibility(false);
            CombatPlayerManager.instance.RemoveInteractableObject(gameObject);
            gameObject.SetActive(false);
        }
    }
    public void SetBeingRobbed()
    {
        costText.text = "Free";
    }
}
