using System.Collections;
using System.Collections.Generic;
using System;
using System.UI;
using float_oat.Desktop90;
using UnityEngine;
using UnityEngine.UI;
using XNode;

namespace Dialog
{
    public class DialogWindowController : MonoBehaviour
    {
        private DialogGraph dialogGraph;

        [Header("UI")]
        [SerializeField] private Text dialogName;
        [SerializeField] private Text dialogText;
        private DialogSegment _activeSegment;

        public List<DialogSegment> NextSegments 
        { 
            get  
            {
                List<DialogSegment> nextSegments = new List<DialogSegment>();
                foreach(NodePort port in _activeSegment.DynamicOutputs)
                {
                    if(!port.IsConnected) { nextSegments.Add(default(DialogSegment)); break;}
                    nextSegments.Add(port.Connection.node as DialogSegment);
                }
                return nextSegments;
            } 
        }

        [Header("Timing")]
        [SerializeField] private float dialogSpeed;
        [SerializeField] private float answerWaitTime;

        [Header("Controllers")]
        [SerializeField] private WindowController windowController;
        [SerializeField] private UIControllerGlobalContainer globalContainerUI;

        private bool skipLoading;
        private int answerIndex = -1;
        

        void Awake()
        {
            windowController.Close();
        }
        private void ResetDialogGraph()
        {
            foreach(DialogSegment node in dialogGraph.nodes) node.visited = false;
        }
        private void GetFirstNode()
        {
            foreach(DialogSegment node in dialogGraph.nodes) 
                if(!node.GetInputPort("input").IsConnected) { UpdateDialog(node); break; }
        }
        private void UpdateDialog(DialogSegment newSegment)
        {
            if(globalContainerUI.AnswerWindowController.isActive()) 
                globalContainerUI.AnswerWindowController.CollapseWindow();

            _activeSegment = newSegment;
            _activeSegment.visited = true;

            switch(_activeSegment)
            {
                case RecieveTaskSegment:
                    RecieveTask();
                    break;
                case DialogSegment :
                    StartCoroutine(LoadDialog(_activeSegment.DialogText));
                    break;
            }
        }

        private void RecieveTask()
        {
            RecieveTaskSegment _currentSegment = _activeSegment as RecieveTaskSegment;

            _currentSegment.AttemptProgressTask(dialogName.name, globalContainerUI.InventoryController);
            StartCoroutine( LoadDialog( _currentSegment.CheckIfTaskIsCompleteAndReturnDialogResult() ) );
        }

        private IEnumerator LoadDialog(string sentence)
        {
            answerIndex = -1;
            skipLoading = false;
            dialogText.text = "";

            for(int i = 0; i < sentence.Length; i++)
            {
                if(skipLoading) // skip loading text
                {
                    dialogText.text += sentence.Substring(i);
                    break;
                }

                dialogText.text += sentence[i];
                yield return new WaitForSeconds(dialogSpeed);
            }
            yield return new WaitForSeconds(answerWaitTime);
            GenerateAnswers();
        }

        private void GenerateAnswers()
        {
            globalContainerUI.AnswerWindowController.OpenAnswerWindow(_activeSegment.Answers, AnswerClicked);
        }                
        public void AnswerClicked(int answerIndex)
        {
            this.answerIndex = answerIndex;
            NodePort port = _activeSegment.GetPort("Answers " + answerIndex);
            
            if(port.IsConnected) UpdateDialog(port.Connection.node as DialogSegment);
            else CloseWindow();
        }

        public void OpenWindow(DialogGraph newDialog, string name)
        {
            if(gameObject.activeInHierarchy) throw new DialogWindowIsOpenException();
            windowController.Open();

            dialogGraph = newDialog;
            ResetDialogGraph();

            dialogName.text = name;
            GetFirstNode();
        }
        private void CloseWindow()
        {
            dialogName.text = "";

            windowController.Close();
            globalContainerUI.AnswerWindowController.CloseWindow();
        }
        
        public void SkipLoading()
        {
            skipLoading = true;
        }
    }
    
}