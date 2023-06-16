using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Items;
using Tasks;
using UnityEngine;
using XNode;

namespace Dialog
{
    public class RecieveTaskSegment : DialogSegment
    {
        public TaskSegment task;
        public string rejectText;

        public override object GetValue(NodePort port) => null;
        
        public void AttemptProgressTask(string npc, InventoryController inventoryController)
        {
            foreach(TaskGoal goal in task.CompletionPaths) TaskController.AttemptProgress(npc, inventoryController, goal);
        }
        
        public string CheckIfTaskIsCompleteAndReturnDialogResult()
        {
            int index = 0;
            foreach(TaskGoal goal in task.CompletionPaths)
            {
                if( goal.Completed() )
                {
                    TaskController.CompleteTaskSegment(task, index);
                    if(goal.goalType == GoalType.Hidden) return goal.hiddenText;
                    return goal.acceptText;
                }
                index++;
            }
            return rejectText;
        }
    }
    
}