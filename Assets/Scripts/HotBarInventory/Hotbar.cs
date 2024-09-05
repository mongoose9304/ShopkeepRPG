using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    public List<HotbarSlot> mySlots = new List<HotbarSlot>();
    public int currentHighlight;
    float hotbarInput;
    public float delayBetweenInputsMax;
    float delayBetweenInputsCurrent;
    public float delayBetweenItemUsages;
    private void Start()
    {
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
       delayBetweenItemUsages= mySlots[currentHighlight].Use();
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
}
