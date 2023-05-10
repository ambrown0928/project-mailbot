using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private Dialogue[] dialogues;
        [HideInInspector] public Dialogue nextDialogue;
        public int currentDialogue;
        [SerializeField] private DialogueWindowController dialogueWindowController;

        public DialogueReporter TriggerDialogue()
        {
            nextDialogue.isDone = false;
            dialogueWindowController.CurrentDialogue = nextDialogue;
            return dialogueWindowController.dialogueReporter;
        }
        public void NextDialogue()
        {
            nextDialogue = dialogues[currentDialogue];
        }
        public void IncreaseDialogueStage()
        {
            if(currentDialogue + 1 < dialogues.Length) currentDialogue++;
        }
    }
}
