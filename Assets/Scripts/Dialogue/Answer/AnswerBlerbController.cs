using System;
using System.Collections;
using System.Collections.Generic;
using float_oat.Desktop90;
using Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog.Answer
{
    public class AnswerBlerbController : MonoBehaviour
    {
        [SerializeField] private Text answerField;
        private int index;
        Action<int> answerClicked;

        public void InitializeBlerb(string answer, int index, DialogSegment nextSegment, Action<int> answerClicked)
        {
            // we want to stop showing the button if the task isn't available, or the task is completed
            if( nextSegment != null  && 
                nextSegment.GetType() == typeof( RecieveTaskSegment )  && 
                (( nextSegment as RecieveTaskSegment ).task.completed ||
                !((( nextSegment as RecieveTaskSegment ).task.graph as TaskGraph ).inLog ) ||
                ( nextSegment as RecieveTaskSegment ).task.id != (( nextSegment as RecieveTaskSegment ).task.graph as TaskGraph).currentNode )) 
                Destroy(this.gameObject); 

            answerField.text = answer;
            this.index = index;
            this.answerClicked = answerClicked;

            if(nextSegment != null && nextSegment.visited == true && answer != "Return") 
            {
                GetComponent<D90Button>().interactable = false;
            }
        }

        public void SelectAnswer()
        {
            answerClicked(index);
        }
    }
}
