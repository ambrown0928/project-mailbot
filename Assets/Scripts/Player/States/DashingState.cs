using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingState : ExtraState
{
    private float dashSpeed = 50f;
    public float dashTime = 0.25f;
    public float dashCooldown = 0.5f;
    public bool canDash = true;

    public override void Enter () { Debug.Log("Entering Dashing State"); }
    public override void Exit () { Debug.Log("Exiting Dashing State"); }

    public override void Action(Rigidbody body, Vector2 val)
    {
        
    }
    public override void Action(Rigidbody body, GameObject model)
    {
        body.velocity = model.transform.forward * dashSpeed;
    }
}
