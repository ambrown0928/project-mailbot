using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using Loot;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class ItemController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        #region Data Values
        [Header("Item Data")]

            [SerializeField] private Item itemData;
            private int quantity = 1;

        #endregion
        #region GameObject Values

            private GameObject potentialSlot;
            private GameObject parent;
            public GameObject windowGlobal;
       
        #endregion
        #region UI Values
        [Header("UI Fields")]

            [SerializeField] private Image itemIcon;
            [SerializeField] private Text quantityField;
            private RectTransform rectTransform;
            private CanvasGroup canvasGroup;

        #endregion

        private bool dragged;
        private static bool selected; // static so it stays the same across all items
        
        private Vector2Int inventorySlot;
        public Vector2Int InventorySlot { get => inventorySlot; set => inventorySlot = value; }
        public InventoryController inventoryController;

        #region Unity Default Functions
         
        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();

            if (itemData == null) return; // object created by code, initialize item will be called there
            InitializeItem();
        }
        void Update()
        {
            quantityField.text = "x" + quantity;

            HandlePackage();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            rectTransform.SetParent(windowGlobal.transform); // deattach from the current panel
            dragged = true;
            selected = false;
            canvasGroup.blocksRaycasts = false; // item must not block raycasts while dragging so panels can interact w/ mouse
        }
        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.position = Input.mousePosition;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            Select();
            dragged = false;
            selected = true;

            if (PlayerHoveringOverNewSlot())
            {
                if(SlotIsFull())
                {
                    if (ItemInSlotIsTheSame())
                    {
                        ItemController otherItem = potentialSlot.transform.GetChild(0).GetComponent<ItemController>();
                        AddQuantity(otherItem.quantity);
                        otherItem.DeleteItem();

                        AddItemToNewSlot();
                        return;
                    }
                    SetParentAndResetPosition(parent.transform);
                    return;
                }
                AddItemToNewSlot();
                return;
            }
            
            SetParentAndResetPosition(parent.transform);
            
        }

        private void AddItemToNewSlot()
        {
            if (SlotIsLootWindow())
            { // trying to add item to loot window
                if (ItemIsPackage())
                { // can't add package to loot window
                    SetParentAndResetPosition(parent.transform);
                    return;
                }
                try
                {
                    GetQuantityToSendToLootWindow();
                    return;
                }
                catch (System.Exception exception)
                {
                    Debug.LogError(exception);
                    SetParentAndResetPosition(parent.transform);
                    return;
                }
            }
            SwitchParentAndSetNewInventorySlot();
            SetParentAndResetPosition(parent.transform);

            SaveItem();
        }

        private void SwitchParentAndSetNewInventorySlot()
        {
            parent = potentialSlot;
            potentialSlot = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(selected && ItemIsCurrentlySelected()) 
            {
                Unselect();
            }
            else
            {
               Select();
            }
        }

        #endregion
        #region Extraneous GameObject Functions

        public void SetParentAndResetPosition(Transform newParent)
        {
            rectTransform.SetParent(newParent);
            rectTransform.position = rectTransform.parent.transform.position;
        }
        private void Select()
        {
            selected = true;
            inventoryController.SingleItemPanel.DisplayItem(itemData);
        }
        private void Unselect()
        {
            selected = false;
            inventoryController.SingleItemPanel.UndisplayItem();
        }
        public void ChangeSelectedItemPotentialSlot(GameObject potentialSlot)
        { // called by InventoryPanel.cs
            if(!dragged) return;
            this.potentialSlot = potentialSlot;
        }
        
        #endregion
        #region Boolean / Check Functions
        /// 
        /// Region for boolean functions
        /// 
        private bool ItemIsCurrentlySelected()
        {
            if(inventoryController.SingleItemPanel.GetSelectedItem() == null) return false;
            return itemData.name == inventoryController.SingleItemPanel.GetSelectedItem().name;
        }
        private bool ItemInSlotIsTheSame()
        {
            if(potentialSlot.tag == "Loot") return false;
            return potentialSlot.transform.GetChild(0).GetComponent<ItemController>().itemData.name == itemData.name;
        }
        private bool SlotIsFull()
        {
            if(potentialSlot.tag == "Loot") return false;
            return potentialSlot.transform.childCount > 0;
        }
        private bool PlayerHoveringOverNewSlot()
        {
            return potentialSlot != null;
        }
        private bool ItemIsPackage()
        {
            return itemData.GetType() == typeof(Package);
        }
        private bool SlotIsLootWindow()
        {
            return potentialSlot.gameObject.tag == "Loot";
        }

        #endregion
        #region Item Management Functions
        /// 
        /// Region for functions relating to the management of 
        /// items. Includes quantity methods, sending the item
        /// to the loot window, and save-load methods.
        /// 
        private void GetQuantityToSendToLootWindow()
        {
            ItemSaveData itemToSend = new ItemSaveData(itemData, quantity, inventorySlot);
            inventoryController.quantityWindowController.OpenWindow(itemToSend, this);
            this.gameObject.SetActive(false);
        }
        public ItemSaveData SendItemToLootWindow(int quantity)
        {
            if(ItemIsPackage()) throw new CannotStorePackageInPackageException();
            ItemSaveData itemToSend = new ItemSaveData(itemData, quantity, inventorySlot);
            return itemToSend;
        }
        public void RecieveQuantityToSendToLootWindow(int quantityToRemove)
        {
            // reset object to parent in case item is still in inventory
            SetParentAndResetPosition(parent.transform);
            gameObject.SetActive(true);

            if(quantityToRemove == 0) return;

            RemoveQuantity(quantityToRemove);
            potentialSlot.GetComponent<LootWindowController>().AddLootItem(SendItemToLootWindow(quantityToRemove));
            SaveItem();

            if(quantity <= 0)
            {
                DeleteItem();
            }
            potentialSlot = null;
        }
        public void AddQuantity(int add)
        {
            quantity += add;
            SaveItem();
        }
        public void RemoveQuantity(int remove)
        {
            quantity -= remove;
            if( quantity <= 0 )
            {
                if(potentialSlot.tag == "Loot") return;
                DeleteItem();
                return;
            }
            SaveItem();
        }
        public void DeleteItem()
        {
            RemoveItem();
            Destroy(gameObject);
        }
        public void RemoveItem()
        {
            quantity = 0;
            SaveItem();
        }
        private void InitializeItem()
        { // for initializing the item's in-game attributes
            gameObject.name = itemData.name;
            itemIcon.sprite = itemData.icon;
            selected = false;
            
            if(ItemIsPackage())
            {
                ((Package) itemData).lootWindowController = inventoryController.lootWindowController;
                ((Package) itemData).lootObserver = new LootObserver();
            }
        }
        public void LoadItem(ItemSaveData data, GameObject parent)
        { // called from Inventory Controller
            itemData = Resources.Load<Item>("Items/" + data.name);
            inventorySlot = data.InventorySlot;
            quantity = data.Quantity;
            this.parent = parent;
            InitializeItem();
        }
        public void SaveItem()
        { // sends data to inventory controller
            ItemSaveData itemToSave = new ItemSaveData(itemData, quantity, inventorySlot); // sends data with old inventory slot
            if(parent.tag != "Loot") InventorySlot = parent.GetComponent<InventoryPanel>().coordinates; // gets new inventory slot
            try
            {
                inventoryController.UpdateItemFromController(itemToSave, InventorySlot);
            }
            catch (System.Exception exception)
            {
                if( exception.GetType() == typeof(System.ItemNotFoundException) )
                {
                    if(quantity == 0)
                    { // double delete just in case
                        inventoryController.RemoveAt(inventorySlot);
                        return;
                    }
                    inventoryController.AddItem(itemToSave); // add item if it isn't there
                    return;
                }
                throw;
            }
        }

        #endregion
        #region Package Code

        private void HandlePackage()
        {
            if(!ItemIsPackage()) return;

            Package package = (Package) itemData; // place to store package for easier coding

            if(package.lootObserver.currentItem == null) return;

            if(package.lootObserver.currentItem.Quantity == 0) 
            {
                // reset package to accept new item
                package.currentItem = null;
                package.lootObserver.currentItem = null; 
            }
            else
            {
                if(package.currentItem == null || package.currentItem.name == "" || package.currentItem.Quantity == 0)
                    package.AddItem(package.lootObserver.currentItem);
            }
            itemData = package; // reset and save package;
        }

        #endregion
    }
}

