using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private InputAction movement;
    private void Start()
    {
        //Adding test items for debug pruposes, remove when ready 
        foreach(ItemData data in TESTITEMS)
        {
            AddItemToHotbar(data, 10);
        }
        SetHighlightedSlot(0);
    }
    private void OnEnable()
    {
        EnableActions();
    }
    private void EnableActions()
    {
        if (CombatPlayerManager.instance)
        {
            movement = CombatPlayerManager.instance.GetPlayer(0).combatMovement.myPlayerInputActions.Player.Dpad;
            CombatPlayerManager.instance.GetPlayer(0).combatMovement.myPlayerInputActions.Player.Dpad.Enable();
            CombatPlayerManager.instance.GetPlayer(0).combatMovement.myPlayerInputActions.Player.RTAction.performed += UseItemPressed;
            CombatPlayerManager.instance.GetPlayer(0).combatMovement.myPlayerInputActions.Player.RTAction.Enable();
        }
    }

    private void UseItemPressed(InputAction.CallbackContext obj)
    {
        if (delayBetweenItemUsages <= 0)
            UseSelectedItem();
    }

    private void OnDisable()
    {
        CombatPlayerManager.instance.GetPlayer(0).combatMovement.myPlayerInputActions.Player.Dpad.Disable();
        CombatPlayerManager.instance.GetPlayer(0).combatMovement.myPlayerInputActions.Player.RTAction.Disable();
    }
    private void Update()
    {
        if (TempPause.instance.isPaused)
            return;
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
        hotbarInput = movement.ReadValue<Vector2>().x;
        if(hotbarInput==0)
        {
            delayBetweenInputsCurrent = 0;
        }
        //temp number inputs, get this crap outta here later -Rob
       /* if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            DeHighlightSlot(currentHighlight);
            currentHighlight = 0;
            SetHighlightedSlot(currentHighlight);
            if (delayBetweenItemUsages <= 0)
                UseSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DeHighlightSlot(currentHighlight);
            currentHighlight = 1;
            SetHighlightedSlot(currentHighlight);
            if (delayBetweenItemUsages <= 0)
                UseSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            DeHighlightSlot(currentHighlight);
            currentHighlight = 2;
            SetHighlightedSlot(currentHighlight);
            if (delayBetweenItemUsages <= 0)
                UseSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            DeHighlightSlot(currentHighlight);
            currentHighlight = 3;
            SetHighlightedSlot(currentHighlight);
            if (delayBetweenItemUsages <= 0)
                UseSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            DeHighlightSlot(currentHighlight);
            currentHighlight = 4;
            SetHighlightedSlot(currentHighlight);
            if (delayBetweenItemUsages <= 0)
                UseSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            DeHighlightSlot(currentHighlight);
            currentHighlight = 5;
            SetHighlightedSlot(currentHighlight);
            if (delayBetweenItemUsages <= 0)
                UseSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            DeHighlightSlot(currentHighlight);
            currentHighlight = 6;
            SetHighlightedSlot(currentHighlight);
            if (delayBetweenItemUsages <= 0)
                UseSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            DeHighlightSlot(currentHighlight);
            currentHighlight = 7;
            SetHighlightedSlot(currentHighlight);
            if (delayBetweenItemUsages <= 0)
                UseSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            DeHighlightSlot(currentHighlight);
            currentHighlight = 8;
            SetHighlightedSlot(currentHighlight);
            if (delayBetweenItemUsages <= 0)
                UseSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            DeHighlightSlot(currentHighlight);
            currentHighlight = 9;
            SetHighlightedSlot(currentHighlight);
            if (delayBetweenItemUsages <= 0)
                UseSelectedItem();
        }
       */


    }
    public void UseSelectedItem()
    {
        if (Items[currentHighlight]==null)
        {
            return;
        }
        if (Items[currentHighlight] && ItemAmounts[currentHighlight]>0)
        {
            if (Items[currentHighlight].type == ItemData.ItemType.consumable)
            {


                ItemAmounts[currentHighlight] -= 1;
                
                if(CombatPlayerManager.instance)
                {
                    CombatPlayerManager.instance.HealPlayer(Items[currentHighlight].consumeHealthValue);
                    CombatPlayerManager.instance.RestorePlayerMana(Items[currentHighlight].consumeManaValue);
                }
                if (ItemAmounts[currentHighlight] == 0)
                {
                    Items[currentHighlight] = null;
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
    public bool AddItemToHotbar(ItemData data_,int amount=1)
    {
        for(int i=0;i<mySlots.Count;i++)
        {
            if (!Items[i])
            {
                continue;
            }
            if (Items[i].itemName==data_.itemName)
            {
                Items[i] = data_;
                ItemAmounts[i] = amount + int.Parse(mySlots[i].amountText.text);
                mySlots[i].AddItem(data_.itemSprite, amount+int.Parse(mySlots[i].amountText.text), Items[i].itemColor);
                return true;
            }
        }
        for (int i = 0; i < mySlots.Count; i++)
        {
            if (!Items[i])
            {
                Items[i] = data_;
                ItemAmounts[i] = amount;
                mySlots[i].AddItem(data_.itemSprite, amount, Items[i].itemColor);
                return true;
            }
        }
        return false;
    }
}
