using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Tasks
{
    [CreateAssetMenu]
    public class TaskGraph : NodeGraph
    {
        public int currentNode;
        public bool inLog;
        public bool completed;
    }
}