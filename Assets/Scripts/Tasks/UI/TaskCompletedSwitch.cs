using System.Collections;
using System.Collections.Generic;
using float_oat.Desktop90;
using UnityEngine;

namespace  Tasks.UI
{
    public class TaskCompletedSwitch : MonoBehaviour
    {
        public D90Button inProgressButton;
        public D90Button completedButton;

        [SerializeField] private TaskLogController taskLogController;

        private void Awake() 
        {
            
        }

        private void Update() 
        {
            inProgressButton.interactable = taskLogController.GetShowCompleted();
            completedButton.interactable = !taskLogController.GetShowCompleted();
        }
    }
}
