using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class HotbarSlot : MonoBehaviour
{
    public GameObject highlightObject;
    public GameObject engagedObject;
    public TextMeshProUGUI amountText;
    public UnityEvent onSelectEvent;
    public UnityEvent onDeselectEvent;
    public Image itemImage;
    public void SetHighlighted()
    {
        highlightObject.SetActive(true);
        onSelectEvent.Invoke();
    }
    public void SetUnHighlighted()
    {
        highlightObject.SetActive(false);
        engagedObject.SetActive(false);
        onDeselectEvent.Invoke();
    }
    public float Use(int amount_)
    {
        engagedObject.SetActive(true);
        amountText.text = amount_.ToString();
        if(amount_==0)
        {
            itemImage.color = new Color(0, 0, 0, 0);
        }
        return 1;
    }
    public void AddItem(Sprite itemSprite_,int amount_,Color color_)
    {
        itemImage.sprite = itemSprite_;
        itemImage.color = color_;
        amountText.text = amount_.ToString();
    }
}
