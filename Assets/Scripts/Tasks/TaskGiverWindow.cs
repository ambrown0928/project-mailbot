using System.Collections;
using System.Collections.Generic;
using Tasks.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Tasks
{
    public class TaskGiverWindow : TaskGiver
    {
        [SerializeField] private Text taskTitle;
        [SerializeField] private Text taskDescription;

        public void SetTaskWindow() 
        {
            taskTitle.text = TaskToGive.GetCurrentStage().name;
            taskDescription.text = TaskToGive.GetCurrentStage().description;
        }
        
    }
}
