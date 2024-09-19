using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHotbar : MonoBehaviour
{
    public List<HotbarSlot> mySlots = new List<HotbarSlot>();
    public int currentHighlight;
    private void Start()
    {
       
        SetHighlightedSlot(0);
    }
    private void Update()
    {
        GetInput();
    }
    void GetInput()
    {
        if (Input.GetButtonDown("Special1"))
        {
            MoveHighlightedSlot(false);
        }
        else if (Input.GetButtonDown("Special2"))
        {
            MoveHighlightedSlot(true);
        }
    }
    public void MoveHighlightedSlot(bool moveRight = false)
    {
        DeHighlightSlot(currentHighlight);
        if (moveRight)
        {
            currentHighlight += 1;
            if (currentHighlight >= mySlots.Count)
                currentHighlight = 0;
        }
        else
        {
            currentHighlight -= 1;
            if (currentHighlight < 0)
                currentHighlight = mySlots.Count - 1;
        }
        mySlots[currentHighlight].SetHighlighted();
    }
    public void SetHighlightedSlot(int Slot_)
    {
        mySlots[Slot_].SetHighlighted();
    }
    public void DeHighlightSlot(int Slot_)
    {
        mySlots[Slot_].SetUnHighlighted();
    }
    public void MoveToSlot_(int Slot_)
    {
        foreach(HotbarSlot slot in mySlots)
        {
            slot.SetUnHighlighted();
        }
        mySlots[Slot_].SetHighlighted();
    }
}
