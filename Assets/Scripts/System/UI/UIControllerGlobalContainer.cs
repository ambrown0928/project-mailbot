using System.Collections;
using System.Collections.Generic;
using Dialog;
using Dialog.Answer;
using Inventory;
using Loot;
using Loot.Quantity;
using Tasks.UI;
using UnityEngine;
/// 
/// Class for holding all controller variables. Makes references global and
/// accessible to any object needed.
/// 
namespace System.UI
{
    public class UIControllerGlobalContainer : MonoBehaviour
    {
        [Header("Inventory")]
        [SerializeField] private InventoryController inventoryController;
        [SerializeField] private SingleItemPanelController singleItemPanelController;
        [SerializeField] private LootWindowController lootWindowController;
        [SerializeField] private QuantityWindowController quantityWindowController;
        [Header("Tasks and Dialog")]
        [SerializeField] private TaskWindowController taskWindowController;
        [SerializeField] private DialogWindowController dialogWindowController;
        [SerializeField] private AnswerWindowController answerWindowController;
        
        public InventoryController InventoryController { get => inventoryController;  }
        public SingleItemPanelController SingleItemPanelController { get => singleItemPanelController; }
        public LootWindowController LootWindowController { get => lootWindowController; }
        public QuantityWindowController QuantityWindowController { get => quantityWindowController; }
        public TaskWindowController TaskWindowController { get => taskWindowController; }
        public DialogWindowController DialogWindowController { get => dialogWindowController; }
        public AnswerWindowController AnswerWindowController { get => answerWindowController; }
    }
    
}