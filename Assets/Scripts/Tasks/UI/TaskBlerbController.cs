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
        [SerializeField] private TaskGraph task;
        [SerializeField] private TaskWindowController taskWindowController;

        public void InitializeBlerb(TaskWindowController taskWindowController, TaskGraph task)
        {
            this.taskWindowController = taskWindowController;

            this.task = task;
            blerbName.text = task.name;
        }
        private void Update() 
        {
            GetComponent<D90Button>().interactable = (taskWindowController.taskPanelController.currentTask == task) ? false : true;
        }
        public void OnClick()
        {
            taskWindowController.taskPanelController.HighlightTask(task);
        }
        
    }
    
}