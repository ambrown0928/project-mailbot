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
    public class LootWindowController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
            [SerializeField] private GameObject currentItemHovering;

        #endregion

        private LootReporter lootReporter;
        private bool isPackage;

        private List<Item> itemsToLoot;
        public List<Item> ItemsToLoot { 
            get => itemsToLoot; 
            set 
            {
                itemsToLoot = value;
                NewLoot();
            } 
        }

        private void Awake() 
        {
            lootReporter = new LootReporter();
            windowController.Close();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            // checks if item is being dragged and makes that the currentItem
            if ( ItemNotBeingDragged(eventData) ) return;
            if ( isPackage && ItemsToLoot.Count > 0 ) return;
            if ( eventData.pointerDrag.gameObject == null ) return;

            currentItemHovering = eventData.pointerDrag.gameObject;
            currentItemHovering.GetComponent<ItemBlerbController>().EnterLootWindow(this);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if ( currentItemHovering == null ) return;
            currentItemHovering.GetComponent<ItemBlerbController>().ExitLootWindow();
        }

        private bool ItemNotBeingDragged(PointerEventData eventData)
        {
            return eventData.pointerDrag == null;
        }

        public void AddLootItem(Item item)
        {
            lootReporter.AddItem(item);
            itemsToLoot.Add(item);
            CreateBlerb(item);
        }
        public void RemoveLootItem(Item itemToRemove)
        {
            itemsToLoot.Remove(itemToRemove);
            Item tempItemToPass = new Item(itemToRemove.Name, 0);
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
        
        public LootReporter OpenLootWindow(List<Item> loot, string title)
        { // called by auto-generating loot enemies
            if(windowController.gameObject.activeInHierarchy) throw new LootWindowIsOpenException();

            isPackage = false;
            ItemsToLoot = loot;

            windowTitle.text = title;
            return lootReporter;
        }
        public LootReporter OpenLootWindow(Item loot, string title)
        {
            if(windowController.gameObject.activeInHierarchy) throw new LootWindowIsOpenException();

            isPackage = true;
            if(loot.Name == "" || loot.Quantity == 0) ItemsToLoot = new List<Item>(); // create an empty list if pkg is empty
            else ItemsToLoot = new List<Item>{ loot }; 

            windowTitle.text = title;
            return lootReporter;
        }
        public void NewLoot()
        {
            ResetList();
            windowController.Open();
            foreach(Item item in itemsToLoot) CreateBlerb(item);
        }
        public void ResetList()
        {
            foreach(Transform child in lootListWindow.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        private void CreateBlerb(Item item)
        {
            // instantiate gameobject and setup requirements
            GameObject newLootBlerb = Instantiate(lootBlerbPrefab);
            newLootBlerb.transform.SetParent(lootListWindow.transform, false);
            newLootBlerb.transform.SetAsFirstSibling();

            // setup loot blerb controller
            LootBlerbController newLootBlerbController = newLootBlerb.GetComponent<LootBlerbController>();
            newLootBlerbController.InventoryController = inventoryController;
            newLootBlerbController.LootWindowController = this;
            newLootBlerbController.LoadLoot(item);
        }

        void OnDisable()
        {
            lootReporter.EndTransmission();
            ItemsToLoot = new List<Item>();
        }
        void OnApplicationQuit() 
        {
            lootReporter.EndTransmission();
        }

    }
}