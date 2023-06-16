using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Tasks
{
    [Serializable]
    public struct Connection { }
    public class TaskSegment : Node
    {
        [Input] public Connection input;
        public int id;

        public string description;
        
        public bool completed;

        [Output(dynamicPortList = true)] public List<TaskGoal> CompletionPaths;

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}
