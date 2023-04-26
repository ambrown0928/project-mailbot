using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.States.Controllers
{
    public class StateController
    {
        private StateStack stateStack;
        private BaseState defaultState;
        private PlayerState currentState;
        private Rigidbody body;
    
        public StateController(BaseState defaultState, Rigidbody body)
        {
            this.defaultState = defaultState;
            stateStack = new StateStack(defaultState);
            ReplaceState(defaultState);
            this.body = body;
        }
    
        public StateStack StateStack { get => stateStack; set => stateStack = value; }
        public PlayerState CurrentState { get => currentState; set => currentState = value; }
    
        public void RunCurrentState()
        {
            CurrentState.Action(body);
        }
        public void RunCurrentState(Vector2 val)
        {
            CurrentState.Action(body, val);
        }
        public void RunCurrentState(GameObject obj)
        {
            CurrentState.Action(body, obj);
        }
        public void SetState(PlayerState new_state)
        {
            StateStack.Push(new_state);
        }
        public void CheckStateStack()
        {
            ReplaceState(stateStack.Pop());
        }
    
        private void ReplaceState(PlayerState state)
        {
            if(CurrentState != null)
            {
                if( PlayerIsInState(state.GetType()) ) return;
                CurrentState.Exit();
            }
            CurrentState = state;
            CurrentState.Enter();
        }
        public Type GetStateType()
        {
            return CurrentState.GetType();
        }
    
        public bool PlayerIsInState(Type state)
        {
            return GetStateType() == state;
        }
    }
    
}