using System;
using System.Collections;
using System.Collections.Generic;
using System.Storage;
using System.UI;
using float_oat.Desktop90;
using Inventory.Items;
using Loot;
using Loot.Quantity;
using Saving;
using UnityEngine;

namespace Inventory
{
    /// 
    /// Class for controlling the inventory. Controls both the literal,
    /// GameObject UI Element seen by the player, and the data to save
    /// and load the inventory, and interact with the items.
    /// 
    public class InventoryController : MonoBehaviour
    {
        private const string FILE_PATH = "/Inventory/InventorySaveData.json";

        #region Storage

            private Storage<Item> inventoryStorage; // the inventory, represented in data
        
        #endregion
        #region External Component Fields
        [Header("External Component Fields")]
        /// 
        /// These variables are defined in the unity inspector. The first holds
        /// templates to create new item blerbs in the item container, which
        /// is the inventory list. The other 4 hold controllers for various 
        /// other UI windows.
        /// 
            [SerializeField] private GameObject itemPrefab;
            [SerializeField] private GameObject itemContainer;

            [SerializeField] private UIControllerGlobalContainer uIControllerGlobalContainer;
            [SerializeField] private WindowController inventoryWindowController; // opens and closes window

        #endregion

        #region Unity Default Functions

        void Awake() 
        {
            try
            { // load inventory & items from inventory
                inventoryStorage = new Storage<Item>();
                Storage<Item> tempStorage = LoadInventory();
                foreach(Item item in tempStorage.storage)
                {
                    Item tempItem = new Item(item); // create new item with itemPrototype
                    inventoryStorage.Create(tempItem);
                }
                InitializeItemBlerbs();
            }
            catch (System.Exception exception)
            { // if inventory load doesnt work, create new inventory and save
                Debug.LogError(exception);
                inventoryStorage = new Storage<Item>();
                SaveInventory();
            }
            inventoryWindowController.Close();
        }
        private void OnApplicationQuit() 
        {
            SaveInventory();
        }

        internal void InitializeItemBlerbs()
        {
            ResetList();
            int index = 0;
            foreach (Item item in inventoryStorage.storage) 
            { 
                CreateBlerb(item, index); 
                index = index + 1; 
            }
        }

        private void ResetList()
        {
            foreach (Transform child in itemContainer.transform) Destroy(child.gameObject);
        }

        private void CreateBlerb(Item item, int index)
        {
            // create new item blerb and put it in the inventory
            GameObject itemClone = Instantiate(itemPrefab);
            itemClone.transform.SetParent(itemContainer.transform);
            itemClone.transform.SetSiblingIndex(index);

            // initialize the item blerb controller, passing necessary controllers just in case
            ItemBlerbController itemCloneController = itemClone.GetComponent<ItemBlerbController>();
            Debug.Log(index);
            itemCloneController.InitializeBlerb( item, uIControllerGlobalContainer, index );
        }
        #endregion
        #region Inventory Management Methods
        /// Methods for managing the inventory 
        /// 
        /// ! IMPORTANT NOTE !
        /// For the functions where an object of the type "Item" is 
        /// passed, these objects are often "requests" for items, and 
        /// not a saved, real instance of an item. As such, the quantities
        /// on these items are instead requests for that amount.
        /// Also, when refer
        ///
        /// For adding a new item to the inventory storage. If the 
        /// item already exists, add the quantity instead.
        /// 
        public void AddItem(Item item)
        {
            try
            {
                inventoryStorage.Create(item);
                CreateBlerb(inventoryStorage.Search(item), inventoryStorage.storage.Count); // confirms its the inventory's item
            }
            catch (System.ItemAlreadyExistsException)
            { // item already exists, update instead
                Item updateItem = inventoryStorage.Search(item);
                updateItem.Quantity += item.Quantity;
                inventoryStorage.Update(updateItem);
            }
            catch (System.Exception exception)
            {
                Debug.LogError(exception);
            }

            SaveInventory();
        }
        public void AddItem( int index, Item item )
        {
            try
            {
                inventoryStorage.Insert(index, item);
                CreateBlerb(inventoryStorage.Search(item), index); // confirms its the inventory's item
            }
            catch (System.ItemAlreadyExistsException)
            { // item already exists, update instead
                Item updateItem = inventoryStorage.Search(item);
                updateItem.Quantity += item.Quantity;
                inventoryStorage.Update(updateItem);
            }
            catch (System.Exception exception)
            {
                Debug.LogError(exception);
            }

            SaveInventory();
        }
        public void TakeItem(Item item)
        { // try to take an item from the inventory
            Item takenItem = inventoryStorage.Search(item);
            // item.Quantity = amount requested to be taken
            if(takenItem.Quantity < item.Quantity) throw new ItemQuantityRequestedLessThanStoredException();

            takenItem.Quantity -= item.Quantity; // take an amount of the item
            if(takenItem.Quantity == 0)
            { // all of item is taken, remove from inventory
                RemoveItem(takenItem);
                return;
            }
            inventoryStorage.Update(takenItem);
            SaveInventory();
        }
        public bool CanTakeItem(Item item)
        {
            bool canTake = false;
            Item takenItem = inventoryStorage.Search(item);

            if(takenItem.Quantity >= item.Quantity) canTake = true;
            return canTake;
        }
        public Item GetItemAt(int index)
        {
            return inventoryStorage.Read(index);
        }
        public Item GetItem(Item item)
        {
            return inventoryStorage.Search(item);
        }
        public void RemoveItem(Item item)
        {
            FindAndDeleteItemGameObject(item);
            inventoryStorage.Delete(item); // delete item in inventory (data)
            SaveInventory();
        }
        public void RemoveItemAt(int index)
        {
            int currentIndex = 0;
            foreach(Transform child in itemContainer.transform)
            {

                if(currentIndex == index)
                { // delete the item in the inventory (literal)
                    Destroy(child.gameObject);
                    return;
                }
                currentIndex++;
            }
            inventoryStorage.DeleteIndex(index);
        }
        public void FindAndDeleteItemGameObject(Item item)
        { // finds the GameObject representing the item in the inventory and deletes it, along with the item
            foreach(Transform child in itemContainer.transform)
            {
                ItemBlerbController childController = child.GetComponent<ItemBlerbController>();

                if(childController.Item.Equals(item))
                { // delete the item in the inventory (literal)
                    Destroy(child.gameObject);
                    return;
                }
            }
            throw new ItemNotFoundException(); // item not in inventory
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
        private void OnDisable()
        { // reset the inventory storage and add everything in the corrent new order
            Storage<Item> tempStorage = new Storage<Item>(inventoryStorage.storage.Count);
            foreach(Item item in inventoryStorage.storage)
            { 
                tempStorage.Insert(item.Index, item); // insert at index saved by item
            }
            inventoryStorage = new Storage<Item>();
            foreach(Item item in tempStorage.storage)
            {
                if(item == null) continue;
                
                inventoryStorage.Create(item);
            }
            SaveInventory();
        }
    }
}
