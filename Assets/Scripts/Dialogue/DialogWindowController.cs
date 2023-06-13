using System.Collections;
using System.Collections.Generic;
using System;
using System.UI;
using float_oat.Desktop90;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public class DialogWindowController : MonoBehaviour
    {
        private DialogGraph dialogGraph;

        [Header("UI")]
        [SerializeField] private Text dialogName;
        [SerializeField] private Text dialogText;
        private DialogSegment _activeSegment;

        [Header("Timing")]
        [SerializeField] private float dialogSpeed;
        [SerializeField] private float answerWaitTime;

        [Header("Controllers")]
        [SerializeField] private WindowController windowController;
        [SerializeField] private UIControllerGlobalContainer globalContainerUI;

        private bool skipLoading;
        private int answerIndex = -1;
        
        private IEnumerator LoadDialog()
        {
            answerIndex = -1;
            skipLoading = false;

            dialogText.text = "";
            string sentence = _activeSegment.DialogText;

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
        public void AnswerClicked(int answerIndex)
        {
            this.answerIndex = answerIndex;
            
            var port = _activeSegment.GetPort("Answers " + answerIndex);
            if(port.IsConnected) UpdateDialog(port.Connection.node as DialogSegment);
            else CloseWindow();
        }
        private void GenerateAnswers()
        {
            globalContainerUI.AnswerWindowController.OpenAnswerWindow(_activeSegment.Answers, AnswerClicked);
        }
        void Awake()
        {
            windowController.Close();
        }
        private void UpdateDialog(DialogSegment newSegment)
        {
            if(globalContainerUI.AnswerWindowController.isActive()) globalContainerUI.AnswerWindowController.CollapseWindow();
            _activeSegment = newSegment;
            StartCoroutine(LoadDialog());
        }
        private void GetGraphFirstNode()
        {
            foreach(DialogSegment node in dialogGraph.nodes)
            {
                if(!node.GetInputPort("input").IsConnected)
                {
                    UpdateDialog(node);
                    break;
                }
            }
        }
        public void OpenWindow(DialogGraph newDialog, string name)
        {
            if(gameObject.activeInHierarchy) throw new DialogWindowIsOpenException();
            windowController.Open();
            dialogGraph = newDialog;
            dialogName.text = name;
            GetGraphFirstNode();
        }
        private void CloseWindow()
        {
            dialogName.text = "";

            windowController.Close();
            globalContainerUI.AnswerWindowController.CloseWindow();
        }
    }
    
}