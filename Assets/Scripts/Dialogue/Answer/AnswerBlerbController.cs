using System;
using System.Collections;
using System.Collections.Generic;
using float_oat.Desktop90;
using GlobalNodes;
using Tasks;
using UnityEngine;
using UnityEngine.UI;
using XNode;

namespace Dialog.Answer
{
    public class AnswerBlerbController : MonoBehaviour
    {
        [SerializeField] private Text answerField;
        [SerializeField] private int index;
        Action<int> answerClicked;

        public void InitializeBlerb(string answer, int index, Node nextSegment, Action<int> answerClicked)
        {
            if (CheckReceiveSegmentInvalid(nextSegment))
                Destroy(this.gameObject);

            answerField.text = answer;
            this.index = index;
            this.answerClicked = answerClicked;
            
            if(nextSegment == null) return;

            if (!(nextSegment is GiveTaskSegment) &&
                (nextSegment as DialogSegment).visited == true && answer != "Return")
            {
                GetComponent<D90Button>().interactable = false;
                return;
            }

            NodePort port = (nextSegment.GetOutputPort("Answers 0"));
            if(port == null)
                return;
            
            if( port.IsConnected &&
                port.Connection.node is GiveTaskSegment &&
                (port.Connection.node as GiveTaskSegment).task.inLog)
                Destroy(this.gameObject);
        }

        private static bool CheckReceiveSegmentInvalid(Node nextSegment)
        { // we want to stop showing the button if the task isn't available, or the task is completed
            return  nextSegment != null &&
                    nextSegment is ReceiveTaskSegment &&
                    ((nextSegment as ReceiveTaskSegment).task.completed ||
                    !(((nextSegment as ReceiveTaskSegment).task.graph as TaskGraph).inLog) ||
                    (nextSegment as ReceiveTaskSegment).task.id != ((nextSegment as ReceiveTaskSegment).task.graph as TaskGraph).currentNode);
        }

        public void SelectAnswer()
        {
            answerClicked(index);
        }
    }
}
