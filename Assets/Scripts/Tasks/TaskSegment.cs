using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Tasks
{
    /// 
    /// Class used to store information on different task
    /// segments, which are individual levels in a task that
    /// lead to a task progressing.
    /// 
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
