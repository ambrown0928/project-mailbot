using System;
using System.Collections;
using System.Collections.Generic;
using System.UI;
using float_oat.Desktop90;
using UnityEngine;

namespace Dialog.Answer
{
    public class AnswerWindowController : MonoBehaviour
    {
        [Header("Controllers")]
        [SerializeField] private UIControllerGlobalContainer globalContainerUI;
        [SerializeField] private WindowController windowController;
        [Header("GameObjects")]
        [SerializeField] private GameObject answerPrefab;
        [SerializeField] private GameObject answerContainer;
        
        public void OpenAnswerWindow(List<string> answers, Action<int> answerClicked)
        {
            if(windowController.gameObject.activeInHierarchy) windowController.Expand();
            else windowController.Open();

            int index = 0;
            foreach(string answer in answers) { CreateBlerb(answer, index, answerClicked); index++; }
        }

        public void CreateBlerb(string answer, int index, Action<int> answerClicked)
        {
            GameObject answerButton = Instantiate(answerPrefab);
            answerButton.transform.SetParent(answerContainer.transform);
            
            AnswerBlerbController answerBlerbController = answerButton.GetComponent<AnswerBlerbController>();
            answerBlerbController.InitializeBlerb(answer, index, answerClicked);
        }
        public void CollapseWindow()
        {
            ResetList();
            windowController.Collapse();
        }
        public void CloseWindow()
        {
            ResetList();
            windowController.Close();
        }

        private void ResetList()
        {
            foreach (Transform child in answerContainer.transform) Destroy(child.gameObject);
        }

        public bool isActive()
        {
            return windowController.gameObject.activeInHierarchy;
        }
    }
}
