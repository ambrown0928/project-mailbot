using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
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
            private float quantity = 1;

        #endregion
        #region GameObject Values

            private GameObject potentialSlot;
            private GameObject parent;
            [SerializeField] private GameObject windowGlobal;
       
        #endregion
        #region UI Values

        [Header("UI Fields")]
            [SerializeField] private Image itemIcon;
            [SerializeField] private Text quantityField;
            private RectTransform rectTransform;
            private CanvasGroup canvasGroup;

        #endregion

        private bool dragged;
        private static bool selected;

        public InventoryController inventoryController;
        
        private Vector2 inventorySlot;
        public Vector2 InventorySlot { get => inventorySlot; set => inventorySlot = value; }

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
                if(PlayerSlotIsFull())
                {
                    if (ItemInSlotIsTheSame())
                    {
                        ItemController otherItem = potentialSlot.transform.GetChild(0).GetComponent<ItemController>();
                        AddQuantity(otherItem.quantity);
                        otherItem.DeleteItem();

                        SetParentAndResetPosition(potentialSlot.transform);
                        parent = potentialSlot;
                        potentialSlot = null;
                        SaveItem();
                        return;
                    }
                    SetParentAndResetPosition(parent.transform);
                    return;
                }
                SetParentAndResetPosition(potentialSlot.transform);
                parent = potentialSlot;
                potentialSlot = null;
                SaveItem();
            }
            else
            {
                SetParentAndResetPosition(parent.transform);
            }
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
        {
            if(!dragged) return;
            this.potentialSlot = potentialSlot;
        }

        #endregion
        #region Boolean / Check Functions

        private bool ItemIsCurrentlySelected()
        {
            if(inventoryController.SingleItemPanel.GetSelectedItem() == null) return false;
            return itemData.name == inventoryController.SingleItemPanel.GetSelectedItem().name;
        }
        private bool ItemInSlotIsTheSame()
        {
            return potentialSlot.transform.GetChild(0).GetComponent<ItemController>().itemData.name == itemData.name;
        }
        private bool PlayerSlotIsFull()
        {
            return potentialSlot.transform.childCount > 0;
        }
        private bool PlayerHoveringOverNewSlot()
        {
            return potentialSlot != null;
        }

        #endregion
        #region Item Management Functions

        public void AddQuantity(float add)
        {
            quantity += add;
            SaveItem();
        }
        public void RemoveQuantity(float remove)
        {
            quantity -= remove;
            if(quantity <= 0 )
            {
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
            // might be removed later, is here in case of extra functionality
        }
        private void InitializeItem()
        { // for initializing the item's in-game attributes
            gameObject.name = itemData.name;
            parent = rectTransform.parent.gameObject; // object is attached to slot either by default or thru inventory controller

            itemIcon.sprite = itemData.icon;
            selected = false;
        }
        public void LoadItem(ItemSaveData data)
        { // called from Inventory Controller
            itemData = data.ItemData;
            inventorySlot = data.InventorySlot;
            quantity = data.Quantity;

            InitializeItem();
        }
        public void SaveItem()
        { // sends data to inventory controller
            ItemSaveData itemToSave = new ItemSaveData(itemData, quantity, inventorySlot);
            inventoryController.UpdateItemFromController(itemToSave);
        }

        #endregion
    }
}

