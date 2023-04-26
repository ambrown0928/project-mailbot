using System;
using System.Collections;
using System.Collections.Generic;
using System.Storage;
using Inventory.Items;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        private Storage<ItemSaveData> inventoryStorage;
        [SerializeField]
        private RectTransform inventoryPanel;
        
        public SingleItemPanelController singleItemPanel;
        
        [SerializeField]
        private GameObject itemPrefab;

        void Awake() 
        {
            inventoryStorage = new Storage<ItemSaveData>(); // TODO - implement save/load functionality
            singleItemPanel = GetComponentInChildren<SingleItemPanelController>();
        }
        
        public void CreateItemOnInitialize(ItemSaveData item)
        {
            GameObject tempItem = Instantiate(itemPrefab);
            ItemController tempItemController = tempItem.GetComponent<ItemController>();
            tempItemController.LoadItem(item);
            tempItemController.SwitchParent( GetRowColumnLiteral(item.InventorySlot) );    
        }

        public Transform GetRowColumnLiteral(Vector2 slot)
        { // gets the row/column from the inventory ui
            return inventoryPanel.GetChild((int)slot.x).GetChild((int)slot.y);
        }

        
    }
}
