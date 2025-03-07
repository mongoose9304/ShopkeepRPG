using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    public static class ItemDatabase
    {
        public static Item GetItem(string name)
        {
            return Resources.Load<Item>("Items/" + name);
        }
    }
}
