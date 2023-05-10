using System;
using System.Collections;
using System.Collections.Generic;
using Dialogues;
using Inventory;
using Inventory.Items;
using Tasks;
using UnityEngine;

namespace Tasks
{
    public class TaskReciever : MonoBehaviour
    {
        [SerializeField] private InventoryController inventoryController;
        [SerializeField] private Task taskToRecieve;
        public Dialogue recievedDialogue;

        public bool CheckIfDeliveryCanBeCompleted(string target)
        {
            if(taskToRecieve.Completed) return false;
            if(taskToRecieve.GetCurrentStage().goal.target != target) return false;
            try 
            {
                ItemSaveData itemToTake = new ItemSaveData( taskToRecieve.GetCurrentStage().goal.itemToDeliver, 
                                                            taskToRecieve.GetCurrentStage().goal.requiredAmount, new Vector2Int());
                inventoryController.TakeItem(itemToTake);
                return taskToRecieve.CheckTaskCompleted(target);
            }
            catch(System.Exception exception)
            {
                Debug.LogError(exception);
                return false;
            }
        }

        public bool ProgressTask(string target)
        {
            if(!taskToRecieve.GoalIsOfType(GoalType.Delivery)) 
            {
                return taskToRecieve.CheckTaskCompleted(target);    
            }
            return CheckIfDeliveryCanBeCompleted(target);
        }

        
    }
}