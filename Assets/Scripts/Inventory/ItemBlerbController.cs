using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        private ItemPrototype itemPrototype;
        private InventoryController inventoryController;
    
        void Update()
        {
            nameField.text = item.Name;
            quantityField.text = "x" + item.Quantity;
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

            _dragIcon.transform.position = eventData.position;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            Destroy(_dragIcon);
        }

        public void InitializeBlerb(Item item, ItemPrototype itemPrototype, InventoryController inventoryController)
        {
            this.item = item;
            this.itemPrototype = itemPrototype;
            icon.sprite = itemPrototype.icon;
            this.inventoryController = inventoryController;
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
    }
}
