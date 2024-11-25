using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventHotbar : MonoBehaviour
{
    public List<HotbarSlot> mySlots = new List<HotbarSlot>();
    public int currentHighlight;
    private void Start()
    {
       
        SetHighlightedSlot(0);
    }
    private void OnEnable()
    {
        if(CombatPlayerManager.instance)
        {
            CombatPlayerManager.instance.GetPlayer(0).combatMovement.myPlayerInputActions.Player.LBAction.performed += MoveSlotLeft;
            CombatPlayerManager.instance.GetPlayer(0).combatMovement.myPlayerInputActions.Player.RBAction.performed += MoveSlotRight;
        }
    }
    private void OnDisable()
    {
        if (CombatPlayerManager.instance)
        {
            CombatPlayerManager.instance.GetPlayer(0).combatMovement.myPlayerInputActions.Player.LBAction.performed -= MoveSlotLeft;
            CombatPlayerManager.instance.GetPlayer(0).combatMovement.myPlayerInputActions.Player.RBAction.performed -= MoveSlotRight;
        }
    }

    private void MoveSlotLeft(InputAction.CallbackContext obj)
    {
        MoveHighlightedSlot(false);
    }
    private void MoveSlotRight(InputAction.CallbackContext obj)
    {
        MoveHighlightedSlot(true);
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
