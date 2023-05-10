using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class InventoryPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        public Vector2Int coordinates;
        private GameObject currentItem;
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (ItemNotBeingDragged(eventData)) return;
            currentItem = eventData.pointerDrag.gameObject;
            
            currentItem.GetComponent<ItemController>().ChangeSelectedItemPotentialSlot(gameObject);
        }
    
        public void OnPointerExit(PointerEventData eventData) 
        {
            if(ItemNotBeingDragged(eventData)) return;
            currentItem.GetComponent<ItemController>().ChangeSelectedItemPotentialSlot(null);
            currentItem = null;
        }
        private bool ItemNotBeingDragged(PointerEventData eventData)
        {
            return eventData.pointerDrag == null;
        }
    }
}
