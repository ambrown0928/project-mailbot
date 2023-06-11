using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Loot.Quantity;
using Loot;
using System;

namespace Inventory
{
    public class ItemBlerbController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("UI Fields")]
        [SerializeField] private Text nameField;
        [SerializeField] private Text quantityField;
        [SerializeField] private Image icon;

        [Header("GameObject Fields")]
        [SerializeField] private GameObject dragIcon; // to store prefab
        private GameObject _dragIcon; // for use in code

        private Item item;
        public Item Item { get => item; set => item = value; }
        private ItemPrototype itemPrototype;

        private InventoryController inventoryController;
        private QuantityWindowController quantityWindowController;
        private LootWindowController lootWindowController;

        private bool isOverLootWindow;

        void Update()
        {
            nameField.text = item.Name;
            quantityField.text = "x" + item.Quantity;

            HandlePackage();
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragIcon = Instantiate(dragIcon);
            _dragIcon.transform.SetParent(GetComponentInParent<Canvas>().transform);
            _dragIcon.transform.SetAsLastSibling();

            _dragIcon.GetComponent<Image>().sprite = itemPrototype.icon;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if(_dragIcon == null) return;
            if(ItemIsType(typeof(PackagePrototype))) return;

            _dragIcon.transform.position = eventData.position;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if(isOverLootWindow) 
            {
                quantityWindowController.OpenWindow(this, item, itemPrototype);
            }
            else
            {
                lootWindowController = null;
            }
            Destroy(_dragIcon);
        }

        public void InitializeBlerb(Item item, ItemPrototype itemPrototype, 
                                    InventoryController inventoryController,
                                    QuantityWindowController quantityWindowController, 
                                    LootWindowController lootWindowController)
        {
            this.item = item;
            this.itemPrototype = itemPrototype;
            icon.sprite = itemPrototype.icon;
            this.inventoryController = inventoryController;
            this.quantityWindowController = quantityWindowController;
            if(ItemIsType(typeof(PackagePrototype)))
            {
                ((PackagePrototype)this.itemPrototype).lootWindowController = lootWindowController;
            }
        }

        public void Select()
        {
            if(inventoryController.SingleItemPanel.GetSelectedItem() == itemPrototype)
            {
                inventoryController.SingleItemPanel.UndisplayItem();
                return;
            }
            inventoryController.SingleItemPanel.DisplayItem(itemPrototype);
        }
        public void EnterLootWindow(LootWindowController lootWindowController)
        {
            this.lootWindowController = lootWindowController;
            isOverLootWindow = true;
        }
        public void SendToLootWindow(int quantity)
        {
            item.Quantity -= quantity;
            if(item.Quantity <= 0) inventoryController.RemoveItem(item);

            Item itemToSend = new Item(item.Name, quantity);
            lootWindowController.AddLootItem(itemToSend);
            lootWindowController = null;
        }
        public void ExitLootWindow()
        {
            isOverLootWindow = false;
        }

        private void HandlePackage()
        {
            if(!ItemIsType(typeof(PackagePrototype))) return;

            PackagePrototype package = (PackagePrototype) itemPrototype;

            if(package.lootObserver.currentItem == null) return;

            if(package.lootObserver.currentItem.Quantity == 0) package.RemoveItem();
            else if(package.currentItem.Name == "") package.AddItem(package.lootObserver.currentItem);
        }

        private bool ItemIsType(Type type)
        {
            return itemPrototype.GetType() == type;
        }
    }
}
