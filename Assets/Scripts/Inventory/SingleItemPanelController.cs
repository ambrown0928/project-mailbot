using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class SingleItemPanelController : MonoBehaviour
    {
        public ItemPrototype currentItemPrototype;
        
        #region UI Fields
        [Header("UI Fields")]

            [SerializeField] private Text itemName;
            [SerializeField] private Text itemType;
            [SerializeField] private Text itemDescription;
            [SerializeField] private Image itemIcon;
            
        #endregion
        #region Unity Default Methods
    
            void Awake() 
            {
                UndisplayItem();
            }
            void Update()
            {
                if (NoItemSelected()) return;
    
                itemName.text = currentItemPrototype.name;
                itemType.text = currentItemPrototype.GetType().ToString();
                itemDescription.text = currentItemPrototype.description;
                itemIcon.sprite = currentItemPrototype.icon;
            }

        #endregion
        #region Boolean Methods

            private bool NoItemSelected()
            {
                return currentItemPrototype == null;
            }
    
        #endregion
        #region Display Item Methods
            
            public void DisplayItem(ItemPrototype item)
            { // called by ItemBlerbController through InventoryController
                currentItemPrototype = item;
                gameObject.SetActive(true);
            }
            public void UndisplayItem()
            { // called by ItemBlerbController through InventoryController. also called by Deselect Global Button
                currentItemPrototype = null;
                gameObject.SetActive(false);
            }

        #endregion
        #region Item Use Methods
    
            public ItemPrototype GetSelectedItem()
            { 
                return currentItemPrototype;
            }
            public void UseItem()
            { // called by use button in inspector
                currentItemPrototype.Use();
            }

        #endregion
    }
}
