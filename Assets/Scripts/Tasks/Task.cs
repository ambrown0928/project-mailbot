using System.Collections;
using System.Collections.Generic;
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
        
        public Task()
        {
        }

        public Task(List<TaskStage> taskStages, int currentTask, bool completed, int id)
        {
            this.taskStages = taskStages;
            CurrentTask = currentTask;
            Completed = completed;
            this.id = id;
        }

        public int CurrentTask { get => currentTask; set => currentTask = value; }
        public bool Completed { get => completed; set => completed = value; }
        public List<TaskStage> TaskStages { get => taskStages; set => taskStages = value; }
        public bool IsInLog { get => isInLog; set => isInLog = value; }

        public void NextStage()
        {
            if(completed) return;

            currentTask++;
            if(currentTask >= taskStages.Count)
            {
                currentTask--;
                completed = true;
            }
        }
        public bool TaskIsReached()
        {
            return taskStages[currentTask].goal.IsReached();
        }
        public TaskStage GetCurrentStage()
        {
            return taskStages[currentTask];
        }
        public void PutTaskInLog()
        {
            isInLog = true;
        }
    }
    
}