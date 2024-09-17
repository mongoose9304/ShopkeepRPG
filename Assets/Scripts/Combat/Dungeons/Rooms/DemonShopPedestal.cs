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
    [SerializeField] ItemData myItem;
   [SerializeField] TextMeshProUGUI itemTitleText;
   [SerializeField] TextMeshProUGUI costText;
   [SerializeField] Image itemImage;
    public List<GameObject> toggleObjects = new List<GameObject>();
    public void SetItem(ItemData newItem)
    {
        myItem = newItem;
        itemTitleText.text = myItem.itemName;
        itemImage.sprite = myItem.itemSprite;
        itemImage.color = myItem.itemColor;
        costText.text = myItem.basePrice.ToString();
    }
    public void ToggleVisibility(bool visible_)
    {
        isActive = visible_;
        foreach(GameObject obj in toggleObjects)
        {
            obj.SetActive(visible_);
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
}
