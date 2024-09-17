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
   [SerializeField] TextMeshProUGUI itemTitleText;
   [SerializeField] Image itemImage;
    public List<GameObject> toggleObjects = new List<GameObject>();
    public void SetItem(string title_,Sprite sprite_)
    {
        itemTitleText.text = title_;
        itemImage.sprite = sprite_;
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
        ToggleVisibility(false);
    }
}
