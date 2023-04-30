using System.Collections;
using System.Collections.Generic;
using float_oat.Desktop90;
using UnityEngine;
using UnityEngine.UI;

namespace Tasks.UI
{
    public class TaskBlerbController : MonoBehaviour
    {
        [SerializeField] private Text blerbName;
        [SerializeField] private Task task;
        [SerializeField] private TaskLogController taskLogController;

        public Task Task { get => task; set => task = value; }
        public TaskLogController TaskLogController { get => taskLogController; set => taskLogController = value; }

        public void InitializeBlerb()
        {
            blerbName.text = task.name;
        }
        private void Update() 
        {
            GetComponent<D90Button>().interactable = !IsHighlightedTask();
        }
        public void OnClick()
        {
            TaskLogController.TaskPanelController.HighlightedTask = task;
        }
        private bool IsHighlightedTask()
        {
            if(taskLogController.TaskPanelController.HighlightedTask == null) return false;
            return task.id == taskLogController.TaskPanelController.HighlightedTask.id;
        }
    }
    
}