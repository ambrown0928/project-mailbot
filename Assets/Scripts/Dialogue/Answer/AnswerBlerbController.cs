using System;
using System.Collections;
using System.Collections.Generic;
using float_oat.Desktop90;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog.Answer
{
    public class AnswerBlerbController : MonoBehaviour
    {
        [SerializeField] private Text answerField;
        private int index;
        Action<int> answerClicked;

        public void InitializeBlerb(string answer, int index, Action<int> answerClicked)
        {
            answerField.text = answer;
            this.index = index;
            this.answerClicked = answerClicked;
        }

        public void SelectAnswer()
        {
            answerClicked(index);
        }
    }
}
