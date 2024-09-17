using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DemonShopPedestal : MonoBehaviour
{
   public ShopRoom myShop;
   [SerializeField] TextMeshProUGUI itemTitleText;
   [SerializeField] TextMeshProUGUI itemDescText;
   [SerializeField] Image itemImage;
    public List<GameObject> toggleObjects = new List<GameObject>();
    public void SetItem(string title_,string desc_,Sprite sprite_)
    {
        itemTitleText.text = title_;
        itemDescText.text = desc_;
        itemImage.sprite = sprite_;
    }
    public void ToggleVisibility(bool visible_)
    {
        foreach(GameObject obj in toggleObjects)
        {
            obj.SetActive(visible_);
        }
    }
}
