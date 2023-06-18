using System;
using System.Collections;
using System.Collections.Generic;
using float_oat.Desktop90;
using Saving;
using UnityEngine;

namespace Tasks.UI
{
    /// 
    /// Controls the UI representation of tasks. Stores tasks 
    /// in a JSON file to load and 
    /// 
    public class TaskWindowController : MonoBehaviour
    {
        [SerializeField] private GameObject taskBlerbPrefab;
        [SerializeField] private GameObject taskContainer;
        public TaskPanelController taskPanelController;
        [SerializeField] private WindowController windowController;

        private bool showCompleted; // toggle for showing completed tasks vs uncompleted tasks

        void Awake()
        {
            try
            {
                TaskController.tasks = TaskController.LoadTaskList();
            }
            catch(Exception e)
            {
                Debug.LogError(e);
                TaskController.tasks = new List<TaskGraph>();
                TaskController.SaveTaskList();
            }
            windowController.Close();
        }

        public void AddTask(TaskGraph task)
        {
            if(!TaskController.AddToLog(task)) return; 
            CreateBlerb(task);

            TaskController.SaveTaskList();
        }
        private void CreateBlerb(TaskGraph task)
        {
            // create game object
            GameObject newBlerb = Instantiate(taskBlerbPrefab);
            newBlerb.transform.SetParent(taskContainer.transform, false);
            newBlerb.transform.SetAsFirstSibling();

            // initialize task data
            TaskBlerbController newBlerbController = newBlerb.GetComponent<TaskBlerbController>();
            newBlerbController.InitializeBlerb(this, task);
        }
        private void InitializeList()
        {
            ResetList();
            TaskGraph lastTask = null;
            foreach(TaskGraph task in TaskController.tasks)
            {
                if(task == null || task.completed != showCompleted) continue;
                CreateBlerb(task);
                lastTask = task;
            }
            if(lastTask == null) return;
            taskPanelController.HighlightTask(lastTask);
        }
        private void ResetList()
        {
            foreach(Transform child in taskContainer.transform) Destroy(child.gameObject);
        }

        public bool GetShowCompleted()
        { // called by TaskCompletedSwitch.cs
            return showCompleted;
        }
        public void ToggleShowComplete()
        { // called by the 'In progress' and 'Completed' buttons in unity inspector
            showCompleted = !showCompleted;
            InitializeList();
        }

        public void OpenWindow()
        {
            windowController.Open();
            InitializeList();
        }
        public void CloseWindow()
        {
            windowController.Close();
        }
        
        private void OnApplicationQuit() 
        {
            TaskController.SaveTaskList();
        }
    }
    
}