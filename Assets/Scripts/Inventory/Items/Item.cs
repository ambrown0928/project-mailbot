using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
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
        [JsonIgnore] private ItemPrototype itemPrototype;
        [SerializeField] private string name;
        [SerializeField] private int quantity;
        [SerializeField] private int index;

        public int Quantity { get => quantity; set => quantity = value; }
        public string Name { get => name; set => name = value; }
        [JsonIgnore] public ItemPrototype ItemPrototype { get => itemPrototype;}
        public int Index { get => index; set => index = value; }

        #region Constructors

        public Item() { } // empty constructor
            public Item(Item item)
            {
                itemPrototype = Resources.Load<ItemPrototype>("Items/" + item.Name); // checks the item is valid // TODO - Replace with AssetBundle
                this.name = item.Name;
                this.quantity = item.Quantity;
                this.index = item.Index;
            }
            public Item(ItemPrototype itemPrototype, int quantity)
            { // load from item prototype
                this.itemPrototype = itemPrototype;
                this.name = itemPrototype.name;
                this.quantity = quantity;
            }
            public Item(string itemName, int quantity)
            { // create new item instance
                itemPrototype = Resources.Load<ItemPrototype>("Items/" + itemName); // checks the item is valid // TODO - Replace with AssetBundle
                this.name = itemPrototype.name;
                this.quantity = quantity;
            }

        #endregion

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
