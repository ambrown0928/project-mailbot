using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Items;
using Tasks;
using UnityEngine;
using XNode;

namespace Dialog
{
    /// Derived from DialogSegment. Node for receiving 
    public class ReceiveTaskSegment : DialogSegment
    {
        public TaskSegment task;
        public string rejectText; // text for when task is rejected

        public override object GetValue(NodePort port) => null;
        
        /// 
        /// Attempt to progress all possible goals for a task segment.
        /// 
        public void AttemptProgressTask(InventoryController inventoryController)
        {
            foreach(TaskGoal goal in task.CompletionPaths) TaskController.AttemptProgress(inventoryController, goal);
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