using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * State for when the player is jumping. Applies a 
 * force in the global up direction, using a defined
 * jump force to apply to the vector.
 * 
 * Can transition to: MovingState, IdleState, DeathState,
 * TraversalState, and DashingState.
 * 
 * Transition to criteria: jump button, player is on the ground,
 * player is not in TraversalState or DeathState.
 * Transition from criteria: automatically transitions away from 
 * state after jump action is done.
 */
public class JumpingState : ExtraState
{    
    private float jumpForce = 20f;
    
    public override void Enter () { Debug.Log("Entered Jumping State"); }
    public override void Exit () { Debug.Log("Exited Jumping State"); }

    /**
     * Adds
     */
    public override void Action(Rigidbody body)
    {
        body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
