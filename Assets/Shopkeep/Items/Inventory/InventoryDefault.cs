using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    public class InventoryDefault : Inventory
    {
        public int money = 0;
        public List<ItemStack> items;
        public override int GetMoney(){  return money; }

        public override void AddMoney(int amountMoney){ money += amountMoney; }

        public override void RemoveMoney(int amountMoney){ money -= amountMoney; }

        public override ItemStack GetItemStack(string name)
        {
            foreach (ItemStack itemStack in items) 
            {
                if(itemStack.item.name == name) 
                {
                    return itemStack;
                }
            }

            throw new Exception($"ItemStack with name '{name}' not found!");
        }

        public override void AddItemStack(ItemStack itemStack)
        {
            //first check if itemstack already exists in inventory
            for (int i = 0; i< items.Count; i++)
            {
                if (items[i].item.name == itemStack.item.name) 
                {
                    ItemStack newStack = items[i];
                    newStack.amount += itemStack.amount;
                    items[i] = newStack;
                    return;
                }
            }
            //if not in inventory, then we add it
            items.Add(itemStack);
        }

        public override void RemoveItemStack(ItemStack itemStack)
        {
            //first check if itemstack already exists in inventory
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item.name == itemStack.item.name)
                {
                    ItemStack newStack = items[i];
                    newStack.amount -= itemStack.amount;

                    //if amount is not positive remove it completely
                    if (newStack.amount <= 0)
                    {
                        items.RemoveAt(i);
                        return;
                    }
                    //otherwise simply modify it
                    items[i] = newStack;
                    return;
                }
            }

            throw new Exception($"ItemStack with name '{itemStack.item.name}' not found!");
        }
    }
}
