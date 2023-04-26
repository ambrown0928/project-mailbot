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
            [SerializeField]
            private Item itemData;
            private float quantity = 1;
            #endregion
            [SerializeField]
        private Image itemIcon;
        
        private Vector2 inventorySlot;
        private GameObject potentialSlot;
        private GameObject parent;
        [SerializeField]
        private GameObject windowGlobal;
        [SerializeField]
        private Text quantityField;
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;

        private bool dragged;
        private static bool selected;

        public InventoryController inventoryController;

        public Vector2 InventorySlot { get => inventorySlot; set => inventorySlot = value; }

        void Awake()
        {

            
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();

            if (itemData == null) return;

            InitializeItem();
        }

        // Update is called once per frame
        void Update()
        {
            quantityField.text = "x" + quantity;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            rectTransform.SetParent(windowGlobal.transform);
            dragged = true;
            selected = false;
            canvasGroup.blocksRaycasts = false;
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

                        SwitchParent(potentialSlot.transform);
                        parent = potentialSlot;
                        potentialSlot = null;
                        return;
                    }
                    SwitchParent(parent.transform);
                    return;
                }
                SwitchParent(potentialSlot.transform);
                parent = potentialSlot;
                potentialSlot = null;
            }
            else
            {
                SwitchParent(parent.transform);
            }
        }
        public void SwitchParent(Transform newParent)
        {
            rectTransform.SetParent(newParent);
            rectTransform.position = rectTransform.parent.transform.position;
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

        private bool ItemIsCurrentlySelected()
        {
            if(inventoryController.singleItemPanel.GetSelectedItem() == null) return false;
            return itemData.name == inventoryController.singleItemPanel.GetSelectedItem().name;
        }

        private void Select()
        {
            selected = true;
            inventoryController.singleItemPanel.DisplayItem(itemData);
        }
        private void Unselect()
        {
            selected = false;
            inventoryController.singleItemPanel.UndisplayItem();
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

        public void ChangeSelectedItemPotentialSlot(GameObject potentialSlot)
        {
            if(!dragged) return;
            this.potentialSlot = potentialSlot;
        }

        public void AddQuantity(float add)
        {
            quantity += add;
        }
        public void RemoveQuantity(float remove)
        {
            quantity -= remove;
            if(quantity <= 0 )
            {
                DeleteItem();
            }
        }
        

        private void DeleteItem()
        {
            RemoveItem();
            Destroy(gameObject);
        }

        public void RemoveItem()
        {

        }

        private void InitializeItem()
        {
            gameObject.name = itemData.name;
            parent = rectTransform.parent.gameObject;

            itemIcon.sprite = itemData.icon;
            selected = false;
        }
        public void LoadItem(ItemSaveData data)
        {
            itemData = data.ItemData;
            inventorySlot = data.InventorySlot;
            quantity = data.Quantity;

            InitializeItem();
        }

    }
}

