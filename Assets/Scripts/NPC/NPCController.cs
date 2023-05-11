using System.Collections;
using System.Collections.Generic;
using Dialogues;
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

        private DialogueObserver dialogueObserver;

        bool withinPlayer = false;

        void Awake()
        {
            dialogueTrigger = GetComponent<DialogueTrigger>();
            taskReciever = GetComponent<TaskReciever>();
            taskGiver = GetComponent<TaskGiver>();

            if(dialogueTrigger != null) 
            {
                tags.Add(NPCTags.Dialogue);
                dialogueObserver = new DialogueObserver();
            }
            if(taskReciever != null) tags.Add(NPCTags.TaskReciever);
            if(taskGiver != null) tags.Add(NPCTags.TaskGiver);
        }
        void OnTriggerEnter(Collider other) 
        {
            if( other.tag == "Player" )
            {
                withinPlayer = true;
            }
        }
        private void OnTriggerExit(Collider other) 
        {
            if( other.tag == "Player" )
            {
                withinPlayer = false;
            }    
        }
        
        void OnActivate()
        { // TODO - better implement task giver / dialogue system
            if(!withinPlayer) return; // stop if not within player
            
            bool taskWasProgressed = false;
            if( tags.Contains(NPCTags.TaskReciever) )
            {    
                taskWasProgressed = taskReciever.ProgressTask(nPCName); // result of progress task
                if(taskWasProgressed) dialogueTrigger.nextDialogue = taskReciever.recievedDialogue;
            }
            if( tags.Contains(NPCTags.Dialogue) ) 
            {
                if(!taskWasProgressed) 
                { 
                    // if conditions are met, advance to the next dialogue stage.
                    if(dialogueObserver.CurrentDialogue != null
                    && dialogueObserver.CurrentDialogue.isDone 
                    && dialogueObserver.CurrentDialogue.name.Equals(nPCName)
                    && dialogueObserver.CurrentDialogue.advanceToNextWhenDone) dialogueTrigger.IncreaseDialogueStage(); 

                    dialogueTrigger.NextDialogue();
                }
                DialogueReporter dialogueReporter = dialogueTrigger.TriggerDialogue();
                dialogueObserver.Subscribe(dialogueReporter);

            }
                
            if( tags.Contains(NPCTags.TaskGiver) && dialogueObserver.CurrentDialogue.isDone
                && dialogueObserver.CurrentDialogue.name == nPCName ) 
                taskGiver.GiveTask();
        }

    }
    public enum NPCTags
    {
        Dialogue,
        TaskReciever,
        TaskGiver
    }
}
