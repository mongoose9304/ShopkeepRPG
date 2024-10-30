using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pedestal : InteractableObject
{
    public ItemData myItem;
    public int amount;
    public Image myItemImage;
 
    /// <summary>
    /// The virtual function all interactbale objects will override to set thier specific functionality
    /// </summary>
    public override void Interact(GameObject interactingObject_ = null)
    {
        ShopManager.instance.OpenPedestal(this);
    }
    public void SetItem(ItemData myItem_,int amount_)
    {
        myItem = myItem_;
        amount = amount_;
        myItemImage.sprite = myItem.itemSprite;
        myItemImage.gameObject.SetActive(true);
    }
    public void ClearItem()
    {
        myItemImage.gameObject.SetActive(false); ;
        myItem = null;
        amount = 0;
        myItemImage.sprite = null;
    }
    public void SetInUse(bool inUse)
    {

        GetComponent<Collider>().enabled = !inUse;
    }
   
}
