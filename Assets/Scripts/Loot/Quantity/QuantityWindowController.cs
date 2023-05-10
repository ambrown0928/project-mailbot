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
        [SerializeField] private Slider quantitySlider;
        [SerializeField] private InputField currentValueField;
        [SerializeField] private Image icon;

        private WindowController windowController;
        private ItemController currentItemController;

        private int currentValue;
        private int maxValue;
        private Item itemData;

        private void Awake() 
        {
            windowController = GetComponentInParent<WindowController>();
            windowController.Close();
        }

        private void Update() 
        {
            if(itemData == null) return;

            currentValue = ((int)quantitySlider.value);
            currentValueField.text = currentValue.ToString();

            icon.sprite = itemData.icon;
        }
        
        public void OpenWindow(ItemSaveData item, ItemController itemController)
        {
            maxValue = item.Quantity;
            itemData = Resources.Load<Item>("Items/" + item.name);
            currentItemController = itemController;

            quantitySlider.maxValue = maxValue;
            windowController.Open();
        }
        public void CloseWindow()
        {
            currentItemController.RecieveQuantityToSendToLootWindow(currentValue);
            currentItemController = null;
            itemData = null;
            windowController.Close();
        }

        public void UpdateValueFromFloat(float value)
        {
            if(quantitySlider) quantitySlider.value = value;
            if(currentValueField) currentValueField.text = quantitySlider.value.ToString();
        }
        public void UpdateValueFromString(string value)
        {
            if(quantitySlider) quantitySlider.value = int.Parse(value);
            if(currentValueField) currentValueField.text = value;
        }
    }
}
