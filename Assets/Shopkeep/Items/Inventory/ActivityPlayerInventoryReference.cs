using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    public class ActivityPlayerInventoryReference : InventoryReference
    {
        private void Start()
        {
            inventory = ManagerActivity.GetManager<ActivityPlayerInventory>();
        }
    }
}