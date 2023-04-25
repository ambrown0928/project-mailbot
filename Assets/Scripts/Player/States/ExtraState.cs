using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/**
 * A type of state that is regarded as an extra
 * state. This is used in the State Stack - a
 * pushdown automata - to add & remove states 
 * that the player is only in for a small amount
 * of time, or the player is generally not in
 * very often.
 * 
 * Inherited by DashingState, JumpingState, and
 * InventoryState
 */
[System.Serializable]
public class ExtraState : PlayerState
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
