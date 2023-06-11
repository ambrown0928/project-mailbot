using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///
/// Class for storing items after use / transferring item information
/// across files and classes. Also used to "request" items 
/// 
namespace Inventory.Items
{
    [System.Serializable]
    public class Item
    {
        [SerializeField] private string name;
        [SerializeField] private int quantity;

        public int Quantity { get => quantity; set => quantity = value; }
        public string Name { get => name; set => name = value; }

        public Item()
        {
        }

        public Item(ItemPrototype itemPrototype, int quantity)
        {
            this.name = itemPrototype.name;
            this.quantity = quantity;
        }

        public Item(string itemName, int quantity)
        {
            try
            {
                ItemPrototype itemPrototype = Resources.Load<ItemPrototype>("Items/" + itemName); // TODO - Replace with AssetBundle
                this.name = itemPrototype.name;
                this.quantity = quantity;
            }
            catch (System.Exception exception)
            {
                Debug.LogError(exception);
                throw;
            }
        }

        public override bool Equals(object obj)
        {
            if (this == null && obj == null) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return ( (Item) obj).name == name; // only care that item is the same in name alone
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(name, quantity);
        }
    }
}
