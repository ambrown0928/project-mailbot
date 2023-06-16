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

        [SerializeField] private TaskWindowController taskWindowController;

        private void Awake() 
        {
            
        }

        private void Update() 
        {
            inProgressButton.interactable = taskWindowController.GetShowCompleted();
            completedButton.interactable = !taskWindowController.GetShowCompleted();
        }
    }
}
