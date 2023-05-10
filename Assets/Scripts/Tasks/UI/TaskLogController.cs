using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using float_oat.Desktop90;
using Tasks.Database;
using UnityEngine;

namespace Tasks.UI
{
    public class TaskLogController : MonoBehaviour
    {
        [SerializeField] private GameObject taskBlerbPrefab;
        [SerializeField] private GameObject taskBlerbContainer;
        [SerializeField] private TaskPanelController taskPanelController;

        private IDbConnection database;
        private List<Task> tasks;
        private bool showCompleted = false;

        public TaskPanelController TaskPanelController { get => taskPanelController; set => taskPanelController = value; }

        void Awake() 
        {
            database = TaskDatabaseAccess.CreateAndOpenDatabase();
            tasks = LoadTasks();
            transform.parent.GetComponent<WindowController>().Close();

            if(tasks.Count == 0) return;
            InitializeList();
        }

        private void InitializeList()
        {
            ResetList();
            Task lastTask = null;
            foreach(Task task in tasks)
            {
                if(task == null) return;
                if(task.Completed != showCompleted) continue;
                CreateBlerb(task);
                lastTask = task;
            }
            if(lastTask == null) return;
            taskPanelController.HighlightedTask = lastTask;
        }

        private void ResetList()
        {
            foreach(Transform child in taskBlerbContainer.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        private void CreateBlerb(Task task)
        {   
            GameObject newBlerb = Instantiate(taskBlerbPrefab);
            newBlerb.transform.SetParent(taskBlerbContainer.transform, false);
            newBlerb.transform.SetAsFirstSibling();

            TaskBlerbController newBlerbController = newBlerb.GetComponent<TaskBlerbController>();
            newBlerbController.Task = task;
            newBlerbController.TaskLogController = this;
            newBlerbController.InitializeBlerb();
        }
        public void AddTask(Task task)
        {
            SaveTask(task);
            CreateBlerb(task);
        }

        public void SaveTask(Task task)
        {
            IDbCommand insertTaskCommand = database.CreateCommand();
            insertTaskCommand.CommandText = "INSERT OR REPLACE INTO Tasks (id, taskName) VALUES (" 
                                        + (int)task.id + ", \"" + task.name + "\")";
            insertTaskCommand.ExecuteNonQuery();

            tasks = LoadTasks();
        }
        public List<Task> LoadTasks()
        {
            List<Task> taskList = new List<Task>();

            IDbCommand selectAllCommand = database.CreateCommand();
            selectAllCommand.CommandText = "SELECT * FROM Tasks";
            IDataReader taskReader = selectAllCommand.ExecuteReader();

            while(taskReader.Read())
            {
                string taskName = taskReader.GetString(1);

                Task task = Resources.Load<Task>("Tasks/" + taskName);
                taskList.Add(task);
            }
            return taskList;
        }
        public void ToggleShowComplete()
        {
            showCompleted = !showCompleted;
            InitializeList();
        }
        public bool GetShowCompleted()
        {
            return showCompleted;
        }
    }
}
