using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/**
 * An interface defining the functions for all player 
 * states. Includes 3 action functions w/different 
 * parameters for any possible uses, and an action/exit
 * system. 
 * 
 * Inherited by BaseState and ExtraState.
 */

public interface PlayerState 
{
    // functions that run when state is active
    void Action(Rigidbody body);
    void Action(Rigidbody body, Vector2 val);
    void Action(Rigidbody body, GameObject obj);
    void Enter(); // runs when state is entered
    void Exit(); // runs when state is exited
}
