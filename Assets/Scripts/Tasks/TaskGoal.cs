using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Items;
using UnityEngine;

namespace Tasks
{
    [System.Serializable]
    public class TaskGoal
    {
        public string target; // item to deliver
        public int requiredAmount;
        public GoalType goalType;
        public int currentAmount;
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