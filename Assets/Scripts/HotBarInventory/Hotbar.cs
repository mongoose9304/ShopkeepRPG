using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    public List<ItemData> TESTITEMS = new List<ItemData>();
    public List<HotbarSlot> mySlots = new List<HotbarSlot>();
    public List<ItemData> Items = new List<ItemData>();
    public List<int> ItemAmounts = new List<int>();
    public int currentHighlight;
    float hotbarInput;
    public float delayBetweenInputsMax;
    float delayBetweenInputsCurrent;
    public float delayBetweenItemUsages;
    private void Start()
    {
        foreach(ItemData data in TESTITEMS)
        {
            AddItemToHotbar(data, 10);
        }
        SetHighlightedSlot(0);
    }
    private void Update()
    {
        GetInput();
        if (delayBetweenItemUsages > 0)
        {
            delayBetweenItemUsages -= Time.deltaTime;
            if (delayBetweenItemUsages < 0)
            {

            }
        }
        if (delayBetweenInputsCurrent > 0)
        {
            delayBetweenInputsCurrent -= Time.deltaTime;
            return;
        }
        if(hotbarInput>.1)
        {
            MoveHighlightedSlot(true);
            delayBetweenInputsCurrent = delayBetweenInputsMax;
        }
        else if (hotbarInput < -0.1)
        {
            MoveHighlightedSlot(false);
            delayBetweenInputsCurrent = delayBetweenInputsMax;
        }
    }
    void GetInput()
    {
        hotbarInput = Input.GetAxis("DpadHorizontal");
        if(hotbarInput==0)
        {
            delayBetweenInputsCurrent = 0;
        }
        if (Input.GetAxis("UseItem") == 1)
        {
            if(delayBetweenItemUsages<=0)
            UseSelectedItem();
        }

    }
    public void UseSelectedItem()
    {
        if(Items[currentHighlight] && ItemAmounts[currentHighlight]>0)
        {
            if (Items[currentHighlight].type == ItemData.ItemType.consumable)
            {


                ItemAmounts[currentHighlight] -= 1;
                if (ItemAmounts[currentHighlight] == 0)
                {
                    Items[currentHighlight] = null;
                }
                if(CombatPlayerManager.instance)
                {
                    CombatPlayerManager.instance.HealPlayer(Items[currentHighlight].consumeHealthValue);
                }
            }
        }
       delayBetweenItemUsages = mySlots[currentHighlight].Use(ItemAmounts[currentHighlight]);
    }
    public void SetHighlightedSlot(int Slot_)
    {
        mySlots[Slot_].SetHighlighted();
    }
    public void DeHighlightSlot(int Slot_)
    {
        mySlots[Slot_].SetUnHighlighted();
    }
    public void MoveHighlightedSlot(bool moveRight=false)
    {
        DeHighlightSlot(currentHighlight);
        if(moveRight)
        {
            currentHighlight += 1;
            if(currentHighlight>=mySlots.Count)
                currentHighlight = 0;
        }
        else
        {
            currentHighlight -= 1;
            if (currentHighlight < 0)
                currentHighlight = mySlots.Count-1;
        }
        mySlots[currentHighlight].SetHighlighted();
    }
    public void AddItemToHotbar(ItemData data_,int amount=1)
    {
        for(int i=0;i<mySlots.Count;i++)
        {
            if(!Items[i])
            {
                Items[i] = data_;
                ItemAmounts[i] = amount;
                mySlots[i].AddItem(data_.itemSprite, amount);
                break;
            }
        }
    }
}
