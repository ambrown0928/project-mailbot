using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Items;
using UnityEngine;

namespace Tasks
{
    /// 
    /// Class used to store and interact with a task's goal. 
    /// 
    [System.Serializable]
    public class TaskGoal
    {
        public string target; // item to deliver
        public int requiredAmount;
        public int currentAmount;

        public GoalType goalType;
        
        public string acceptText;
        public string hiddenText;
        
        public void Complete() => currentAmount = requiredAmount;
        public bool Completed() => currentAmount >= requiredAmount;
        
    }
    public enum GoalType
    {
        Delivery,
        Talk,
        Hidden
    }
}