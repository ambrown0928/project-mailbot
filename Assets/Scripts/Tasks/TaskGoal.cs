using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasks
{
    [System.Serializable]
    public class TaskGoal
    {
        public GoalType goalType;
        public int requiredAmount;
        public int currentAmount;
        public string target;
    
        public bool CompareTarget(string target)
        {
            return this.target == target;
        }
        public bool IsReached()
        {
            return currentAmount >= requiredAmount;
        }
        public void EnemyDisabled(string target)
        {
            if(CompareTarget(target) && GoalIsOfType(GoalType.Disable))
            {
                currentAmount++;
            }
        }
        public void DeliveryCompleted(string target)
        {
            if(GoalIsOfType(GoalType.Delivery) && CompareTarget(target))
            {
                currentAmount = requiredAmount;
            }
        }
        public void ItemGathered(string target)
        {
            if(GoalIsOfType(GoalType.Gather) && CompareTarget(target))
            {
                currentAmount++;
            }
        }
        private bool GoalIsOfType(GoalType type)
        {
            return goalType == type;
        }
    }
    [System.Serializable]
    public enum GoalType
    {
        Delivery,
        Gather,
        Disable
    }
}