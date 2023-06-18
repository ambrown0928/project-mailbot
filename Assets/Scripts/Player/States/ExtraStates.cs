using System.Collections;
using System.Collections.Generic;
using float_oat.Desktop90;
using Inventory;
using Tasks.UI;
using UnityEngine;
using UnityEngine.InputSystem;

/// 
/// <summary>
/// This file contains all the extra states the player can
/// </summary> 
/// 

namespace Player.States
{ 
    [System.Serializable]
    public class ExtraState : PlayerState
    {
        public virtual void Enter () { }
        public virtual void Exit () { }
        
        public virtual void Action(Rigidbody body) { }
        public virtual void Action(Rigidbody body, Vector2 val) { }
        public virtual void Action(Rigidbody body, GameObject obj) { }
    }
    
    public class JumpingState : ExtraState
    {    
        private float jumpForce = 20f;
        
        public override void Enter () { Debug.Log("Entered Jumping State"); }
        public override void Exit () { Debug.Log("Exited Jumping State"); }
        
        public override void Action(Rigidbody body)
        {
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    
    public class InventoryState : ExtraState
    {
        private InventoryController inventoryController;
    
        public override void Enter () { }
        public override void Exit () 
        {
            inventoryController.Close();
            inventoryController = null;
        }
        
        public override void Action (Rigidbody body, GameObject inventoryWindow)
        {
            body.velocity = new Vector3(0f, body.velocity.y, 0f);
            if(PlayerHasNotOpenedInventory())
            {
                inventoryController = inventoryWindow.GetComponentInChildren<InventoryController>();
                inventoryController.Open();
            }   
        }
    
        private bool PlayerHasNotOpenedInventory()
        {
            return inventoryController == null;
        }
    }

    public class TaskLogState : ExtraState
    {
        private TaskWindowController taskWindowController;
    
        public override void Enter () { }
        public override void Exit () 
        {
            taskWindowController.CloseWindow();
            taskWindowController = null;
        }
        
        public override void Action (Rigidbody body, GameObject taskWindow)
        {
            body.velocity = new Vector3(0f, body.velocity.y, 0f);
            if(PlayerHasNotOpenedTaskLog()) 
            {
                taskWindowController = taskWindow.GetComponent<TaskWindowController>();
                taskWindowController.OpenWindow();
            }
        }
    
        private bool PlayerHasNotOpenedTaskLog()
        {
            return taskWindowController == null;
        }
    }

    public class DashingState : ExtraState
    {
        private float dashSpeed = 50f;
        public float dashTime = 0.25f;
        public float dashCooldown = 0.5f;
        public bool canDash = true;
    
        public override void Enter () { Debug.Log("Entering Dashing State"); }
        public override void Exit () { Debug.Log("Exiting Dashing State"); }

        public override void Action(Rigidbody body, GameObject model)
        {
            body.velocity = model.transform.forward * dashSpeed;
        }
    }
    public class DialogState : ExtraState
    {
        public override void Enter () { Debug.Log("Entering Dialog State"); }
        public override void Exit () { Debug.Log("Exiting Dialog State"); }

        public override void Action(Rigidbody body)
        {
            body.velocity = new Vector3(0f, body.velocity.y, 0f);
        }
    }
}