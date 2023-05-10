using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///
/// Class for storing items after use / transferring item information
/// across files and classes.
/// 
namespace Inventory.Items
{
    [System.Serializable]
    public class ItemSaveData
    {
        public string name;
        [SerializeField] private int quantity;
        [SerializeField] private Vector2Int inventorySlot;

        public ItemSaveData()
        {
        }

        public ItemSaveData(Item itemData, int quantity, Vector2Int inventorySlot)
        {
            this.name = itemData.name;
            this.quantity = quantity;
            InventorySlot = inventorySlot;
        }

        public ItemSaveData(string itemName, int quantity)
        {
            try
            {
                Item itemData = Resources.Load<Item>("Items/" + itemName);
                this.name = itemData.name;
                this.quantity = quantity;
                InventorySlot = new Vector2Int(0, 0);
            }
            catch (System.Exception exception)
            {
                Debug.LogError(exception);
                throw;
            }
        }

        public int Quantity { get => quantity; set => quantity = value; }
        public Vector2Int InventorySlot { get => inventorySlot; set => inventorySlot = value; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return ((ItemSaveData)obj).name == name; // only care that item is the same in name alone
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(name, quantity, inventorySlot, Quantity, InventorySlot);
        }
    }
}
