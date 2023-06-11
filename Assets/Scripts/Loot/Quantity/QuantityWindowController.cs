using System;
using System.Collections;
using System.Collections.Generic;
using float_oat.Desktop90;
using Inventory;
using Inventory.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Loot.Quantity
{
    public class QuantityWindowController : MonoBehaviour
    {
        #region  UI Fields
        [Header("UI Fields")]
    
            [SerializeField] private Slider quantitySlider;
            [SerializeField] private InputField currentValueField;
            [SerializeField] private Image icon;

        #endregion
        #region Controllers

            private WindowController windowController;
            // private ItemController currentItemController;
        
        #endregion

        private int currentValue;
        private int maxValue;
        private ItemPrototype itemData;

        #region Unity Default Functions

        private void Awake() 
        {
            windowController = GetComponentInParent<WindowController>();
            windowController.Close(); // close on awake so object loads but isn't visible until necessary
        }
        private void Update() 
        {
            if(itemData == null) return; // skip if no item

            currentValue = ((int)quantitySlider.value);
            currentValueField.text = currentValue.ToString();

            icon.sprite = itemData.icon;
        }
        
        #endregion
        #region Window Functions

        public void OpenWindow(Item item)
        {
            maxValue = item.Quantity;
            itemData = Resources.Load<ItemPrototype>("Items/" + item.Name); // TODO - Replace with AssetBundle / other solution
            // currentItemController = itemController;

            quantitySlider.maxValue = maxValue;
            windowController.Open();
        }
        public void CloseWindow()
        { // called when ok button is pressed
            // currentItemController.RecieveQuantityToSendToLootWindow(currentValue);
            // currentItemController = null;
            // itemData = null;
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
