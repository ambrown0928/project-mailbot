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
        #region UI Fields
        [Header("UI Fields")]

            [SerializeField] private Text blerbName;
            [SerializeField] private Text quantityField;
            [SerializeField] private Image icon;

        #endregion
        #region Item Data Fields
        [Header("Item Data Fields")]

            [SerializeField] private ItemPrototype itemData;
            [SerializeField] private int quantity;

        #endregion
        #region  Controller Fields

            private InventoryController inventoryController;
            private LootWindowController lootWindowController;

        #endregion

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
            try
            {
                Item itemToSave = new Item(itemData, quantity);
                inventoryController.AddItem(itemToSave);
                lootWindowController.RemoveLootItem(itemToSave);
                Destroy(this.gameObject);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public void LoadLoot(Item itemToLoad)
        {
            itemData = Resources.Load<ItemPrototype>("Items/" + itemToLoad.Name); // TODO - Replace with AssetBundle or other solution
            quantity = itemToLoad.Quantity;
            InitializeBlerb();
        }
        private void OnDestroy() 
        {
            lootReporter.EndTransmission();
        }
    }
}
