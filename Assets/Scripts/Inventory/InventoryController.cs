using System;
using System.Collections;
using System.Collections.Generic;
using System.Storage;
using Inventory.Items;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        private Storage<ItemSaveData> inventoryStorage;
        [SerializeField] private RectTransform inventoryPanel;
        [SerializeField] private SingleItemPanelController singleItemPanel;
        
        [SerializeField] private GameObject itemPrefab;

        public SingleItemPanelController SingleItemPanel { get => singleItemPanel; set => singleItemPanel = value; }

        void Awake() 
        {
            inventoryStorage = new Storage<ItemSaveData>(5, 7); // TODO - implement save/load functionality
        }
        
        public void CreateItemOnInitialize(ItemSaveData item)
        {
            GameObject tempItem = Instantiate(itemPrefab);
            ItemController tempItemController = tempItem.GetComponent<ItemController>();
            tempItemController.LoadItem(item);
            tempItemController.SetParentAndResetPosition( GetRowColumnLiteral(item.InventorySlot) );    
        }

        public Transform GetRowColumnLiteral(Vector2 slot)
        { // gets the row/column from the inventory ui
            return inventoryPanel.GetChild((int)slot.x).GetChild((int)slot.y);
        }

        public void AddItem(Item item, int quantity)
        {
            Vector2 slot = new Vector2(0, 0);
            ItemSaveData newItem = new ItemSaveData(item, quantity, slot);
            try
            { // check if item already exists & update quantity
                ItemSaveData inventoryItem = inventoryStorage.GetItemSearch(newItem);
                inventoryItem.Quantity += quantity;
                inventoryStorage.UpdateItem(inventoryItem, inventoryItem.InventorySlot);
            }
            catch (System.ItemNotFoundException)
            {
                try
                { // try adding the item to inventory
                    newItem.InventorySlot = inventoryStorage.GetNextEmpty();
                    inventoryStorage.InsertAt(newItem, slot);
                    CreateItemOnInitialize(newItem);
                }
                catch (System.InventoryIsFullException)
                {
                    throw;
                }
            }   
        }
        public void TakeItem(ItemSaveData item)
        {
            try
            {
                ItemSaveData inventoryItem = inventoryStorage.GetItemSearch(item);
                if(inventoryItem.Quantity < item.Quantity) throw new ItemQuantityRequestedLessThanStoredException(); 
                inventoryItem.Quantity -= item.Quantity;
                if(inventoryItem.Quantity == 0)
                {
                    inventoryStorage.RemoveAt(inventoryItem.InventorySlot);
                    GetRowColumnLiteral(inventoryItem.InventorySlot).GetComponentInChildren<ItemController>().DeleteItem();
                }
                else
                {
                    GetRowColumnLiteral(inventoryItem.InventorySlot).GetComponentInChildren<ItemController>().RemoveQuantity(item.Quantity);
                    inventoryStorage.UpdateItem(inventoryItem, inventoryItem.InventorySlot);
                }
            }
            catch (System.ItemNotFoundException)
            {
                throw;
            }
        }
        public void UpdateItemFromController(ItemSaveData item)
        {
            try
            {
                ItemSaveData updateItem = inventoryStorage.GetItemSearch(item);
                updateItem.Quantity = item.Quantity;
                if(!updateItem.InventorySlot.Equals(item.InventorySlot))
                {
                    inventoryStorage.RemoveAt(updateItem.InventorySlot);
                    updateItem.InventorySlot = item.InventorySlot;
                }
                inventoryStorage.InsertAt(updateItem, updateItem.InventorySlot);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
