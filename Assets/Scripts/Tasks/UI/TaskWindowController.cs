using System;
using System.Collections;
using System.Collections.Generic;
using float_oat.Desktop90;
using Saving;
using UnityEngine;

namespace Tasks.UI
{
    public class TaskWindowController : MonoBehaviour
    {
        private const string FILE_PATH = "/Tasks/task-log.json";

        [SerializeField] private GameObject taskBlerbPrefab;
        [SerializeField] private GameObject taskContainer;
        [SerializeField] private TaskPanelController taskPanelController;
        [SerializeField] private WindowController windowController;

        private List<TaskGraph> tasks;
        private bool showCompleted;

        void Awake()
        {
            try
            {
                tasks = LoadTaskList();
            }
            catch(Exception e)
            {
                Debug.LogError(e);
                tasks = new List<TaskGraph>();
                SaveTaskList();
            }
            windowController.Close();
        }

        public void AddTask(TaskGraph task)
        {
            // check in log and update
            if(task.inLog) return;
            task.inLog = true;

            // actually add to log (data and literal)
            tasks.Add(task);
            CreateBlerb(task);

            SaveTaskList();
        }
        private void CreateBlerb(TaskGraph task)
        {
            GameObject newBlerb = Instantiate(taskBlerbPrefab);
            newBlerb.transform.SetParent(taskContainer.transform, false);
            newBlerb.transform.SetAsFirstSibling();

            TaskBlerbController newBlerbController = newBlerb.GetComponent<TaskBlerbController>();
            newBlerbController.InitializeBlerb(this, task);
        }
        private void InitializeList()
        {
            ResetList();
            TaskGraph lastTask = null;
            foreach(TaskGraph task in tasks)
            {
                if(task == null || task.completed != showCompleted) continue;
                CreateBlerb(task);
                lastTask = task;
            }
            if(lastTask == null) return;
            // taskPanelController.HighlightedTask = lastTask;
        }
        private void ResetList()
        {
            foreach(Transform child in taskContainer.transform) Destroy(child.gameObject);
        }

        public bool GetShowCompleted()
        {
            return showCompleted;
        }
        public void ToggleShowComplete()
        {
            showCompleted = !showCompleted;
        }

        private void SaveTaskList()
        {
            List<TaskJson> saveList = new List<TaskJson>();

            foreach(TaskGraph task in tasks) saveList.Add(new TaskJson(task.name, task.currentNode));
            

            SaveLoad<List<TaskJson>>.SaveToJson(saveList, FILE_PATH);
        }
        private List<TaskGraph> LoadTaskList()
        {
            List<TaskJson> loadList = SaveLoad<List<TaskJson>>.LoadFromJson(FILE_PATH);
            List<TaskGraph> returnList = new List<TaskGraph>();

            foreach(TaskJson taskJson in loadList)
            {
                returnList.Add( Resources.Load<TaskGraph>("Tasks/" + taskJson.name) );
            }
            return returnList;
        }
        private void OnApplicationQuit() 
        {
            SaveTaskList();
        }
    }
    
}