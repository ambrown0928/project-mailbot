using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class SingleItemPanelController : MonoBehaviour
    {
        [SerializeField]
        private Text itemName;
        [SerializeField]
        private Text itemType;
        [SerializeField]
        private Text itemDescription;
        [SerializeField]
        private Image itemIcon;
        private Item currentItem;

        void Update() 
        {
            if(currentItem == null) return;

            itemName.text = currentItem.name;
            itemDescription.text = currentItem.description;
            itemIcon.sprite = currentItem.icon;
        }
        public void DisplayItem(Item item)
        {
            gameObject.SetActive(true);
            currentItem = item;
            Debug.Log(currentItem);
        }
        public void UndisplayItem()
        {
            currentItem = null;
            gameObject.SetActive(false);
        }
        public Item GetSelectedItem()
        {
            return currentItem;
        }
    }
}
