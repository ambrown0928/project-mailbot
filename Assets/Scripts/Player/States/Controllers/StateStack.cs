using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Creates a stack of ExtraStates to run in order of
 * most recently pushed to the stack. 
 */
public class StateStack
{
    private BaseState baseState; // base state for player to default to
    private List<ExtraState> stateStack = new List<ExtraState>();

    public StateStack(BaseState baseState)
    {
        this.baseState = baseState;
    }

    public bool IsEmpty()
    {
        return (stateStack.Count == 0);
    }

    public void Push(PlayerState newState)
    {
        if( StateIsBaseType(newState.GetType()) )
        {
            baseState = (BaseState) newState;
            return;
        }
        
        if( PlayerIsInTraversalState() ) return; 

        stateStack.Add( (ExtraState) newState );
    }

    public PlayerState Pop()
    {
        if(IsEmpty()) return baseState;
        
        ExtraState state = stateStack[stateStack.Count - 1];
        stateStack.Remove(state);
        return state;
    }

    private bool PlayerIsInTraversalState()
    {
        return baseState.GetType() == typeof(TraversalState);
    }

    private bool StateIsBaseType(Type state)
    {
        return state.IsSubclassOf(typeof(BaseState));
    }
}
