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
        public int requiredAmount;
        public GoalType goalType;
        public int currentAmount;
        public string target; // target npc to deliver to
        public Item itemToDeliver;
    
        public bool CompareTarget(string target)
        {
            return this.target == target;
        }
        public bool IsReached()
        {
            return currentAmount >= requiredAmount;
        }
        
        public bool CanCompleteDelivery(string target)
        {
            if(!GoalIsOfType(GoalType.Delivery)) return false;
            if(this.target != target) return false;
            
            currentAmount += requiredAmount;
            return true;
        }

        public bool TalkedTo(string target)
        {
            if(!GoalIsOfType(GoalType.Talk) || this.target != target) return false;
            currentAmount++;
            return true;
        }

        public bool GoalIsOfType(GoalType goalType)
        {
            return this.goalType == goalType;
        }
    }
    public enum GoalType
    {
        Delivery,
        Talk
    }
}