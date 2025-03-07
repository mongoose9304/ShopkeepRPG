using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    public class GlobalPlayerInventoryReference : InventoryReference
    {
        private void Start()
        {
            inventory = ManagerGlobal.GetManager<GlobalPlayerInventory>();
        }
    }
}