using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * State that runs when player is in traversal mode, which
 * increases the hover length, disables the state stack, 
 * and makes the player move in a faster, more vehicle-like
 * manner. 
 * 
 * Can transition to: MovingState, IdleState, and DeathState.
 * 
 * Transition to criteria: traversal mode button.
 * Transition from criteria: traversal mode button, health <= 0
 */
public class TraversalState : BaseState
{
    private float traversalSpeed = 60f; // speed of acceleration
    private float maxSpeed = 50f; // maximum speed
    private float friction = 25f; // friction co-efficient
    
    public override void Enter () 
    {
        Debug.Log("Entered Traversal State");     
    }
    public override void Exit () { Debug.Log("Exited Traversal State"); }
    
    /**
     * Creates a Vector3 that points in the direction of the rigidbody
     * multiplied by the player's input and the acceleration. Adds this
     * vector to the rigidbody's velocity & clamps said velocity using
     * the max speed. Then, applies friction to the rigidbody.
     * 
     * Parameters: 
     * Rigidbody body - the player's rigidbody. 
     * Vector2 input - the input vector for the player (y-axis = up/down, 
     * x-axis = left/right.)
     */
    public override void Action(Rigidbody body, Vector2 input)
    {
        Vector3 movement = ( body.transform.forward * input.y * Time.fixedDeltaTime
                    + body.transform.right * input.x * Time.fixedDeltaTime ) * traversalSpeed;
        
        body.velocity += movement;

        body.velocity = Vector3.ClampMagnitude(body.velocity, maxSpeed);
        ApplyFriction(body);
    }

    /**
     * Creates a Vector3 in the opposite x and z directions of the 
     * rigidbody's velocity. Checks to see if the magnitude is greater
     * than a certain inputue, then normalizes the vector and applies the 
     * friction coefficient to the vector (multiplied by fixedDeltaTime)
     * and adds this vector to the velocity.
     * 
     * Parameters: 
     * Rigidbody body - the player's rigidbody.
     */
    private void ApplyFriction(Rigidbody body)
    {
        Vector3 frictionDir = new Vector3(-body.velocity.x, 0f, -body.velocity.z);   
        if(Mathf.Abs(frictionDir.magnitude) < 0.1f) return;

        frictionDir.Normalize();
        frictionDir *= friction * Time.fixedDeltaTime;
        body.velocity += frictionDir;
    }
}
