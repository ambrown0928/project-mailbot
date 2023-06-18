using System;
using System.Collections;
using System.Collections.Generic;
using GlobalNodes;
using Inventory;
using Inventory.Items;
using Saving;
using UnityEngine;
using XNode;

namespace Tasks
{
    /// 
    /// Static class for controlling tasks and task progress. Has 
    /// functionality for attempting to progress a task, checking
    /// if a hidden task has been completed, completing a task, 
    /// and getting nodes from the graph.
    /// 
    public static class TaskController
    {
        private const string FILE_PATH = "/Tasks/task-log.json";

        public static List<TaskGraph> tasks;
        private static TaskGoal _hiddenGoal;
        
        /// 
        /// Attempt to progress a task. Inventory controller is needed
        /// for deliveries so items can be checked. goal is the current 
        /// goal to be checked. Called by RecieveTaskSegment in Dialog
        /// 
        public static void AttemptProgress(InventoryController inventoryController, TaskGoal goal)
        {
            switch (goal.goalType)
            {
                case GoalType.Delivery:
                    Item itemRequest = new Item(goal.target, goal.requiredAmount);
                    try
                    { // try to deliver item
                        inventoryController.CanTakeItem(itemRequest); // throws NullReferenceException
                        
                        // item can be delivered, take item & complete goal
                        Item item = inventoryController.TakeItem(itemRequest);
                        if (item.ItemPrototype is PackagePrototype) ManagePackageDeliveryGoal(item.ItemPrototype as PackagePrototype);
                        goal.Complete();
                        
                    }
                    catch (System.NullReferenceException)
                    {
                        Debug.Log("Delivery Failed. Couldn't find the item in the inventory");
                    }
                    return;
                case GoalType.Talk:
                    goal.Complete();
                    return;
                case GoalType.Hidden:
                    _hiddenGoal = goal; 
                    return;
            }   
        }
        /// 
        /// Manages the delivery when the item is a package. Checks 
        /// for a hidden goal and attempts to complete it. 
        /// 
        private static void ManagePackageDeliveryGoal(PackagePrototype packagePrototype)
        {
            if(packagePrototype.currentItem == null) return;
            if(CheckHiddenGoal(packagePrototype)) return; 
            // todo : make package change text on certain items(should it be a task segment or a task?)
        }
        /// 
        /// Checks if the task has a hidden goal and attempts to complete
        /// the goal if so.
        /// 
        /// Returns a boolean for if the goal was completed successfully
        /// or not.
        /// 
        private static bool CheckHiddenGoal(PackagePrototype packagePrototype)
        {
            if(_hiddenGoal == null) return false;

            if( packagePrototype.currentItem.Name == _hiddenGoal.target &&
                packagePrototype.currentItem.Quantity >= _hiddenGoal.requiredAmount )
            {
                _hiddenGoal.Complete();
                return true;
            }
            return false;
        }
        /// 
        /// Runs when a task is completed. 
        /// 
        public static void CompleteTaskSegment(TaskSegment taskSegment, int index)
        {
            taskSegment.completed = true;
            
            NodePort port = taskSegment.GetPort("CompletionPaths " + index);

            if(port.IsConnected) UpdateTaskGraph(
                taskSegment.graph as TaskGraph, 
                port.Connection.node
            ); // set the currentNode of the graph to the new node
            else (taskSegment.graph as TaskGraph).completed = true; // task has no more steps and is completed
        }
        public static void UpdateTaskGraph(TaskGraph graph, Node node)
        {
            if(node is GiveTaskSegment)
            {
                UpdateTaskGraph(node.graph as TaskGraph, (node as GiveTaskSegment).AttemptToAddToLog());
                return;
            }
            graph.currentNode = (node as TaskSegment).id;
        }
        /// 
        /// Gets the first node of the graph. 
        /// 
        /// Returns the first node of the graph.
        /// Throws exception if no start node can be found
        /// 
        public static TaskSegment GetFirstNode(TaskGraph task)
        {
            foreach(TaskSegment node in task.nodes) 
            {
                if(node.GetInputPort("input").IsConnected) continue;
                
                task.currentNode = 0;
                return(node);   
            }
            throw new NodeHasNoStartException();
        }
        // 
        // Gets the current node of the graph.
        // 
        // Returns the current node of the graph
        // Throws exception if task.currentNode doesn't exist in graph.
        // 
        public static TaskSegment GetCurrentNode(TaskGraph task)
        {
            foreach(TaskSegment node in task.nodes) if(task.currentNode == node.id) return node;
            throw new GraphHasNoCurrentNodeException();
        }

        public static bool AddToLog(TaskGraph task)
        {
            if(task.inLog) return false;
            task.inLog = true;

            tasks.Add(task);
            SaveTaskList();
            return true;
        }

        public static void SaveTaskList()
        { // save tasks to a file
            List<TaskJson> saveList = new List<TaskJson>();

            foreach(TaskGraph task in tasks) saveList.Add(new TaskJson(task.name, false));
            SaveLoad<List<TaskJson>>.SaveToJson(saveList, FILE_PATH);
        }
        public static List<TaskGraph> LoadTaskList()
        { // load tasks and transfer to new list
            List<TaskJson> loadList = SaveLoad<List<TaskJson>>.LoadFromJson(FILE_PATH);
            List<TaskGraph> returnList = new List<TaskGraph>();

            foreach(TaskJson taskJson in loadList) returnList.Add( Resources.Load<TaskGraph>("Tasks/" + taskJson.name) ); // TODO replace with asset bundle
            
            return returnList;
        }
    }
}