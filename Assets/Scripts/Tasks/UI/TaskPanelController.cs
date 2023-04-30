using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tasks.UI
{
    public class TaskPanelController : MonoBehaviour
    {
        [SerializeField] private Text taskName;
        [SerializeField] private Text taskDescription;
        [SerializeField] private GameObject nextArrow;
        [SerializeField] private GameObject previousArrow;
        [SerializeField] private Task highlightedTask;
        [SerializeField] private int localCurrentTask;

        public Task HighlightedTask { get => highlightedTask; 
            set 
            {
                localCurrentTask = value.CurrentTask;
                highlightedTask = value; 
            }
        }

        void Update()
        {
            if(NoTaskSelected()) return;
            
            taskName.text = highlightedTask.TaskStages[localCurrentTask].name;
            taskDescription.text = highlightedTask.TaskStages[localCurrentTask].description;

            nextArrow.SetActive(LocalCurrentTaskLessThanHighlightedCurrentTask());
            previousArrow.SetActive(CurrentTaskGreaterThanZero());
        }

        private bool NoTaskSelected()
        {
            return highlightedTask == null;
        }
        private bool LocalCurrentTaskLessThanHighlightedCurrentTask()
        {
            return localCurrentTask < highlightedTask.CurrentTask;
        }
        private bool CurrentTaskGreaterThanZero()
        {
            return localCurrentTask > 0;
        }

        public void IncreaseLocalCurrentTask()
        {
            localCurrentTask++;
        }
        public void DecreaseLocalCurrentTask()
        {
            localCurrentTask--;
        }
    }
}
