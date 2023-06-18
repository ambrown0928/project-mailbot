using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasks
{
    /// 
    /// For saving task in task log. Saves the name and 
    /// the current segment of the task so reloading is easier
    /// 
    public class TaskJson
    {
        public string name;
        public bool selected;

        public TaskJson(string name, bool selected)
        {
            this.name = name;
            this.selected = selected;
        }
    }
}
