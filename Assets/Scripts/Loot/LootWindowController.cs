using System;
using System.Collections;
using System.Collections.Generic;
using float_oat.Desktop90;
using Inventory;
using Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Loot
{
    public class LootWindowController : MonoBehaviour, IPointerEnterHandler
    {
        #region Controller Fields
        [Header("Controller Fields")]

            [SerializeField] private WindowController windowController;
            [SerializeField] private InventoryController inventoryController;
            [SerializeField] private Text windowTitle;

        #endregion
        #region Game Object Fields
        [Header("GameObject Fields")]

            [SerializeField] private GameObject lootBlerbPrefab;
            [SerializeField] private GameObject lootListWindow;
            [SerializeField] private GameObject currentItem;

        #endregion

        private LootReporter lootReporter;
        private bool isPackage;

        private List<ItemSaveData> itemsToLoot;
        public List<ItemSaveData> ItemsToLoot { 
            get => itemsToLoot; 
            set 
            {
                itemsToLoot = value;
                NewLoot(itemsToLoot);
            } 
        }

        private void Awake() 
        {
            lootReporter = new LootReporter();
            windowController.Close();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            // checks
            if (ItemNotBeingDragged(eventData)) return;
            if(isPackage && currentItem != null) return;

            currentItem = eventData.pointerDrag.gameObject;
            currentItem.GetComponent<ItemController>().ChangeSelectedItemPotentialSlot(gameObject);
        }
        private bool ItemNotBeingDragged(PointerEventData eventData)
        {
            return eventData.pointerDrag == null;
        }

        public void AddLootItem(ItemSaveData item)
        {
            lootReporter.AddItem(item);
            itemsToLoot.Add(item);
            CreateBlerb(item);
        }
        public void RemoveLootItem(ItemSaveData itemToRemove)
        {
            currentItem = null; // removes current item so package can add new item
            itemsToLoot.Remove(itemToRemove);
            ItemSaveData tempItemToPass = new ItemSaveData(itemToRemove.name, 0);
            lootReporter.RemoveItem(tempItemToPass);
            if(itemsToLoot.Count == 0 && !isPackage)
            { // keep window open if package so player can add items
                windowController.Close();
            }
        }
        public void LootAll()
        {
            foreach(Transform child in lootListWindow.transform)
            {
                child.GetComponent<LootBlerbController>().Loot();
            }
            windowController.Close();
        }
        
        public LootReporter OpenLootWindow(List<ItemSaveData> loot, string title)
        { // called by auto-generating loot enemies
            if(windowController.gameObject.activeInHierarchy) throw new LootWindowIsOpenException();
            isPackage = false;
            ItemsToLoot = loot;

            windowTitle.text = title;
            return lootReporter;
        }
        public LootReporter OpenLootWindow(ItemSaveData loot, string title)
        {
            if(windowController.gameObject.activeInHierarchy) throw new LootWindowIsOpenException();

            isPackage = true;
            if(loot.name == "" || loot.Quantity == 0) ItemsToLoot = new List<ItemSaveData>(); // create an empty list if pkg is empty
            else ItemsToLoot = new List<ItemSaveData>{loot}; 
            
            windowTitle.text = title;
            return lootReporter;
        }
        public void NewLoot(List<ItemSaveData> itemsToLoot)
        {
            ResetList();
            windowController.Open();
            foreach(ItemSaveData item in itemsToLoot) CreateBlerb(item);
        }
        public void ResetList()
        {
            foreach(Transform child in lootListWindow.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        private void CreateBlerb(ItemSaveData item)
        {
            // instantiate gameobject and setup requirements
            GameObject newLootBlerb = Instantiate(lootBlerbPrefab);
            newLootBlerb.transform.SetParent(lootListWindow.transform, false);
            newLootBlerb.transform.SetAsFirstSibling();

            if(isPackage) currentItem = newLootBlerb;

            // setup loot blerb controller
            LootBlerbController newLootBlerbController = newLootBlerb.GetComponent<LootBlerbController>();
            newLootBlerbController.InventoryController = inventoryController;
            newLootBlerbController.LootWindowController = this;
            newLootBlerbController.LoadLoot(item);
        }

        void OnDisable()
        {
            lootReporter.EndTransmission();
            ItemsToLoot = new List<ItemSaveData>();
        }
        void OnApplicationQuit() 
        {
            lootReporter.EndTransmission();
        }
    }
}