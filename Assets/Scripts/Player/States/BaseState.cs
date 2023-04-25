using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * This is a type of state that is regarded as a 
 * base state. This is a state that is part of the
 * base finite automata machine, and can affect whether
 * the State Stack can be used.
 * 
 * Inherited by IdleState, MovingState, DeathState and 
 * TraversalState.
 */
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
