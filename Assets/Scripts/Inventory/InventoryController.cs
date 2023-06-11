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

            private Storage<Item> inventoryStorage;
        
        #endregion
        #region External Component Fields
        [Header("External Component Fields")]
        /// 
        /// These variables are defined in the unity inspector
        /// 
            [SerializeField] private GameObject itemPrefab;
            [SerializeField] private GameObject itemContainer;
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
                inventoryStorage = LoadInventory();
            }
            catch (System.Exception exception)
            { // if inventory load doesnt work, create new inventory and save
                Debug.Log(exception);
                inventoryStorage = new Storage<Item>();
                SaveInventory();
            }
            InitializeItemBlerbs();
            inventoryWindowController.Close();
        }
        private void OnApplicationQuit() 
        {
            SaveInventory();
        }

        private void InitializeItemBlerbs()
        {
            foreach(Transform child in itemContainer.transform)
            {
                Destroy(child.gameObject);
            }
            foreach(Item item in inventoryStorage.storage)
            {
                CreateBlerb(item);
            }
        }
        private void CreateBlerb(Item item)
        {
            GameObject itemClone = Instantiate(itemPrefab);
            itemClone.transform.SetParent(itemContainer.transform);

            ItemBlerbController itemCloneController = itemClone.GetComponent<ItemBlerbController>();
            itemCloneController.InitializeBlerb(item, Resources.Load<ItemPrototype>("Items/" + item.Name), this);
        }
        #endregion
        #region Inventory Management Methods

        /// Methods for managing the inventory 
        /// 
        /// ! IMPORTANT NOTE ! 
        /// ! For the functions where an object of the type "Item" is 
        /// ! passed, these objects are often "requests" for items, and 
        /// ! not a saved, real instance of an item. As such, the quantities
        /// ! on these items are instead requests for that amount.

        /// 
        /// For adding a new item to the inventory storage. If the 
        /// item already exists, add the quantity instead.
        /// 
        public void AddItem(Item item)
        {
            try
            {
                inventoryStorage.Create(item);
                CreateBlerb(inventoryStorage.Search(item)); // confirms its the inventory's item
            }
            catch (System.Exception exception)
            { // item already exists, update instead
                Debug.LogError(exception);
                Item updateItem = inventoryStorage.Search(item);
                updateItem.Quantity += item.Quantity;
                inventoryStorage.Update(updateItem);
            }
            SaveInventory();
        }
        public void TakeItem(Item item)
        { 
            Item takenItem = inventoryStorage.Search(item);
            if(takenItem.Quantity < item.Quantity) throw new ItemQuantityRequestedLessThanStoredException();

            takenItem.Quantity -= item.Quantity; // take an amount of the item
            if(takenItem.Quantity == 0)
            {
                inventoryStorage.Delete(takenItem);
                return;
            }
            inventoryStorage.Update(takenItem);
            SaveInventory();
        }
        public int GetItemAmount(Item item)
        {
            Item itemToRead = inventoryStorage.Search(item);

            return itemToRead.Quantity; 
        }
        public Item GetItem(Item item)
        {
            return inventoryStorage.Search(item);
        }
        public void RemoveItem(Item item)
        {
            inventoryStorage.Delete(item);
            SaveInventory();
        }
        
        #endregion
        
        private void SaveInventory()
        {
            SaveLoad<Storage<Item>>.SaveToJson(inventoryStorage, FILE_PATH);
        }
        private Storage<Item> LoadInventory()
        {
            return SaveLoad<Storage<Item>>.LoadFromJson(FILE_PATH);
        }

        public void Open()
        {
            InitializeItemBlerbs();
            inventoryWindowController.Open();
        }
        public void Close()
        {
            inventoryWindowController.Close();
        }
    }
}
