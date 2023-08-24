using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Dialog
{
    [System.Serializable]
    public struct Connection { }
    public class DialogSegment : Node
    {
        [Input] public Connection input;
        

        public bool visited;
        public string DialogText;
        public AudioClip npcSound;
        
        [Output(dynamicPortList = true)] public List<string> Answers;

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}
