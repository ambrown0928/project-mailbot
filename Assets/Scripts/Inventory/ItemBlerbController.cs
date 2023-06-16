using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Loot.Quantity;
using Loot;
using System;
using System.UI;

namespace Inventory
{
    /// 
    /// Class for controlling the inventory GameObject version of an item saved
    /// in the inventory system. The quantity is determed by the InventoryController.
    /// Has functionality for
    /// 
    public class ItemBlerbController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region Controller Refereneces

            private UIControllerGlobalContainer uIControllerGlobalContainer;

        #endregion
        #region UI Fields
        [Header("UI Fields")]

            [SerializeField] private Text nameField;
            [SerializeField] private Text quantityField;
            [SerializeField] private Image icon;

        #endregion
        #region GameObject Variables
        [Header("GameObject Field")]

            [SerializeField] private GameObject dragIcon; // to store prefab for drag icon
            private GameObject _dragIcon; // for use in code
            private static GameObject currentItemHovering;
            private ItemBlerbController itemCurrentlyHoveringOver;

        #endregion
        #region Item References
        
            private Item item; // reference to item in inventory. controlled by InventoryController
            public Item Item { get => item; }
            private ItemPrototype itemPrototype; // equal to item.ItemPrototype

        #endregion

        private bool isOverLootWindow; // easy way to check if over loot window
        private int currentIndex;
        public int CurrentIndex { get => currentIndex; set => currentIndex = value; }
        private int possibleIndex = -1;

        #region Blerb & Item Functions
        ///
        /// Functions for controlling, initalizing, and managing items and
        /// the blerb they're in.
        /// 
        public void InitializeBlerb( Item item, UIControllerGlobalContainer uIControllerGlobalContainer, int index)
        {
            this.item = item;
            this.itemPrototype = item.ItemPrototype;
            icon.sprite = itemPrototype.icon;

            this.uIControllerGlobalContainer = uIControllerGlobalContainer;
            currentIndex = index;
            item.Index = index;

            if( ItemIsType(typeof(PackagePrototype)) ) ( (PackagePrototype)this.itemPrototype ).lootWindowController = uIControllerGlobalContainer.LootWindowController;
        }
        public void Select()
        { // select the object
            if( uIControllerGlobalContainer.SingleItemPanelController.GetSelectedItem() == itemPrototype )
            { // item is already selected
                uIControllerGlobalContainer.SingleItemPanelController.UndisplayItem();
                return;
            }
            uIControllerGlobalContainer.SingleItemPanelController.DisplayItem(itemPrototype);
        }
        private void HandlePackage()
        {
            if(!ItemIsType(typeof(PackagePrototype))) return; // don't run on items that aren't packages

            PackagePrototype package = (PackagePrototype) itemPrototype; // stores explicit cast

            if(package.lootObserver.currentItem == null) return; // no need to continue - no item being observed

            if(package.lootObserver.currentItem.Quantity == 0) 
                package.RemoveItem();
            else if(package.currentItem == null || package.currentItem.Name == "" || package.currentItem.Quantity == 0) 
                package.AddItem(package.lootObserver.currentItem);
        }
        private bool ItemIsType(Type type)
        {
            return itemPrototype.GetType() == type;
        }

        #endregion
        #region Unity Default Functions
        /// 
        /// Functions called by UnityEngine.
        /// 
        void Update()
        {
            HandlePackage();

            nameField.text = item.Name;
            quantityField.text = "x" + item.Quantity;

            if(transform.GetSiblingIndex() != currentIndex)
            {
                transform.SetSiblingIndex(currentIndex);
                item.Index = currentIndex;
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            // create new drag icon & make top sibling in global UI
            _dragIcon = Instantiate(dragIcon);
            _dragIcon.transform.SetParent(GetComponentInParent<Canvas>().transform);
            _dragIcon.transform.SetAsLastSibling();
            // set drag icon image to item icon
            _dragIcon.GetComponent<Image>().sprite = itemPrototype.icon;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if(_dragIcon == null) return;

            _dragIcon.transform.position = eventData.position;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if(isOverLootWindow) 
            {
                uIControllerGlobalContainer.QuantityWindowController.OpenWindow(item);
            }
            if(possibleIndex > -1)
            {
                int previousIndex = currentIndex;
                currentIndex = possibleIndex;
                item.Index = currentIndex;

                itemCurrentlyHoveringOver.currentIndex = previousIndex;
                itemCurrentlyHoveringOver = null;
                possibleIndex = -1;
            }
            Destroy(_dragIcon);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if ( ItemNotBeingDragged(eventData) ) return;
            if ( eventData.pointerDrag.gameObject == this.gameObject ) return;

            currentItemHovering = eventData.pointerDrag.gameObject;
            currentItemHovering.GetComponent<ItemBlerbController>().EnterItemBlerb(this, currentIndex);
        }

        private bool ItemNotBeingDragged(PointerEventData eventData)
        {
            return eventData.pointerDrag == null;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if( ItemNotBeingDragged(eventData) ) return;
            if( currentItemHovering == null) return;
            
            currentItemHovering.GetComponent<ItemBlerbController>().ExitItemBlerb();
            currentItemHovering = null;
        }
        #endregion
        #region Loot Window Functions
        /// 
        /// Functions called by the loot and quantity windows.
        /// 
        public void EnterLootWindow()
        { // called by loot window OnPointerEnter
            if(ItemIsType(typeof(PackagePrototype))) return;
            isOverLootWindow = true;
        }
        public void ExitLootWindow()
        { // called by loot window OnPointerExit
            isOverLootWindow = false;
        }
        public void EnterItemBlerb(ItemBlerbController itemCurrentlyHoveringOver, int index)
        {
            possibleIndex = index;
            this.itemCurrentlyHoveringOver = itemCurrentlyHoveringOver;
        }
        public void ExitItemBlerb()
        {
            possibleIndex = -1;
            itemCurrentlyHoveringOver = null;
        }
        #endregion
    }
}
