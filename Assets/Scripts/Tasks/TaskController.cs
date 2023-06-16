using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Items;
using UnityEngine;
using XNode;

namespace Tasks
{
    public static class TaskController
    {
        private static TaskGoal _hiddenGoal;

        public static void AttemptProgress(string npc, InventoryController inventoryController, TaskGoal goal)
        {
            
            switch (goal.goalType)
            {
                case GoalType.Delivery:
                    Item itemRequest = new Item(goal.target, goal.requiredAmount);
                    try
                    {
                        inventoryController.CanTakeItem(itemRequest); // throws NullReferenceException
                        
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
        private static void ManagePackageDeliveryGoal(PackagePrototype packagePrototype)
        {
            if(packagePrototype.currentItem == null) return;
            if(CheckHiddenGoal(packagePrototype)) return;
            // todo : make package change text on certain items(should it be a task segment or a task?)
        }
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
        public static void CompleteTaskSegment(TaskSegment task, int index)
        {
            task.completed = true;
            
            NodePort port = task.GetPort("CompletionPaths " + index);

            if(port.IsConnected) UpdateTaskGraph(
                task.graph as TaskGraph, 
                port.Connection.node as TaskSegment
            );
            else (task.graph as TaskGraph).completed = true;
        }
        public static void UpdateTaskGraph(TaskGraph graph, TaskSegment node)
        {
            graph.currentNode = node.id;
        }

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
        public static TaskSegment GetCurrentNode(TaskGraph task)
        {
            foreach(TaskSegment node in task.nodes) if(task.currentNode == node.id) return node;
            throw new GraphHasNoCurrentNodeException();
        }
    }
}