using System.Collections;
using System.Collections.Generic;
using float_oat.Desktop90;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class DialogueWindowController : MonoBehaviour
    {
        private WindowController windowController;
        private Dialogue currentDialogue;
        private Coroutine speaking;

        [SerializeField] private float dialogueSpeed;
        [SerializeField] private float buttonWaitTime;
        [SerializeField] private GameObject nextButton;
        [SerializeField] private GameObject endButton;
        [SerializeField] private Text nPCName;
        [SerializeField] private Text dialogueBox;

        private bool goToNextSentence;
        private bool skipLoading;

        public Dialogue CurrentDialogue { get => currentDialogue; set => currentDialogue = value; }

        private void Awake() 
        {
            windowController = GetComponent<WindowController>();
            windowController.Close();    
        }
        private void Update()
        {
            if (NoDialogue()) return;

            nPCName.text = currentDialogue.name;
            if (SpeakingHasNotStarted()) 
            {
                windowController.Open();
                speaking = StartCoroutine(LoadDialogue());
            }
        }

        private bool SpeakingHasNotStarted()
        {
            return speaking == null;
        }

        private bool NoDialogue()
        {
            return CurrentDialogue == null;
        }

        private IEnumerator LoadDialogue()
        {
            // load sentences into a queue for easy FIFO
            Queue<string> sentences = new Queue<string>();
            foreach(string sentence in currentDialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            // load dialogue by letter
            while(sentences.Count > 0)
            {
                nextButton.SetActive(false);
                endButton.SetActive(false);

                skipLoading = false;
                goToNextSentence = false;

                dialogueBox.text = "";
                string sentence = sentences.Dequeue();

                for(int i = 0; i < sentence.Length; i++)
                {
                    if(skipLoading) // skip loading text
                    {
                        dialogueBox.text += sentence.Substring(i);
                        break;
                    }
                    dialogueBox.text += sentence[i];
                    yield return new WaitForSeconds(dialogueSpeed);
                }
                yield return new WaitForSeconds(buttonWaitTime);
                if(sentences.Count == 0)
                {
                    endButton.SetActive(true);
                }
                else
                {
                    nextButton.SetActive(true);
                }
                while(!goToNextSentence) { yield return null; } // lock coroutine while player is staying on this sentence
            }
            CurrentDialogue = null;
            speaking = null;
            windowController.Close();
        }

        public void ActivateSkipLoading()
        {
            skipLoading = true;
        }
        public void GoToNextSentence()
        {
            goToNextSentence = true;
        }

    }
}
