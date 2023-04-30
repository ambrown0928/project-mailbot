using System.Collections;
using System.Collections.Generic;
using Tasks.UI;
using UnityEngine;

namespace Tasks
{
    public class TaskGiver : MonoBehaviour
    {
        [SerializeField] private Task taskToGive;
        [SerializeField] private TaskLogController taskLogController;

        public Task TaskToGive { get => taskToGive; set => taskToGive = value; }
        
        public void GiveTask()
        {
            if(taskToGive == null || taskToGive.IsInLog) return;
            taskToGive.PutTaskInLog();
            taskLogController.AddTask(taskToGive);
            taskToGive = null;
        }
    }
}