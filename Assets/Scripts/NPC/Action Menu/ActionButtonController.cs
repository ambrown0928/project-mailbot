using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NPC.ActionMenu
{
    public class ActionButtonController : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Text buttonTitle;
        private Action buttonAction;
        private Action closeWindow;

        public void OnPointerClick(PointerEventData eventData)
        {
            buttonAction();
            closeWindow();
        }
        public void InitializeActionButton(string buttonTitle, Action buttonAction, Action closeWindow)
        {
            this.buttonTitle.text = buttonTitle;
            this.buttonAction = buttonAction;
            this.closeWindow = closeWindow;
        }
    }
}
