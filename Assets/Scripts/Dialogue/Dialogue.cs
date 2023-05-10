using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    /// 
    /// Class that stores information for dialogue
    /// 
    [System.Serializable]
    public class Dialogue
    {
        public string name;
        public bool isDone = false;
        public bool advanceToNextWhenDone = false;
        [TextArea(3, 10)] public string[] sentences;
    }
}
