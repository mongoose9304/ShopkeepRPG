using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    public abstract class Inventory : MonoBehaviour
    {
        public abstract int GetMoney();
        public abstract void AddMoney(int amountMoney);
        public abstract void RemoveMoney(int amountMoney);

        public abstract Shopkeeper.ItemStack GetItemStack(string name);
        public abstract void AddItemStack(Shopkeeper.ItemStack itemStack);
        public abstract void RemoveItemStack(Shopkeeper.ItemStack itemStack);
    }
}
