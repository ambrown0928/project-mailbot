using System;
using System.Collections;
using System.Collections.Generic;
using System.UI;
using float_oat.Desktop90;
using Inventory;
using Inventory.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Loot.Quantity
{
    public class QuantityWindowController : MonoBehaviour
    {
        #region Controllers
        [Header("Controller References")]

            [SerializeField] private WindowController windowController;
            [SerializeField] private UIControllerGlobalContainer UIControllerGlobalContainer;
        
        #endregion
        #region  UI Fields
        [Header("UI Fields")]
    
            [SerializeField] private Slider quantitySlider;
            [SerializeField] private InputField currentValueField;
            [SerializeField] private Image icon;

        #endregion
        #region Slider Fields
    
            private int currentValue;
            private int maxValue;

        #endregion
        #region Item Fields

            private ItemPrototype itemPrototype;
            private Item currentItem;

        #endregion
        #region Unity Default Functions

            private void Awake() 
            {
                windowController.Close(); // close on awake so object loads but isn't visible until necessary
            }
            private void Update() 
            {
                if(itemPrototype == null) return; // skip if no item

                currentValue = ((int)quantitySlider.value);
                currentValueField.text = currentValue.ToString();

                icon.sprite = itemPrototype.icon;
            }
        
        #endregion
        #region Window Functions

            public void OpenWindow( Item item)
            {
                currentItem = item;
                maxValue = currentItem.Quantity;
                itemPrototype = item.ItemPrototype;

                quantitySlider.maxValue = maxValue;
                windowController.Open();
            }
            public void CloseWindow()
            { // called when ok button is pressed
                currentItem.Quantity -= currentValue;
                Item itemToSend = new Item(currentItem.Name, currentValue);
                UIControllerGlobalContainer.LootWindowController.AddLootItem(itemToSend);

                if(currentItem.Quantity <= 0) UIControllerGlobalContainer.InventoryController.RemoveItem(currentItem); // delete item after if all was taken

                itemPrototype = null;
                currentItem = null;
                windowController.Close();
            }
        
        #endregion
        #region Update Values Functions

            public void UpdateValueFromFloat(float value)
            { // called by slider to update value
                if(quantitySlider) quantitySlider.value = value;
                if(currentValueField) currentValueField.text = quantitySlider.value.ToString();
            }
            public void UpdateValueFromString(string value)
            { // called by text field to update value
                if(quantitySlider) quantitySlider.value = int.Parse(value);
                if(currentValueField) currentValueField.text = value;
            }

        #endregion
    }
}
