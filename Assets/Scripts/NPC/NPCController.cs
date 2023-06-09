using System.Collections;
using System.Collections.Generic;
using Dialogues;
using NPC.ActionMenu;
using Tasks;
using UnityEngine;

namespace NPC
{
    public class NPCController : MonoBehaviour
    {
        public string nPCName;
        public List<NPCTags> tags;

        private DialogueTrigger dialogueTrigger;
        private TaskReciever taskReciever;
        private TaskGiver taskGiver;

        [SerializeField] private ActionMenuController actionMenuController;
        [SerializeField] private GameObject actionMenuOffset;
        private DialogueObserver dialogueObserver;
        bool withinPlayer = false;

        void Awake()
        {
            dialogueTrigger = GetComponent<DialogueTrigger>();
            taskReciever = GetComponent<TaskReciever>();
            taskGiver = GetComponent<TaskGiver>();

            // setting tags 
            if(dialogueTrigger != null) 
            {
                tags.Add(NPCTags.Dialogue);
                dialogueObserver = new DialogueObserver();
            }
            if(taskReciever != null) tags.Add(NPCTags.TaskReciever);
            if(taskGiver != null) tags.Add(NPCTags.TaskGiver);

            dialogueTrigger.SetDialogueName(nPCName);
        }
        void OnTriggerEnter(Collider other) 
        {
            if( other.tag == "Player" ) withinPlayer = true;   
        }
        void OnTriggerExit(Collider other) 
        {
            if( other.tag == "Player" ) 
            {
                withinPlayer = false;
                actionMenuController.CloseWindow();
            }  
        }

        void OnActivate()
        { // TODO - better implement task giver / dialogue system
            if (!withinPlayer || GetActionMenuActiveInHierarchy()) return; // stop if not within player or action menu is opened

            actionMenuController.OpenWindow(this, actionMenuOffset);
        }
        private bool GetActionMenuActiveInHierarchy()
        {
            return actionMenuController.transform.parent.gameObject.activeInHierarchy;
        }

        public void GiveTask()
        {
            taskGiver.GiveTask();
        }
        public void Talk()
        {
            // if conditions are met, advance to the next dialogue stage.
            if (dialogueObserver.CurrentDialogue != null
            && dialogueObserver.CurrentDialogue.isDone
            && dialogueObserver.CurrentDialogue.name.Equals(nPCName)
            && dialogueObserver.CurrentDialogue.advanceToNextWhenDone) dialogueTrigger.IncreaseDialogueStage();

            dialogueTrigger.NextDialogue();

            DialogueReporter dialogueReporter = dialogueTrigger.TriggerDialogue();
            dialogueObserver.Subscribe(dialogueReporter);
        }
        public void RecieveTask()
        {
            bool taskWasProgressed = taskReciever.ProgressTask(nPCName); // result of progress task
            if (taskWasProgressed) dialogueTrigger.nextDialogue = taskReciever.recievedDialogue;
        }
    }
    public enum NPCTags
    {
        Dialogue,
        TaskReciever,
        TaskGiver
    }
}
