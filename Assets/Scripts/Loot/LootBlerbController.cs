using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Loot
{
    public class LootBlerbController : MonoBehaviour
    {
        [SerializeField] private Text blerbName;
        [SerializeField] private Text quantityField;
        [SerializeField] private Image icon;
        [SerializeField] private Item itemData;
        [SerializeField] private int quantity;
        [SerializeField] private InventoryController inventoryController;
        [SerializeField] private LootWindowController lootWindowController;

        private LootReporter lootReporter;

        public InventoryController InventoryController { get => inventoryController; set => inventoryController = value; }
        public LootWindowController LootWindowController { get => lootWindowController; set => lootWindowController = value; }
        
        private void Awake() 
        {
            lootReporter = new LootReporter();
        }

        public void InitializeBlerb()
        {
            blerbName.text = itemData.name;
            quantityField.text = "x" + quantity;
            icon.sprite = itemData.icon;
        }

        public void Loot()
        {
            ItemSaveData itemToSave = new ItemSaveData(itemData, quantity, new Vector2Int(0, 0));
            inventoryController.AddItem(itemToSave);
            lootWindowController.RemoveLootItem(itemToSave);
            Destroy(this.gameObject);
        }
        public void LoadLoot(ItemSaveData itemToLoad)
        {
            itemData = Resources.Load<Item>("Items/" + itemToLoad.name);
            quantity = itemToLoad.Quantity;
            InitializeBlerb();
        }
        private void OnDestroy() 
        {
            lootReporter.EndTransmission();
        }
    }
}
