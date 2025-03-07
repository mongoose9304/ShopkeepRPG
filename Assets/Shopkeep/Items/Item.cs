using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Shopkeeper/Item", order = 1)]
    public class Item : ScriptableObject
    {
        [SerializeField]
        private Sprite itemImage;
    }

    [System.Serializable]
    public struct ItemStack
    {
        public Shopkeeper.Item item;
        public int amount;

        public ItemStack(Shopkeeper.Item _item, int _amount)
        {
            item = _item;
            amount = _amount;
        }
    }
}
