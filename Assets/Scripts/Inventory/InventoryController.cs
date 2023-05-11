using System;
using System.Collections;
using System.Collections.Generic;
using System.Storage;
using float_oat.Desktop90;
using Inventory.Items;
using Loot;
using Loot.Quantity;
using Saving;
using UnityEngine;

namespace Inventory
{
    /// 
    /// Class for controlling inventory. 
    /// 
    public class InventoryController : MonoBehaviour
    {
        private const string FILE_PATH = "/Inventory/InventorySaveData.json";

        #region Storage
        
            [SerializeField] private Storage<ItemSaveData> inventoryStorage;
            private InventorySaveLoad storageFunctions = new InventorySaveLoad(); // stores save/load functionality
        
        #endregion
        #region External Component Fields
        [Header("External Component Fields")]
        /// 
        /// These variables are defined in the unity inspector
        /// 
            [SerializeField] private RectTransform inventoryPanel;
            [SerializeField] private GameObject itemPrefab;
            [SerializeField] internal LootWindowController lootWindowController;
            [SerializeField] internal QuantityWindowController quantityWindowController;
            [SerializeField] private SingleItemPanelController singleItemPanel;
            [SerializeField] private WindowController inventoryWindowController;
        #endregion

        public SingleItemPanelController SingleItemPanel { get => singleItemPanel; set => singleItemPanel = value; }

        #region Unity Default Functions

        void Awake() 
        {
            try
            { // load inventory & items from inventory
                inventoryStorage = storageFunctions.LoadFromJson(FILE_PATH);
                if(inventoryStorage == null) inventoryStorage = new Storage<ItemSaveData>(5, 7); // create new inventory if nothing is loaded

                foreach(List<ItemSaveData> xLists in inventoryStorage.storage)
                { // loading each item from inventory storage
                    foreach(ItemSaveData item in xLists)
                    {
                        if(item == null) continue;
                        CreateItemOnInitialize(item);
                    }
                }
            }
            catch (System.Exception exception)
            { // if inventory load doesnt work, create new inventory and save
                Debug.Log(exception);
                inventoryStorage = new Storage<ItemSaveData>(5, 7);
                storageFunctions.SaveToJson(inventoryStorage, FILE_PATH);
            }
            inventoryWindowController.Close();
        }
        private void OnApplicationQuit() 
        {
            storageFunctions.SaveToJson(inventoryStorage, FILE_PATH);
        }

        #endregion
        #region Inventory Management Methods

        public void RemoveAt(Vector2Int slot)
        { // for removing quickly from inventoryStorage, called in ItemController
            inventoryStorage.RemoveAt(slot);
        }
        public void AddItem(ItemSaveData item)
        {
            if(item == null) return;
            try
            { // check if item already exists & update quantity
                ItemSaveData inventoryItem = inventoryStorage.GetItemSearch(item);
                inventoryItem.Quantity += item.Quantity;
                inventoryStorage.UpdateItem(inventoryItem, inventoryItem.InventorySlot);
                // add item quantity to literal object in game
                GetRowColumnLiteral(inventoryItem.InventorySlot).GetChild(0).GetComponent<ItemController>().AddQuantity(item.Quantity);
            }
            catch (System.ItemNotFoundException)
            { // item isn't in inventory yet
                try
                { // try adding the item to inventory
                    item.InventorySlot = inventoryStorage.GetNextEmpty();
                    inventoryStorage.InsertAt(item, item.InventorySlot);
                    CreateItemOnInitialize(item);
                }
                catch (System.InventoryIsFullException)
                { // can't add item to inventory
                    throw;
                }
            }

            storageFunctions.SaveToJson(inventoryStorage, FILE_PATH); // save storage after function is called to keep file updated
        }
        public void TakeItem(ItemSaveData item)
        { 
            try
            { // break if item isn't in inventory / isn't enough to prevent taking item player doesn't have
                ItemSaveData inventoryItem = inventoryStorage.GetItemSearch(item);

                if(inventoryItem.Quantity < item.Quantity) throw new ItemQuantityRequestedLessThanStoredException();

                inventoryItem.Quantity -= item.Quantity;
                if(inventoryItem.Quantity == 0)
                { // all of the item is taken
                    RemoveAt(inventoryItem.InventorySlot); 
                    GetRowColumnLiteral(inventoryItem.InventorySlot).GetComponentInChildren<ItemController>().DeleteItem();
                }
                else
                { // only some of item is taken
                    GetRowColumnLiteral(inventoryItem.InventorySlot).GetComponentInChildren<ItemController>().RemoveQuantity(item.Quantity);
                    inventoryStorage.UpdateItem(inventoryItem, inventoryItem.InventorySlot);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            storageFunctions.SaveToJson(inventoryStorage, FILE_PATH); // keep inventory updated
        }
        public void TakeAllItem(ItemSaveData item)
        { // takes all of a specific item in a specific slot
            List<ItemSaveData> updateItemList = inventoryStorage.GetAllItemsSearch(item); // get list of items item could be
            ItemSaveData updateItem = null;
            
            foreach(ItemSaveData items in updateItemList)
            {
                if(item.InventorySlot.Equals(items.InventorySlot))
                {
                    updateItem = items;
                    break;
                }
            }
            RemoveAt(updateItem.InventorySlot);
            GetRowColumnLiteral(updateItem.InventorySlot).GetComponentInChildren<ItemController>().DeleteItem();

            storageFunctions.SaveToJson(inventoryStorage, FILE_PATH); // keep inventory updated
        }
        public void UpdateItemFromController(ItemSaveData item, Vector2Int coords)
        { // called by item controller
            try
            {
                List<ItemSaveData> updateItemList = inventoryStorage.GetAllItemsSearch(item);
                ItemSaveData updateItem = null;
                foreach(ItemSaveData items in updateItemList)
                {
                    if(item.InventorySlot.Equals(items.InventorySlot))
                    {
                        updateItem = items;
                        break;
                    }
                }
                updateItem.Quantity = item.Quantity;
                if(updateItem.Quantity == 0)
                { // remove item if new quantity is 0
                    RemoveAt(updateItem.InventorySlot);
                    return;
                }
                if(!updateItem.InventorySlot.Equals(coords))
                { // switch slot if not in new slot
                    RemoveAt(updateItem.InventorySlot); // remove at og slot to prevent duplicate
                    updateItem.InventorySlot = coords;
                    inventoryStorage.InsertAt(updateItem, updateItem.InventorySlot);
                    return;
                }
                inventoryStorage.UpdateItem(updateItem, updateItem.InventorySlot);
            }
            catch (System.Exception)
            {
                throw;
            }
            storageFunctions.SaveToJson(inventoryStorage, FILE_PATH);
        }

        #endregion
        #region Misc Functions

        public void CreateItemOnInitialize(ItemSaveData item)
        {
            GameObject tempItem = Instantiate(itemPrefab);

            ItemController tempItemController = tempItem.GetComponent<ItemController>();
            tempItemController.SetParentAndResetPosition( GetRowColumnLiteral(item.InventorySlot) );   
            tempItemController.inventoryController = this;
            tempItemController.windowGlobal = this.gameObject; 
            tempItemController.LoadItem(item, GetRowColumnLiteral(item.InventorySlot).gameObject);
        }
        public Transform GetRowColumnLiteral(Vector2Int slot)
        { // gets the row/column from the inventory ui
            return inventoryPanel.GetChild(slot.y).GetChild(slot.x);
        }
        
        #endregion
    }
}
