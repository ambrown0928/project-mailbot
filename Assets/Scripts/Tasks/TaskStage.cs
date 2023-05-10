using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasks
{
    [System.Serializable]
    public class TaskStage 
    {
        public int id;
        public string name;
        [TextArea(3, 10)] public string description;
        public TaskGoal goal;

        public TaskStage()
        {
        }

        public TaskStage(int id, string name, string description, TaskGoal goal)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.goal = goal;
        }
    }
}