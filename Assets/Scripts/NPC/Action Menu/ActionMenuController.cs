using System;
using System.Collections;
using System.Collections.Generic;
using float_oat.Desktop90;
using UnityEngine;
using UnityEngine.UI;

namespace NPC.ActionMenu
{    
    public class ActionMenuController : MonoBehaviour
    {   
        #region UI Fields
        [Header("UI Fields")]

            [SerializeField] private Text nPCName;
            [SerializeField] private GameObject actionButtonContainer;
            [SerializeField] private GameObject actionButtonPrefab;
        
        #endregion
        #region Component Fields
        [Header("Component Fields")]

            [SerializeField] private WindowController windowController;
            [SerializeField] private Camera mainCamera;
            [SerializeField] private RectTransform rectTransform;
        #endregion

        private NPCController currentNPCController;
        private GameObject currentNPC;

        void Awake()
        {
            windowController.Close();
        }
        void Update()
        {
            rectTransform.position = (currentNPC) ? mainCamera.WorldToScreenPoint(currentNPC.transform.position) : rectTransform.position;
        }
        public void OpenWindow(NPCController nPCController, GameObject nPC)
        {
            ResetList();
            windowController.Open();
            currentNPC = nPC;
            nPCName.text = nPCController.nPCName;

            foreach(NPCTags tag in nPCController.tags)
            {
                switch(tag)
                {
                    case NPCTags.Dialogue:
                        CreateActionButton("Talk", nPCController.Talk);
                        break;
                    case NPCTags.TaskReciever:
                        CreateActionButton("Turn in Task", nPCController.RecieveTask);
                        break;
                    case NPCTags.TaskGiver:
                        CreateActionButton("Accept Task", nPCController.GiveTask);
                        break;
                }
            }
        }
        private void CreateActionButton(string buttonTitle, Action buttonAction)
        {
            // instantiate object & set parent
            GameObject newActionButton = Instantiate(actionButtonPrefab);
            newActionButton.transform.SetParent(actionButtonContainer.transform, false);
            newActionButton.transform.SetAsFirstSibling();

            // get action button controller and initialize
            ActionButtonController newActionButtonController = newActionButton.GetComponent<ActionButtonController>();
            newActionButtonController.InitializeActionButton(buttonTitle, buttonAction, CloseWindow);
        }
        public void CloseWindow()
        {
            windowController.Close();
            currentNPC = null;
        }
        private void ResetList()
        {
            foreach(Transform child in actionButtonContainer.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}
