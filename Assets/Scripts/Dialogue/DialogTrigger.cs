using System.Collections;
using System.Collections.Generic;
using NPC;
using UnityEngine;

namespace Dialog
{
    public class DialogTrigger : MonoBehaviour
    {
        [SerializeField] private DialogWindowController dialogWindowController;
        private NPCController nPCController;
        [SerializeField] private DialogGraph dialog;

        private void Awake() 
        {
            nPCController = GetComponent<NPCController>();
        }
        public void TriggerDialog()
        {
            dialogWindowController.OpenWindow(dialog, nPCController.nPCName);
        }
    }
    
}