using System;
using System.Collections;
using System.Collections.Generic;
using GlobalNodes;
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

        [SerializeField] public TaskGraph currentTask;
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

            UpdateText();
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
                if (goal.goalType == GoalType.Hidden) 
                    continue;
                if (GetNode("CompletionPaths " + index, false) != null)
                    return true;
                
            }
            return false;
        }
        private bool PreviousTaskAvailable()
        {
            if(_activeSegment.GetInputPort("input").IsConnected) 
                return true;
            return false;
        }
        public void GoToPreviousNode()
        {
            _activeSegment = GetNode("input", true);
        }
        public void GoToNextNode()
        {
            int index = 0;
            foreach(TaskGoal goal in _activeSegment.CompletionPaths)
            {
                Node node = GetNode("CompletionPaths " + index, false);
                if( node != null ) 
                    _activeSegment = node as TaskSegment;
            }
        }
        private TaskSegment GetNode(string portName, bool isInput)
        {
            NodePort port = _activeSegment.GetPort(portName);
            if(!port.IsConnected) // skip if no next node
                return null;

            Node node = port.Connection.node;
            if(node is GiveTaskSegment) // skip GiveTaskSegment, get real TaskSegment
                node = (isInput) ? node.GetInputPort("input").Connection.node : node.GetOutputPort("output").Connection.node;
            if(!(node as TaskSegment).completed && (node as TaskSegment).id != currentTask.currentNode) // next node isn't completed
                return null;
            return node as TaskSegment;
        }
    }
}
