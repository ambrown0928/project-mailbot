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
    public class InventoryController : MonoBehaviour
    {
        private const string FILE_PATH = "/Inventory/InventorySaveData.json";

        [SerializeField] private Storage<ItemSaveData> inventoryStorage;
        [SerializeField] private RectTransform inventoryPanel;
        [SerializeField] private SingleItemPanelController singleItemPanel;
        [SerializeField] private WindowController inventoryWindowController;
        [SerializeField] private GameObject itemPrefab;

        [SerializeField] internal LootWindowController lootWindowController;
        [SerializeField] internal QuantityWindowController quantityWindowController;
        private InventorySaveLoad storageFunctions = new InventorySaveLoad();

        public SingleItemPanelController SingleItemPanel { get => singleItemPanel; set => singleItemPanel = value; }

        void Awake() 
        {
            try
            {
                inventoryStorage = storageFunctions.LoadFromJson(FILE_PATH);
                if(inventoryStorage == null) inventoryStorage = new Storage<ItemSaveData>(5, 7);
                foreach(List<ItemSaveData> xLists in inventoryStorage.storage)
                {
                    foreach(ItemSaveData item in xLists)
                    {
                        if(item == null) continue;
                        CreateItemOnInitialize(item);
                    }
                }
            }
            catch (System.Exception)
            {
                inventoryStorage = new Storage<ItemSaveData>(5, 7); // TODO - implement save/load functionality
                storageFunctions.SaveToJson(inventoryStorage, FILE_PATH);
            }
            inventoryWindowController.Close();
        }
        public void RemoveAt(Vector2Int slot)
        {
            inventoryStorage.RemoveAt(slot);
        }
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

        public void AddItem(ItemSaveData item)
        {
            if(item == null) return;
            try
            { // check if item already exists & update quantity
                ItemSaveData inventoryItem = inventoryStorage.GetItemSearch(item);
                inventoryItem.Quantity += item.Quantity;
                inventoryStorage.UpdateItem(inventoryItem, inventoryItem.InventorySlot);
                GetRowColumnLiteral(inventoryItem.InventorySlot).GetChild(0).GetComponent<ItemController>().AddQuantity(item.Quantity);
            }
            catch (System.ItemNotFoundException)
            {
                try
                { // try adding the item to inventory
                    item.InventorySlot = inventoryStorage.GetNextEmpty();
                    Debug.Log(item.InventorySlot.x);
                    Debug.Log(item.InventorySlot.y);
                    inventoryStorage.InsertAt(item, item.InventorySlot);
                    CreateItemOnInitialize(item);
                }
                catch (System.InventoryIsFullException)
                {
                    throw;
                }
            }   
            storageFunctions.SaveToJson(inventoryStorage, FILE_PATH);
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
                storageFunctions.SaveToJson(inventoryStorage, FILE_PATH);
            }
            catch (System.Exception)
            {
                throw;
            }
            
        }
        public void TakeAllItem(ItemSaveData item)
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
            inventoryStorage.RemoveAt(updateItem.InventorySlot);
            GetRowColumnLiteral(updateItem.InventorySlot).GetComponentInChildren<ItemController>().DeleteItem();
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
                    inventoryStorage.RemoveAt(updateItem.InventorySlot);
                    return;
                }
                if(!updateItem.InventorySlot.Equals(coords))
                {
                    inventoryStorage.RemoveAt(updateItem.InventorySlot);
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
        }
        private void OnApplicationQuit() 
        {
            storageFunctions.SaveToJson(inventoryStorage, FILE_PATH);
        }
    }
}
