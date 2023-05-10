using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Items;
using Tasks.UI;
using UnityEngine;

namespace Tasks
{
    public class TaskGiver : MonoBehaviour
    {
        [SerializeField] private Task taskToGive;
        [SerializeField] private TaskLogController taskLogController;
        [SerializeField] private InventoryController inventoryController;
        
        private void Awake() 
        {
            if(taskToGive.IsInLog) taskToGive = null;    
        }

        public Task TaskToGive { get => taskToGive; set => taskToGive = value; }
        
        public void GiveTask()
        {
            if(taskToGive == null || taskToGive.IsInLog) return;
            ItemSaveData deliveryItem = taskToGive.PutTaskInLog();
            inventoryController.AddItem(deliveryItem);
            
            taskLogController.AddTask(taskToGive);
            taskToGive = null;
        }
    }
}