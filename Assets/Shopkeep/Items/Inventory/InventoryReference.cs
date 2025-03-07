using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    public class InventoryReference : Inventory
    {
        public Inventory inventory;

        public override int GetMoney() { return inventory.GetMoney(); }

        public override void AddMoney(int amountMoney) { inventory.AddMoney(amountMoney); }

        public override void RemoveMoney(int amountMoney) { inventory.RemoveMoney(amountMoney); }

        public override ItemStack GetItemStack(string name) { return inventory.GetItemStack(name); }

        public override void AddItemStack(ItemStack itemStack){ inventory.AddItemStack(itemStack); }

        public override void RemoveItemStack(ItemStack itemStack) {  inventory.RemoveItemStack(itemStack); }
        
    }
}

