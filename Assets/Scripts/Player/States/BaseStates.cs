using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

///
/// 
/// <summary> 
/// This file contains all the base states for the player. A base
/// state is a state that is run in the finite automata and replaces
/// the other state the player is in, rather than adding to the stack.
/// </summary>
/// 
/// 
namespace Player.States
{
    [System.Serializable]
    public class BaseState : PlayerState
    {
        public virtual void Enter () { }
        public virtual void Exit () { }

        public virtual void Action(Rigidbody body)
        {

        }
        public virtual void Action(Rigidbody body, Vector2 val)
        {

        }
        public virtual void Action(Rigidbody body, GameObject obj)
        {

        }
    }
    [System.Serializable]
    public class MovingState : BaseState
    {
        private float moveSpeed = 15f;
    
        public override void Enter () { Debug.Log("Entered Moving State"); }
        public override void Exit () { Debug.Log("Exited Moving State"); }
        
        public override void Action(Rigidbody body, Vector2 val)
        {
            Vector3 movement = ( body.transform.forward * val.y + 
                        body.transform.right * val.x ) * moveSpeed;
    
            movement.y = body.velocity.y;
    
            body.velocity = movement;
        }
    }
    public class IdleState : BaseState
    {
        public override void Enter() { Debug.Log("Entered Idle State"); }
        public override void Exit() { Debug.Log("Exited Idle State"); }
        
        public override void Action(Rigidbody body)
        {
            base.Action(body);
            body.velocity = new Vector3(0f, body.velocity.y, 0f);
        }
    }
    public class DeathState : BaseState
    {

        public override void Enter () { }
        public override void Exit () { }
    
        public override void Action(Rigidbody body, GameObject corpse)
        {
            GameObject clone = GameObject.Instantiate(corpse, body.position, body.rotation);
            clone.transform.parent = null;
        }
    }
    public class TraversalState : BaseState
    {
        private float traversalSpeed = 60f;
        private float maxSpeed = 50f;
        private float friction = 25f; // friction co-efficient
        
        public override void Enter () 
        {
            Debug.Log("Entered Traversal State");     
        }
        public override void Exit () { Debug.Log("Exited Traversal State"); }
        
        public override void Action(Rigidbody body, Vector2 input)
        {
            Vector3 movement = ( body.transform.forward * input.y * Time.fixedDeltaTime
                        + body.transform.right * input.x * Time.fixedDeltaTime ) * traversalSpeed;
            
            body.velocity += movement;
    
            body.velocity = Vector3.ClampMagnitude(body.velocity, maxSpeed);
            ApplyFriction(body);
        }
    
        private void ApplyFriction(Rigidbody body)
        {
            Vector3 frictionDir = new Vector3(-body.velocity.x, 0f, -body.velocity.z);   
            if(Mathf.Abs(frictionDir.magnitude) < 0.1f) return;
    
            frictionDir.Normalize();
            frictionDir *= friction * Time.fixedDeltaTime;
            body.velocity += frictionDir;
        }
    }
    
}
