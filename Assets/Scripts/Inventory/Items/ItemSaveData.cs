using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///
/// Class for storing items after use.
/// 
namespace Inventory.Items
{
    [System.Serializable]
    public class ItemSaveData
    {
        private Item itemData;
        private float quantity;
        private Vector2 inventorySlot;

        public ItemSaveData(Item itemData, float quantity, Vector2 inventorySlot)
        {
            this.itemData = itemData;
            this.quantity = quantity;
            InventorySlot = inventorySlot;
        }

        public Item ItemData { get => itemData; }
        public float Quantity { get => quantity; set => quantity = value; }
        public Vector2 InventorySlot { get => inventorySlot; set => inventorySlot = value; }
    }
}
