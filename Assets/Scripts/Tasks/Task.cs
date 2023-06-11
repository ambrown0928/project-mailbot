using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;

namespace Tasks
{
    [System.Serializable] [CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Task", order = 1)]
    public class Task : ScriptableObject
    {
        [SerializeField] public int id;
        [SerializeField] private List<TaskStage> taskStages;
        [SerializeField] private int currentTask = 0;
        [SerializeField] private bool completed;
        [SerializeField] private bool isInLog;

        public int CurrentTask { get => currentTask; set => currentTask = value; }
        public bool Completed { get => completed; set => completed = value; }
        public List<TaskStage> TaskStages { get => taskStages; set => taskStages = value; }
        public bool IsInLog { get => isInLog; set => isInLog = value; }

        void Awake()
        {
            
        }

        public void NextStage()
        {
            if(completed) return;

            currentTask++;
            if(currentTask >= taskStages.Count) // stages are completed
            {
                Debug.Log("Completed Task");
                completed = true;
                currentTask = taskStages.Count - 1; // locks currentTask to max stage index
            }
        }
        public bool TaskIsReached()
        {
            return taskStages[currentTask].goal.IsReached();
        }
        private bool CompleteTask(string target)
        {
            if(!IsInLog) return false;
            if(Completed) return false;
            if(TaskIsReached()) return false;

            switch(GetCurrentStage().goal.goalType)
            {
                case GoalType.Delivery :
                    return GetCurrentStage().goal.CanCompleteDelivery(target);
                case GoalType.Talk :
                    return GetCurrentStage().goal.TalkedTo(target);
                default:
                    return false;
            }
        }
        public bool CheckTaskCompleted(string target)
        {
            if(!CompleteTask(target)) return false;
            Debug.Log("Completed Task Goal");
            NextStage();
            return true;
        }
        public TaskStage GetCurrentStage()
        {
            return taskStages[currentTask];
        }
        public bool GoalIsOfType(GoalType goalType)
        {
            return GetCurrentStage().goal.GoalIsOfType(goalType);
        }
        public Item PutTaskInLog()
        {
            isInLog = true;
            if(GoalIsOfType(GoalType.Delivery)) return new Item(GetCurrentStage().goal.itemToDeliver, 
                                                                        GetCurrentStage().goal.requiredAmount); // creates an item to deliver and sends to inventory
            return null;
        }
    }
    
}