using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * State for when the player isn't moving / is idle. Runs
 * an action that makes sure the player isn't moving in the
 * x or z directions. 
 * 
 * Can transition to: MovingState, TraversalState, JumpingState,
 * DeathState, InventoryState, and DashingState.
 * 
 * Transition to criteria: input vector = 0, player not in traversal
 * mode.
 * Transition from criteria: input vector.abs > 0, traversal button,
 * jump button, dash button, inventory button, health <= 0
 */
public class IdleState : BaseState
{
    public override void Enter() { Debug.Log("Entered Idle State"); }
    public override void Exit() { Debug.Log("Exited Idle State"); }
    /**
     * Sets the rigidbody's velocity in the x and z positions to
     * 0.
     * 
     * Parameters:
     * Rigidbody body - the player's rigidbody
     */
    public override void Action(Rigidbody body)
    {
        base.Action(body);
        body.velocity = new Vector3(0f, body.velocity.y, 0f);
    }
}
