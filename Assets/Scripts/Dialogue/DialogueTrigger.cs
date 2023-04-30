using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private Dialogue dialogue;
        [SerializeField] private DialogueWindowController dialogueWindowController;

        public void TriggerDialogue()
        {
            dialogueWindowController.CurrentDialogue = dialogue;
        }
    }
}
