using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/**
 * State for when the player is moving in a normal
 * fashion.
 * 
 * Can transition to: IdleState, TraversalState,
 * DeathState, JumpingState, DashingState, and 
 * InventoryState.
 * 
 * Transition to criteria: input vector.abs > 0
 * Transition from criteria: input vector = 0, 
 * traversal button, dash button, and jump button.
 */
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
