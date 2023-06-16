using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode;

namespace Tasks.UI
{
    public class TaskPanelController : MonoBehaviour
    {
        [SerializeField] private Text taskName;
        [SerializeField] private Text taskDescription;

        [SerializeField] private GameObject nextArrow;
        [SerializeField] private GameObject previousArrow;

        [SerializeField] private TaskGraph currentTask;
        private TaskSegment _activeSegment;

        public void HighlightTask(TaskGraph task)
        {
            currentTask = task;
            _activeSegment = TaskController.GetCurrentNode(currentTask);
        }

        public void UpdateText()
        {
            taskName.text = _activeSegment.name;
            taskDescription.text = _activeSegment.description;
        }

        void Update()
        {
            if(NoTaskSelected()) return;

            nextArrow.SetActive(NextTaskAvailable());
            previousArrow.SetActive(PreviousTaskAvailable());
        }

        private bool NoTaskSelected()
        {
            return currentTask == null;
        }
        private bool NextTaskAvailable()
        {
            int index = 0;
            foreach(TaskGoal goal in _activeSegment.CompletionPaths)
            {
                NodePort port = _activeSegment.GetPort("Element " + index);
                if(port.IsConnected && (port.Connection.node as TaskSegment).completed)
                {
                    return true;
                }
            }
            return false;
        }
        private bool PreviousTaskAvailable()
        {
            if(_activeSegment.GetInputPort("input").IsConnected) return true;
            return false;
        }
        public void GoToPreviousNode()
        {
            _activeSegment = _activeSegment.GetInputPort("input").Connection.node as TaskSegment;
        }
        public void GoToNextNode()
        {
            int index = 0;
            foreach(TaskGoal goal in _activeSegment.CompletionPaths)
            {
                NodePort port = _activeSegment.GetPort("Element " + index);
                if((port.Connection.node as TaskSegment).completed) _activeSegment = port.Connection.node as TaskSegment;
            }
        }
    }
}
