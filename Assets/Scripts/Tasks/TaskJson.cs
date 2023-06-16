using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasks
{
    public class TaskJson
    {
        public string name;
        public int currentSegment;

        public TaskJson(string name, int currentSegment)
        {
            this.name = name;
            this.currentSegment = currentSegment;
        }
    }
}
